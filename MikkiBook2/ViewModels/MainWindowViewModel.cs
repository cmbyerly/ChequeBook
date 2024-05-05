using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MikkiBook2.Data;

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

        this.TransactionTypes = new List<TransactionTypes>();
        this.TransactionTypes.AddRange(Data.TransactionTypes.GetTransactionTypes());
        this.TransactionTypes = this.TransactionTypes.OrderBy(x => x.TransactionTypeCode).ToList();

        this.TransactionList = new List<AccountTransaction>();
        this.RebuildTransactions().Wait();
    }

    /// <summary>
    /// Greeting message
    /// </summary>
    public string Greeting { get; set; }
    
    /// <summary>The current identifier</summary>
    public long CurrentId { get; set; }
    
    /// <summary>The current balance</summary>
    public string CurrentBalance { get; set; }
    
    /// <summary>The transaction types</summary>
    public List<TransactionTypes> TransactionTypes { get; set; }
    
    /// <summary>The transaction list</summary>
    public List<AccountTransaction> TransactionList { get; set; }

    /// <summary>
    /// Rebuilds the transaction list.
    /// </summary>
    private Task RebuildTransactions()
    {
        decimal balance = 0.00M;

        using (var tempContext = new AccountContext())
        {
            tempContext.Database.EnsureCreated();
            foreach (var trans in tempContext.AccountTransactions.ToList())
            {
                balance = balance + trans.Amount;
                this.TransactionList.Add((AccountTransaction)trans.Clone());
            }

            CurrentBalance = $"${balance}";
        }
        
        return Task.CompletedTask;
    }
}