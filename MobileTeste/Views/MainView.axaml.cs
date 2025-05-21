using Avalonia.Controls;
using MobileTeste.ViewModels;

namespace MobileTeste.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent(); // Esse método é gerado automaticamente pelo Avalonia
        DataContext = new MainViewModel();

    }
}
