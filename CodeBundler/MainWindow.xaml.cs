﻿using CodeBundler.Engine.Services;
using CodeBundler.Windows;
using Microsoft.Win32;
using System.IO;
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
    
    private HelpWindow? _helpWindow;

    #endregion

    #region Constructor

    public MainWindow()
    {
        InitializeComponent();

        _fileConsolidator.StatusUpdated += FileProcessingStartedHandler;
    }

    #endregion

    #region Main Menu Eventhandlers

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ViewHelp_Click(object sender, RoutedEventArgs e)
    {
        if (_helpWindow == null)
        {
            _helpWindow = new HelpWindow();
            _helpWindow.Owner = this;
            _helpWindow.Closed += (s, args) => _helpWindow = null; // Clear reference when closed
            _helpWindow.Show();
        }
        else
        {
            _helpWindow.WindowState = WindowState.Normal;
            _helpWindow.Activate();
        }
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

        try
        {
            var filesToConsolidate =
                await _fileCollector.GetFilesFromSolutionAsync(fileDialog.FileName);

            await ConsolidateFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing solution: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
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

        try
        {
            var filesToConsolidate =
                await _fileCollector.GetFilesFromProjectAsync(fileDialog.FileName);

            await ConsolidateFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
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

        try
        {
            var filesToConsolidate =
                await _fileCollector.GetFilesFromFoldersAsync(folderDialog.FolderNames);

            await ConsolidateFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing folder(s): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
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

        try
        {
            var filesToConsolidate = 
                await _fileCollector.GetFilesFromFilesAsync(fileDialog.FileNames);

            await ConsolidateFiles(filesToConsolidate);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error processing file(s): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    #endregion

    #region Context menu option handlers

    private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(OutputTextBox.Text))
        {
            Clipboard.SetText(OutputTextBox.Text);
        }
    }

    private void SaveToFile_Click(object sender, RoutedEventArgs e)
    {
        var saveDialog = new SaveFileDialog
        {
            Title = "Save Consolidated Code",
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            FileName = "ConsolidatedCode.txt"
        };

        if (saveDialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            File.WriteAllText(saveDialog.FileName, OutputTextBox.Text);

            MessageBox.Show("File saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    #region General methods

    private async Task ConsolidateFiles(IReadOnlyList<string> files)
    {
        UpdateStatusMessage($"Started consolidating {files.Count} files");

        OutputTextBox.Text = await _fileConsolidator.GetFilesAsStringAsync(files);

        UpdateStatusMessage($"Finished consolidating {files.Count} files");
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