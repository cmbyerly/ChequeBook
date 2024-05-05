using Avalonia.Controls;
using Avalonia.Interactivity;
using MikkiBook2.ViewModels;

namespace MikkiBook2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.RebuildTransactions().Wait();
    }

    private void New_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.NewRecord().Wait();
    }
    
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.SaveRecord().Wait();
    }
}