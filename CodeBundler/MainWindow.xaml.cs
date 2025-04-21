using CodeBundler.Windows;
using System.Reflection.Metadata;
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
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Solution",
            Filter = "Solution Files (*.sln)|*.sln",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        //var solution = await FileParser.ParseSolutionFileAsync(fileDialog.FileName);

        //var analysisJob = new AnalysisJob(solution);

        //List<Document> uniqueClassFileNames = await analysisJob.GetClassFiles();
    }

    private async void SelectSourceProject_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Project",
            Filter = "Project Files (*.csproj)|*.csproj",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        //var project = await FileParser.ParseProjectFileAsync(fileDialog.FileName);
    }

    private void SelectSourceFolder_Click(object sender, RoutedEventArgs e)
    {
        var folderDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source Folder",
            Filter = "Folders|*.*",
            Multiselect = false,
            CheckFileExists = false,
            CheckPathExists = true
        };

        if (folderDialog.ShowDialog() != true)
        {
            return;
        }
    }

    private void SelectSourceFile_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select Source File",
            Filter = "C# Files (*.cs)|*.cs",
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }
    }

    #endregion

}