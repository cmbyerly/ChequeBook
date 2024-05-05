using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MikkiBook2.Data;
using ReactiveUI;

namespace MikkiBook2.ViewModels;

/// <summary>
///     The main window view model.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    ///     The current transaction amount.
    /// </summary>
    private string? currentAmount;

    /// <summary>
    ///     The current balance
    /// </summary>
    private string? currentBalance;

    /// <summary>
    ///     The current check number.
    /// </summary>
    private string? currentCheckNumber;

    /// <summary>
    ///     The current transaction description.
    /// </summary>
    private string? currentDescription;

    /// <summary>
    ///     The current identifier
    /// </summary>
    private long? currentId;

    /// <summary>
    ///     The current transaction reconciled date.
    /// </summary>
    private DateTimeOffset? currentReconciledDate;

    /// <summary>
    ///     The current transaction date.
    /// </summary>
    private DateTimeOffset? currentTransactionDate;

    /// <summary>
    ///     The current transaction type.
    /// </summary>
    private TransactionTypes? currentTransactionType;

    /// <summary>
    ///     The database context
    /// </summary>
    private readonly AccountContext? dbContext;

    /// <summary>
    ///     Whether the transaction is a credit.
    /// </summary>
    private bool isCredit;

    /// <summary>
    ///     New window view model
    /// </summary>
    public MainWindowViewModel()
    {
        Greeting = "Welcome to Mikki Book 2!";

        CurrentBalance = "$0.00";

        dbContext = new AccountContext();
        dbContext.Database.EnsureCreated();

        TransactionTypes = new List<TransactionTypes>(
            Data.TransactionTypes.GetTransactionTypes().OrderBy(x => x.TransactionTypeCode));

        TransactionList = new ObservableCollection<AccountTransaction>();
    }

    /// <summary>
    ///     Greeting message
    /// </summary>
    public string Greeting { get; set; }

    /// <summary>
    ///     The current identifier.
    /// </summary>
    public long? CurrentId
    {
        get => currentId;
        set => this.RaiseAndSetIfChanged(ref currentId, value);
    }

    /// <summary>
    ///     The current balance.
    /// </summary>
    public string? CurrentBalance
    {
        get => currentBalance;
        set => this.RaiseAndSetIfChanged(ref currentBalance, value);
    }

    /// <summary>
    ///     The current transaction description.
    /// </summary>
    public string? CurrentDescription
    {
        get => currentDescription;
        set => this.RaiseAndSetIfChanged(ref currentDescription, value);
    }

    /// <summary>
    ///     The current transaction type.
    /// </summary>
    public TransactionTypes? CurrentTransactionType
    {
        get => currentTransactionType;
        set => this.RaiseAndSetIfChanged(ref currentTransactionType, value);
    }

    /// <summary>
    ///     The current transaction date.
    /// </summary>
    public DateTimeOffset? CurrentTransactionDate
    {
        get => currentTransactionDate;
        set => this.RaiseAndSetIfChanged(ref currentTransactionDate, value);
    }

    /// <summary>
    ///     The current transaction reconciled date.
    /// </summary>
    public DateTimeOffset? CurrentReconciledDate
    {
        get => currentReconciledDate;
        set => this.RaiseAndSetIfChanged(ref currentReconciledDate, value);
    }

    /// <summary>
    ///     The current transaction amount.
    /// </summary>
    public string? CurrentAmount
    {
        get => currentAmount;
        set => this.RaiseAndSetIfChanged(ref currentAmount, value);
    }

    /// <summary>
    ///     The current check number.
    /// </summary>
    public string? CurrentCheckNumber
    {
        get => currentCheckNumber;
        set => this.RaiseAndSetIfChanged(ref currentCheckNumber, value);
    }

    /// <summary>
    ///     Whether the transaction is a credit.
    /// </summary>
    public bool IsCredit
    {
        get => isCredit;
        set => this.RaiseAndSetIfChanged(ref isCredit, value);
    }

    /// <summary>The transaction types</summary>
    public List<TransactionTypes> TransactionTypes { get; set; }

    /// <summary>The transaction list</summary>
    public ObservableCollection<AccountTransaction> TransactionList { get; set; }

    /// <summary>
    ///     Rebuilds the transaction list.
    /// </summary>
    public Task RebuildTransactions()
    {
        var balance = 0.00M;
        TransactionList.Clear();

        using (var tempContext = new AccountContext())
        {
            tempContext.Database.EnsureCreated();
            foreach (var trans in tempContext.AccountTransactions.OrderByDescending(x => x.TransactionDate))
            {
                balance = balance + trans.Amount;
                TransactionList.Add((AccountTransaction)trans.Clone());
            }

            CurrentBalance = $"${balance.ToString("N2")}";
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Creates a new record.
    /// </summary>
    /// <returns></returns>
    public Task NewRecord()
    {
        CurrentId = null;
        CurrentDescription = string.Empty;
        CurrentTransactionType = null;
        CurrentTransactionDate = DateTimeOffset.Now;
        CurrentReconciledDate = null;
        CurrentAmount = string.Empty;
        CurrentCheckNumber = string.Empty;
        IsCredit = false;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Saves the record
    /// </summary>
    /// <returns>The number of records impacted.</returns>
    public Task<int> SaveRecord()
    {
        var reval = 0;

        if (currentId == null || currentId == 0)
        {
            var accountTransaction = new AccountTransaction(
                0,
                CurrentDescription,
                CurrentTransactionType.TransactionTypeCode,
                CurrentTransactionDate.Value.DateTime,
                CurrentReconciledDate?.DateTime,
                IsCredit
                    ? Math.Abs(Convert.ToDecimal(CurrentAmount))
                    : Math.Abs(Convert.ToDecimal(CurrentAmount)) * -1,
                CurrentCheckNumber);

            dbContext.AccountTransactions.Add(accountTransaction);
            reval = dbContext.SaveChanges();
        }
        else
        {
            var accountTransaction = dbContext.AccountTransactions.First(x => x.Id == currentId);
            accountTransaction.TransactionDate = CurrentTransactionDate.Value.DateTime;
            accountTransaction.TransType = CurrentTransactionType.TransactionTypeCode;
            accountTransaction.CheckNumber = CurrentCheckNumber;
            accountTransaction.Amount = IsCredit
                ? Math.Abs(Convert.ToDecimal(CurrentAmount))
                : Math.Abs(Convert.ToDecimal(CurrentAmount)) * -1;
            accountTransaction.ReconciliationDate = CurrentReconciledDate?.DateTime;
            accountTransaction.Description = CurrentDescription;

            dbContext.AccountTransactions.Update(accountTransaction);
            reval = dbContext.SaveChanges();
        }

        RebuildTransactions().Wait();
        NewRecord().Wait();

        return Task.FromResult(0);
    }

    /// <summary>
    ///     Deletes the record.
    /// </summary>
    /// <returns></returns>
    public Task DeleteRecord()
    {
        if (CurrentId != null && CurrentId != 0)
        {
            var accountTransaction = dbContext.AccountTransactions.First(x => x.Id == currentId);
            dbContext.AccountTransactions.Remove(accountTransaction);
            dbContext.SaveChanges();
        }

        RebuildTransactions().Wait();
        NewRecord().Wait();

        return Task.CompletedTask;
    }
}