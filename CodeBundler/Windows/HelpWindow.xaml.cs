using System.Windows;

namespace CodeBundler.Windows;

/// <summary>
/// Interaction logic for HelpWindow.xaml
/// </summary>
public partial class HelpWindow : Window
{
    public HelpWindow()
    {
        InitializeComponent();

        HelpText.Text =
            "CodeBundler combines C# and VB.NET source code files into a single string or file.\n\n" +
            "Select a solution, project, directory (or directories), or file(s) to consolidate. " +
            "If you select directories, the source code files from all child subdirectories will also be included.\n\n" +
            "CodeBundler reads all *.cs and *.vb files, except these generated files:\n" +
            "\u2022 AssemblyAttributes.*\n" +
            "\u2022 AssemblyInfo.*\n" +
            "\u2022 *.g.*\n" +
            "\u2022 *.g.i.*\n" +
            "\u2022 *.Designer.*\n" +
            "\u2022 *.generated.*\n" +
            "\n" +
            "The consolidated text will be displayed in the textbox on the screen.\n\n" +
            "Right-click on the text for a menu to copy it to your clipboard or save to a file.";
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
