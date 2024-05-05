using System.Collections.Generic;
using System.Linq;

namespace MikkiBook2.Data;

    /// <summary>
    ///  TransactionTypes
    /// </summary>
    public class TransactionTypes
    {
        /// <summary>Initializes a new instance of the <see cref="TransactionTypes" /> class.</summary>
        /// <param name="transactionTypeCode">The transaction type code.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <param name="isDebit">if set to <c>true</c> [is debit].</param>
        /// <param name="isCredit">if set to <c>true</c> [is credit].</param>
        public TransactionTypes(string transactionTypeCode, string transactionType, bool isDebit, bool isCredit)
        {
            TransactionTypeCode = transactionTypeCode;
            TransactionType = transactionType;
            IsDebit = isDebit;
            IsCredit = isCredit;
        }

        /// <summary>Gets or sets the transaction type code.</summary>
        /// <value>The transaction type code.</value>
        public string TransactionTypeCode { get; set; }

        /// <summary>Gets or sets the type of the transaction.</summary>
        /// <value>The type of the transaction.</value>
        public string TransactionType { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is debit.</summary>
        /// <value>
        ///   <c>true</c> if this instance is debit; otherwise, <c>false</c>.</value>
        public bool IsDebit { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is credit.</summary>
        /// <value>
        ///   <c>true</c> if this instance is credit; otherwise, <c>false</c>.</value>
        public bool IsCredit { get; set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{TransactionTypeCode} - {TransactionType}";
        }

        /// <summary>Gets the transaction types.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TransactionTypes[] GetTransactionTypes()
        { 
            List<TransactionTypes> transactionTypes = new List<TransactionTypes>();

            transactionTypes.Add(new TransactionTypes("ATM", "ATM Withdrawl", true, false));
            transactionTypes.Add(new TransactionTypes("DC", "Debit Card", true, false));
            transactionTypes.Add(new TransactionTypes("AP", "Automatic Payment", true, false));
            transactionTypes.Add(new TransactionTypes("IP", "Internet Payment", true, false));
            transactionTypes.Add(new TransactionTypes("DD", "Direct Deposit", false, true));
            transactionTypes.Add(new TransactionTypes("D", "Deposit", false, true));
            transactionTypes.Add(new TransactionTypes("PH", "Phone", true, false));
            transactionTypes.Add(new TransactionTypes("OD", "Other Debit", true, false));
            transactionTypes.Add(new TransactionTypes("CK", "Check", true, false));
            transactionTypes.Add(new TransactionTypes("OC", "Other Credit", false, true));


            return transactionTypes.OrderBy(x => x.TransactionTypeCode).ToArray();
        }

    }