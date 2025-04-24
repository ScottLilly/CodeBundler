namespace CodeBundler.Engine.Models;

public class ConfigurationValues
{
    #region Properties

    public string SolutionFilePath { get; private set; } = string.Empty;
    public string ProjectFilePath { get; private set; } = string.Empty;
    public string FolderPath { get; private set; } = string.Empty;
    public List<string> FilePaths { get; } = [];

    #endregion

    #region Constructor

    private ConfigurationValues()
    {
    }

    #endregion

    #region Static Instantiators

    public static ConfigurationValues CreateFromCommandLineArgs(string[] commandLineArgs)
    {
        var configurationValues = new ConfigurationValues();

        // TODO: Parse command line arguments and populate configurationValues

        return configurationValues;
    }

    public static ConfigurationValues CreateFromSolutionFile(string solutionFilePath)
    {
        return new ConfigurationValues
        {
            SolutionFilePath = solutionFilePath
        };
    }

    public static ConfigurationValues CreateFromProjectFile(string projectFilePath)
    {
        return new ConfigurationValues
        {
            ProjectFilePath = projectFilePath
        };
    }

    public static ConfigurationValues CreateFromFolder(string folderPath)
    {
        return new ConfigurationValues
        {
            FolderPath = folderPath
        };
    }

    public static ConfigurationValues CreateFromFiles(params string[] filePaths)
    {
        var configurationValues = new ConfigurationValues();

        configurationValues.FilePaths.AddRange(filePaths);

        return configurationValues;
    }

    #endregion
}
