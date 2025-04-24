using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace CodeBundler.Engine.Services;

public class FileAggregator
{
    #region Private Fields

    private static readonly List<string> s_fileExtensionsToInclude = [ ".cs", ".vb"];

    private static readonly List<string> s_excludedFilePatterns = [
        "AssemblyAttributes.cs",
        "AssemblyInfo.cs",
        ".g.cs", // Generated files
        ".g.i.cs", // Generated files
        ".Designer.cs", // Designer-generated files
        ".generated.cs", // Other generated patterns
        "AssemblyAttributes.vb",
        "AssemblyInfo.vb",
        ".g.vb", // Generated files
        ".g.i.vb", // Generated files
        ".Designer.vb", // Designer-generated files
        ".generated.vb" // Other generated patterns
        ];

    #endregion

    #region Public static methods

    public static async Task<List<string>> GetContentsForSolutionAsync(string solutionFileName)
    {
        if (string.IsNullOrEmpty(solutionFileName))
        {
            throw new ArgumentException("Solution file name cannot be null or empty.", nameof(solutionFileName));
        }

        if (!File.Exists(solutionFileName))
        {
            throw new FileNotFoundException("Solution file not found.", solutionFileName);
        }

        // Read the solution file using MSBuildWorkspace
        using var workspace = MSBuildWorkspace.Create();

        var solution = await workspace.OpenSolutionAsync(solutionFileName) 
            ?? throw new InvalidOperationException("Failed to open solution.");

        // Instantiate output collection
        List<string> outputDocuments = [];

        // Loop through each project in the solution
        foreach (var project in solution.Projects)
        {
            // Get all documents in the project
            var documents = project.Documents;

            foreach (var document in documents)
            {
                // Check if the file matches any of the excluded patterns
                if (s_excludedFilePatterns.Any(pattern => document.Name.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)))
                {
                    continue; // Skip excluded files
                }

                // Read the content of the document
                var text = await document.GetTextAsync();

                outputDocuments.Add(text.ToString());
            }
        }

        return outputDocuments;
    }

    public static async Task<List<string>> GetContentsForProjectAsync(string projectFileName)
    {
        if (string.IsNullOrEmpty(projectFileName))
        {
            throw new ArgumentException("Project file name cannot be null or empty.", nameof(projectFileName));
        }

        if (!File.Exists(projectFileName))
        {
            throw new FileNotFoundException("Project file not found.", projectFileName);
        }

        // Read the solution file using MSBuildWorkspace
        using var workspace = MSBuildWorkspace.Create();

        var project = await workspace.OpenProjectAsync(projectFileName) 
            ?? throw new InvalidOperationException("Failed to open project.");

        // Instantiate output collection
        List<string> outputDocuments = [];

        // Get all documents in the project
        var documents = project.Documents;

        foreach (var document in documents)
        {
            // Check if the file matches any of the excluded patterns
            if (s_excludedFilePatterns.Any(pattern => document.Name.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)))
            {
                continue; // Skip excluded files
            }

            // Read the content of the document
            var text = await document.GetTextAsync();

            outputDocuments.Add(text.ToString());
        }

        return outputDocuments;
    }

    public static async Task<List<string>> GetContentsForFoldersAsync(string[] folderPaths)
    {
        if (folderPaths.Length == 0)
        {
            throw new ArgumentException("Folder paths cannot be null or empty.", nameof(folderPaths));
        }

        if (folderPaths.Any(fp => !Directory.Exists(fp)))
        {
            throw new DirectoryNotFoundException("One or more folder paths do not exist: " + string.Join(", ", folderPaths));
        }

        // Instantiate output collection
        List<string> outputDocuments = [];

        // Loop through each folder path passed in
        foreach (var folderPath in folderPaths)
        {
            foreach (var extension in s_fileExtensionsToInclude)
            {
                // Get all files in the folder with the specified extension
                var files = Directory.GetFiles(folderPath, $"*{extension}", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    // Check if the file matches any of the excluded patterns
                    if (s_excludedFilePatterns.Any(pattern => file.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue; // Skip excluded files
                    }

                    // Read the content of the document
                    var documentText = await File.ReadAllTextAsync(file);

                    outputDocuments.Add(documentText);
                }
            }
        }

        return outputDocuments;
    }

    #endregion
}
