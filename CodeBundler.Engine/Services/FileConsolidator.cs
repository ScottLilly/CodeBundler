using System.Text;

namespace CodeBundler.Engine.Services;

public class FileConsolidator
{
    private static readonly string SEPARATOR = new('=', 30);

    public event EventHandler<string> FileProcessingStarted;

    public async Task<string> GetFilesAsStringAsync(IReadOnlyList<string> files)
    {
        if (files == null || files.Count == 0)
        {
            return string.Empty;
        }

        // Calculate total size for StringBuilder capacity
        long estimatedSize = 0;
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            estimatedSize += fileInfo.Length;
            estimatedSize += Path.GetFileName(file).Length + Environment.NewLine.Length;
            estimatedSize += SEPARATOR.Length + Environment.NewLine.Length;
            estimatedSize += Environment.NewLine.Length;
        }

        // Set StringBuilder's capacity, cap at int.MaxValue
        var sb = new StringBuilder(Math.Min((int)estimatedSize, int.MaxValue));
        foreach (var file in files)
        {
            OnFileProcessingStarted(file);

            sb.AppendLine(Path.GetFileName(file));
            sb.AppendLine(SEPARATOR);
            sb.Append(await File.ReadAllTextAsync(file));
            sb.AppendLine("");
        }

        return sb.ToString();
    }

    public async Task ConsolidateToSingleFileAsync(IReadOnlyList<string> files, string outputFilePath)
    {
        if (files == null || files.Count == 0)
        {
            throw new ArgumentException("No files to consolidate.", nameof(files));
        }

        if (string.IsNullOrEmpty(outputFilePath))
        {
            throw new ArgumentException("Output file path cannot be null or empty.", nameof(outputFilePath));
        }

        var consolidatedContent = await GetFilesAsStringAsync(files);

        await File.WriteAllTextAsync(outputFilePath, consolidatedContent);
    }

    private void OnFileProcessingStarted(string fileName)
    {
        FileProcessingStarted?.Invoke(this, fileName);
    }
}
