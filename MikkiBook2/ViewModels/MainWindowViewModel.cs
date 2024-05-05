using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MikkiBook2.Data;
using ReactiveUI;

namespace MikkiBook2.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    /// <summary>The database context</summary>
    private AccountContext? dbContext;
    
    /// <summary>
    /// New window view model
    /// </summary>
    public MainWindowViewModel()
    {
        Greeting = "Welcome to Mikki Book 2!";
        
        CurrentBalance = $"$0.00";

        this.dbContext = new AccountContext();
        this.dbContext.Database.EnsureCreated();

        this.TransactionTypes = new List<TransactionTypes>(
            Data.TransactionTypes.GetTransactionTypes().OrderBy(x => x.TransactionTypeCode));

        this.TransactionList = new ObservableCollection<AccountTransaction>();
    }

    /// <summary>
    /// Greeting message
    /// </summary>
    public string Greeting { get; set; }

    /// <summary>The current identifier</summary>
    private long? currentId;
    public long? CurrentId
    {
        get => currentId;
        set => this.RaiseAndSetIfChanged(ref currentId, value);
    }
    
    /// <summary>The current balance</summary>
    private string? currentBalance;
    public string? CurrentBalance
    {
        get => currentBalance;
        set => this.RaiseAndSetIfChanged(ref currentBalance, value);
    }

    private string? currentDescription;
    public string? CurrentDescription
    {
        get => currentDescription;
        set => this.RaiseAndSetIfChanged(ref currentDescription, value);
    }
    
    private TransactionTypes? currentTransactionType;
    public TransactionTypes? CurrentTransactionType
    {
        get => currentTransactionType;
        set => this.RaiseAndSetIfChanged(ref currentTransactionType, value);
    }
    
    private DateTimeOffset? currentTransactionDate;
    public DateTimeOffset? CurrentTransactionDate
    {
        get => currentTransactionDate;
        set => this.RaiseAndSetIfChanged(ref currentTransactionDate, value);
    }
    
    private DateTimeOffset? currentReconciledDate;
    public DateTimeOffset? CurrentReconciledDate
    {
        get => currentReconciledDate;
        set => this.RaiseAndSetIfChanged(ref currentReconciledDate, value);
    }
    
    private string? currentAmount;
    public string? CurrentAmount
    {
        get => currentAmount;
        set => this.RaiseAndSetIfChanged(ref currentAmount, value);
    }
    
    private string? currentCheckNumber;
    public string? CurrentCheckNumber
    {
        get => currentCheckNumber;
        set => this.RaiseAndSetIfChanged(ref currentCheckNumber, value);
    }
    
    /// <summary>The transaction types</summary>
    public List<TransactionTypes> TransactionTypes { get; set; }
    
    /// <summary>The transaction list</summary>
    public ObservableCollection<AccountTransaction> TransactionList { get; set; }

    /// <summary>
    /// Rebuilds the transaction list.
    /// </summary>
    public Task RebuildTransactions()
    {
        decimal balance = 0.00M;
        this.TransactionList.Clear();

        using (var tempContext = new AccountContext())
        {
            tempContext.Database.EnsureCreated();
            foreach (var trans in tempContext.AccountTransactions.OrderByDescending(x => x.TransactionDate))
            {
                balance = balance + trans.Amount;
                this.TransactionList.Add((AccountTransaction)trans.Clone());
            }

            CurrentBalance = $"${balance}";
        }
        
        return Task.CompletedTask;
    }
    
    public Task NewRecord()
    {
        this.CurrentId = null;
        this.CurrentDescription = string.Empty;
        this.CurrentTransactionType = null;
        this.CurrentTransactionDate = DateTimeOffset.Now;
        this.CurrentReconciledDate = null;
        this.CurrentAmount = string.Empty;
        this.CurrentCheckNumber = string.Empty;
        
        return Task.CompletedTask;
    }

    public Task<int> SaveRecord()
    {
        return Task.FromResult(0);
    }
}