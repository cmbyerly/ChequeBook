using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MikkiBook2.Data;

/// <summary>
    ///   AccountTransaction
    /// </summary>
    public class AccountTransaction: ICloneable
    {
        /// <summary>Initializes a new instance of the <see cref="AccountTransaction" /> class.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="description">The description.</param>
        /// <param name="transType">Type of the trans.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="reconciliationDate">The reconciliation date.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="checkNumber">The check number.</param>
        public AccountTransaction(long id, string description, string transType, DateTime transactionDate, DateTime? reconciliationDate, decimal amount, string checkNumber)
        {
            Id = id;
            Description = description;
            TransType = transType;
            TransactionDate = transactionDate;
            ReconciliationDate = reconciliationDate;
            Amount = amount;
            CheckNumber = checkNumber;
        }

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [Required]
        [Column("DESC")]
        public string Description { get; set; }

        /// <summary>Gets or sets the type of the trans.</summary>
        /// <value>The type of the trans.</value>
        [Required]
        [Column("TYPE")]
        public string TransType { get; set; }

        /// <summary>Gets or sets the transaction date.</summary>
        /// <value>The transaction date.</value>
        [Required]
        [Column("TDATE")]
        public DateTime TransactionDate { get; set; }

        /// <summary>Gets or sets the reconciliation date.</summary>
        /// <value>The reconciliation date.</value>
        [Column("RDATE")]
        public DateTime? ReconciliationDate { get; set; }

        /// <summary>Gets or sets the amount.</summary>
        /// <value>The amount.</value>
        [Required]
        [Column("AMOUNT")]
        public decimal Amount { get; set; }

        /// <summary>Gets or sets the check number.</summary>
        /// <value>The check number.</value>
        [Column("CHECK_NUMBER")]
        public string CheckNumber { get; set; }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return new AccountTransaction(Id, Description, TransType, TransactionDate, ReconciliationDate, Amount, CheckNumber);
        }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var amtchar = Amount >= 0.00M ? "+" : string.Empty;
            return $"{TransType.PadRight(2, ' ')} - {Description.PadRight(100, ' ')} - {amtchar}{Amount.ToString("N2")}";
        }
    }