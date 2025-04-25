using System.Text;

namespace CodeBundler.Engine.Services;

public class FileConsolidator
{
    private static string SEPARATOR = new string('=', 30);

    public static async Task<string> GetFilesAsString(IReadOnlyList<string> files)
    {
        if (files == null || files.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        foreach (var file in files)
        {
            sb.AppendLine(Path.GetFileName(file));
            sb.AppendLine(SEPARATOR);
            sb.Append(await File.ReadAllTextAsync(file));
            sb.AppendLine("");
        }

        return sb.ToString();
    }

    public static async void ConsolidateToSingleFile(
        IReadOnlyList<string> files, string outputFilePath)
    {
        if (files == null || files.Count == 0)
        {
            throw new ArgumentException("No files to consolidate.", nameof(files));
        
        }
        if (string.IsNullOrEmpty(outputFilePath))
        {
            throw new ArgumentException("Output file path cannot be null or empty.", nameof(outputFilePath));
        }

        var consolidatedContent = await GetFilesAsString(files);

        await File.WriteAllTextAsync(outputFilePath, consolidatedContent);
    }
}
