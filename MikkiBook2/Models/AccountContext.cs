using Microsoft.EntityFrameworkCore;

namespace MikkiBook2.Data;

/// <summary>
///     AccountContext
/// </summary>
public class AccountContext : DbContext
{
    /// <summary>Gets or sets the account transactions.</summary>
    /// <value>The account transactions.</value>
    public DbSet<AccountTransaction> AccountTransactions { get; set; }

    /// <summary>
    ///     Override this method to configure the database (and other options) to be used for this context.
    ///     This method is called for each instance of the context that is created.
    ///     The base implementation does nothing.
    /// </summary>
    /// <param name="optionsBuilder">
    ///     A builder used to create or modify options for this context. Databases (and other extensions)
    ///     typically define extension methods on this object that allow you to configure the context.
    /// </param>
    /// <remarks>
    ///     <para>
    ///         In situations where an instance of
    ///         <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions">DbContextOptions</see> may or may not have been
    ///         passed
    ///         to the constructor, you can use
    ///         <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured">IsConfigured</see> to
    ///         determine if
    ///         the options have already been set, and skip some or all of the logic in
    ///         <see
    ///             cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)">
    ///             OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)
    ///         </see>
    ///         .
    ///     </para>
    ///     <para>
    ///         See <a href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</a>
    ///         for more information and examples.
    ///     </para>
    /// </remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=trans.db");
    }

    /// <summary>
    ///     Override this method to further configure the model that was discovered by convention from the entity types
    ///     exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1">DbSet</see> properties on your derived context. The
    ///     resulting model may be cached
    ///     and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">
    ///     The builder being used to construct the model for this context. Databases (and other extensions) typically
    ///     define extension methods on this object that allow you to configure aspects of the model that are specific
    ///     to a given database.
    /// </param>
    /// <remarks>
    ///     <para>
    ///         If a model is explicitly set on the options for this context (via
    ///         <see
    ///             cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)">
    ///             UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)
    ///         </see>
    ///         )
    ///         then this method will not be run. However, it will still run when creating a compiled model.
    ///     </para>
    ///     <para>
    ///         See <a href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</a> for more
    ///         information and
    ///         examples.
    ///     </para>
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountTransaction>()
            .ToTable("TRANS_TABLE")
            .HasKey(i => i.Id);
    }
}