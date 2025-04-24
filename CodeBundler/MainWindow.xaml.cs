using CodeBundler.Engine.Models;
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

        var fileContents = await FileAggregator.GetContentsForSolutionAsync(fileDialog.FileName);
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

        var fileContents = await FileAggregator.GetContentsForProjectAsync(fileDialog.FileName);
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

        var fileContents = await FileAggregator.GetContentsForFoldersAsync(folderDialog.FolderNames);
    }

    private void SelectSourceFile_Click(object sender, RoutedEventArgs e)
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

        var configurationValues = ConfigurationValues.CreateFromFiles(fileDialog.FileNames);
    }

    #endregion

}