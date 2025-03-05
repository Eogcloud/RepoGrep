using RepoGrep.Core;
using System.CommandLine;

namespace RepoGrep.CLI;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var rootCommand = new RootCommand("RepoGrep: A text-based search tool for code repositories.");

		var patternArg = new Argument<string>(
			name: "pattern",
			description: "The search pattern or regex."
		);
		var directoryArg = new Argument<string>(
			name: "directory",
			description: "The directory to search."
		);

		var recursiveOption = new Option<bool>(
			aliases: ["-r", "--recursive"],
			description: "Perform a recursive search."
		);

		var ignoreCaseOption = new Option<bool>(
			aliases: ["-i", "--ignore-case"],
			description: "Use case-insensitive matching."
		);

		var showLineNumbersOption = new Option<bool>(
			aliases: ["-n", "--line-numbers"],
			description: "Show line numbers in the output."
		);

		var verboseOption = new Option<bool>(
			aliases: ["-v", "--verbose"],
			description: "Enable verbose logging (with timestamps)."
		);

		var parallelOption = new Option<bool>(
			aliases: ["-p", "--parallel"],
			description: "Use parallel processing for file search."
		);

		var extensionsOption = new Option<string>(
			aliases: ["-e", "--extensions"],
			getDefaultValue: () => ".cs,.md",
			description: "Comma-separated list of allowed file extensions (e.g., .cs,.md)"
		);

		rootCommand.AddArgument(patternArg);
		rootCommand.AddArgument(directoryArg);
		rootCommand.AddOption(recursiveOption);
		rootCommand.AddOption(ignoreCaseOption);
		rootCommand.AddOption(showLineNumbersOption);
		rootCommand.AddOption(verboseOption);
		rootCommand.AddOption(parallelOption);
		rootCommand.AddOption(extensionsOption);

		rootCommand.SetHandler(
			(string pattern, string directory, bool recursive, bool ignoreCase, bool showLineNumbers, bool verbose, bool parallel, string extensions) =>
			{
				var allowedExtensions = extensions
					.Split(',', StringSplitOptions.RemoveEmptyEntries)
					.Select(ext => ext.Trim().StartsWith('.') ? ext.Trim().ToLower() : "." + ext.Trim().ToLower())
					.ToList();

				var options = new SearchOptions
				{
					Pattern = pattern,
					Directory = directory,
					Recursive = recursive,
					IgnoreCase = ignoreCase,
					ShowLineNumbers = showLineNumbers,
					Verbose = verbose,
					AllowedExtensions = allowedExtensions
				};

				var engine = new RepoGrepEngine(options);
				int matchCount = parallel ? engine.ExecuteParallel() : engine.Execute();
				Console.WriteLine($"Total matches: {matchCount}");
			},
			patternArg,
			directoryArg,
			recursiveOption,
			ignoreCaseOption,
			showLineNumbersOption,
			verboseOption,
			parallelOption,
			extensionsOption
		);

		await rootCommand.InvokeAsync(args);
	}
}