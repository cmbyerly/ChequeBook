using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MikkiBook2.Data;
using MikkiBook2.ViewModels;

namespace MikkiBook2.Views;

/// <summary>
///     The main window
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    ///     Mainwindow constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    ///     Load data clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.RebuildTransactions().Wait();
    }

    /// <summary>
    ///     New clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void New_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.NewRecord().Wait();
    }

    /// <summary>
    ///     Save clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.SaveRecord().Wait();
    }

    /// <summary>
    ///     Selection of the DataGrid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender != null)
        {
            var dv = (DataGrid)sender;

            if (dv.SelectedItem != null)
            {
                var item = (AccountTransaction)dv.SelectedItem;
                var viewModel = (MainWindowViewModel)DataContext;
                viewModel.CurrentId = item.Id;
                viewModel.CurrentDescription = item.Description;
                viewModel.CurrentTransactionType =
                    viewModel.TransactionTypes.First(x => x.TransactionTypeCode == item.TransType);
                viewModel.CurrentTransactionDate = item.TransactionDate;
                viewModel.CurrentReconciledDate = item.ReconciliationDate;
                viewModel.CurrentAmount = Math.Abs(Convert.ToDecimal(item.Amount)).ToString("N2");
                viewModel.CurrentCheckNumber = item.CheckNumber;
                viewModel.IsCredit = item.Amount > 0;
            }
        }
    }

    /// <summary>
    ///     Deletes the selected record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Delete_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.DeleteRecord().Wait();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender != null)
        {
            var dv = (ComboBox)sender;

            if (dv.SelectedItem != null)
            {
                var item = (TransactionTypes)dv.SelectedItem;
                var viewModel = (MainWindowViewModel)DataContext;
                viewModel.IsCredit = item.IsCredit;
            }
        }
    }
}