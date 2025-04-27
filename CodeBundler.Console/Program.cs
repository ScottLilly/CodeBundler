using ArgumentParser;
using CodeBundler.Engine.Services;

Console.WriteLine("Starting CodeBundler...");

try
{
    // Parse command line arguments
    var argumentParser = new Parser(["-"], [' ']);
    var parsedArguments = argumentParser.Parse(Environment.GetCommandLineArgs());

    // Ensure we have the required arguments
    if (!parsedArguments.NamedArguments.ContainsKey("o"))
    {
        throw new ArgumentException("Output file argument \"-o\" is required.");
    }

    if (string.IsNullOrWhiteSpace(parsedArguments.NamedArguments["o"]))
    {
        throw new ArgumentException("Output file argument \"-o\" cannot be empty.");
    }

    try
    {
        string outoutFilePath = Path.GetFullPath(parsedArguments.NamedArguments["o"]);
    }
    catch (Exception ex)
    {
        throw new ArgumentException("Invalid output file path provided.", ex);
    }

    if (!parsedArguments.NamedArguments.ContainsKey("s") &&
        !parsedArguments.NamedArguments.ContainsKey("p") &&
        !parsedArguments.NamedArguments.ContainsKey("d") &&
        !parsedArguments.NamedArguments.ContainsKey("f"))
    {
        throw new ArgumentException("At least one source file argument is required: -s (solution), -p (project), -d (directories), or -f (files).");
    }

    // Collect files based on the provided arguments
    Console.WriteLine("Collecting files...");
    IReadOnlyList<string> files = [];
    var fileCollector = new FileCollector();

    if (parsedArguments.NamedArguments.ContainsKey("s"))
    {
        files = await fileCollector.GetFilesFromSolutionAsync(parsedArguments.NamedArguments["s"]);
    }
    else if (parsedArguments.NamedArguments.ContainsKey("p"))
    {
        files = await fileCollector.GetFilesFromProjectAsync(parsedArguments.NamedArguments["p"]);
    }
    else if (parsedArguments.NamedArguments.ContainsKey("d"))
    {
        files = await fileCollector.GetFilesFromFoldersAsync(parsedArguments.NamedArguments["d"].Split(';'));
    }
    else if (parsedArguments.NamedArguments.ContainsKey("f"))
    {
        files = await fileCollector.GetFilesFromFilesAsync(parsedArguments.NamedArguments["f"].Split(';'));
    }

    if (files.Count == 0)
    {
        Console.WriteLine("No files found to consolidate.");
        return;
    }
    else
    {
        Console.WriteLine($"Found {files.Count} files for consolidation.");
    }

    // Consolidate files into a single output file
    Console.WriteLine("Consolidating files...");
    var fileConsolidator = new FileConsolidator();
    fileConsolidator.StatusUpdated += (sender, message) => Console.WriteLine(message);

    await fileConsolidator.ConsolidateToSingleFileAsync(files, parsedArguments.NamedArguments["o"]);

    Console.WriteLine($"Files consolidated successfully into: {parsedArguments.NamedArguments["o"]}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return;
}
