using CodeBundler.Engine.Services;
using CodeBundler.Windows;
using Microsoft.Win32;
using System.Windows;

namespace CodeBundler;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Constructor

    public MainWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Eventhandlers

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ViewHelp_Click(object sender, RoutedEventArgs e)
    {
        var helpWindow = new HelpWindow();
        helpWindow.Owner = this;
        helpWindow.ShowDialog();
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow();
        aboutWindow.Owner = this;
        aboutWindow.ShowDialog();
    }

    private async void SelectSourceSolution_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Select Source Solution",
            Filter = "Solution Files (*.sln)|*.sln",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        var filesToConsolidate = await FileCollector.GetFilesFromSolutionAsync(fileDialog.FileName);

        OutputTextBox.Text = await FileConsolidator.GetFilesAsString(filesToConsolidate);
    }

    private async void SelectSourceProject_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Select Source Project",
            Filter = "Project Files (*.csproj, *.vbproj)|*.csproj;*.vbproj",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        var filesToConsolidate = await FileCollector.GetFilesFromProjectAsync(fileDialog.FileName);

        OutputTextBox.Text = await FileConsolidator.GetFilesAsString(filesToConsolidate);
    }

    private async void SelectSourceFolder_Click(object sender, RoutedEventArgs e)
    {
        var folderDialog = new OpenFolderDialog
        {
            Title = "Select Source Folder",
            Multiselect = true
        };

        if (folderDialog.ShowDialog() != true)
        {
            return;
        }

        var filesToConsolidate = await FileCollector.GetFilesFromFoldersAsync(folderDialog.FolderNames);

        OutputTextBox.Text = await FileConsolidator.GetFilesAsString(filesToConsolidate);
    }

    private async void SelectSourceFiles_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Select Source Code File(s)",
            Filter = "Source Code Files (*.cs, *.vb)|*.cs;*.vb|All Files (*.*)|*.*",
            Multiselect = true
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        var filesToConsolidate = await FileCollector.GetFilesFromFilesAsync(fileDialog.FileNames);

        OutputTextBox.Text = await FileConsolidator.GetFilesAsString(filesToConsolidate);
    }

    private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(OutputTextBox.Text))
        {
            Clipboard.SetText(OutputTextBox.Text);
        }
    }

    #endregion

}