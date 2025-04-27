using CSharpExtender.ExtensionMethods;
using System.Reflection;
using System.Windows.Input;

namespace CodeBundler.ViewModels;

public class AboutViewModel
{
    private static readonly Version s_version =
        Assembly.GetExecutingAssembly().GetName().Version;

    public string VersionText =>
        $"{s_version.Major}.{s_version.Minor}.{s_version.Revision}";
    public string Copyright =>
        $"© {(DateTime.Now.Year == 2025 ? "2025" : $"2025 - {DateTime.Now.Year}")}, Scott Lilly";
    public string License =>
        "Licensed under the MIT License";
    public string ContactInformation =>
        "https://scottlilly.com/contact-scott/";
    public string ProjectWebsite =>
        "https://scottlilly.com/codebundler/";
    public string SourceCode =>
        "https://github.com/ScottLilly/CodeBundler";
    public string Disclaimer =>
        "This software is provided as-is, without any warranty, express or implied. " + 
        "While every effort has been made to ensure the code functions correctly, it is used at your own risk.";
    public string Credits =>
        "This software uses the following third-party components:\n" +
        "• ScottLilly.ArgumentParser\n" +
        "• ScottLilly.CSharpExtender";

    public ICommand OpenUrlCommand { get; }

    public AboutViewModel()
    {
        OpenUrlCommand = new TypedRelayCommand<string>(OpenUrl);
    }

    private void OpenUrl(string url)
    {
        if (url.HasText())
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
        }
    }
}
