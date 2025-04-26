using CodeBundler.ViewModels;
using System.Windows;

namespace CodeBundler.Windows;

/// <summary>
/// Interaction logic for AboutWindow.xaml
/// </summary>
public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        DataContext = new AboutViewModel();
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
