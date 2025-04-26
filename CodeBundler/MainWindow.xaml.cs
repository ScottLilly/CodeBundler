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
    #region Fields

    private readonly FileCollector _fileCollector = new();
    private readonly FileConsolidator _fileConsolidator = new();

    #endregion

    #region Constructor

    public MainWindow()
    {
        InitializeComponent();

        _fileConsolidator.StatusUpdated += FileProcessingStartedHandler;
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

        UpdateStatusMessage("Finding code files in solution");

        var filesToConsolidate = await _fileCollector.GetFilesFromSolutionAsync(fileDialog.FileName);

        await ConsolidateFiles(filesToConsolidate);
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

        UpdateStatusMessage("Finding code files in project");

        var filesToConsolidate = await _fileCollector.GetFilesFromProjectAsync(fileDialog.FileName);

        await ConsolidateFiles(filesToConsolidate);
    }

    private async void SelectSourceFolder_Click(object sender, RoutedEventArgs e)
    {
        var folderDialog = new OpenFolderDialog
        {
            Title = "Select Source Folder(s)",
            Multiselect = true
        };

        if (folderDialog.ShowDialog() != true)
        {
            return;
        }

        UpdateStatusMessage("Finding code files in folder(s)");

        var filesToConsolidate = await _fileCollector.GetFilesFromFoldersAsync(folderDialog.FolderNames);

        await ConsolidateFiles(filesToConsolidate);
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

        UpdateStatusMessage("Finding code file(s)");

        var filesToConsolidate = await _fileCollector.GetFilesFromFilesAsync(fileDialog.FileNames);

        await ConsolidateFiles(filesToConsolidate);
    }

    private async Task ConsolidateFiles(IReadOnlyList<string> files)
    {
        UpdateStatusMessage($"Started consolidating {files.Count} files");

        OutputTextBox.Text = await _fileConsolidator.GetFilesAsStringAsync(files);

        UpdateStatusMessage($"Finished consolidating {files.Count} files");
    }

    private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(OutputTextBox.Text))
        {
            Clipboard.SetText(OutputTextBox.Text);
        }
    }

    private void FileProcessingStartedHandler(object? sender, string statusMessage)
    {
        UpdateStatusMessage(statusMessage);
    }

    private void UpdateStatusMessage(string message)
    {
        Dispatcher.Invoke(() => StatusMessageLabel.Content = message);
    }

    #endregion
}