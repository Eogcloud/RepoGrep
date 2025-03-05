using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RepoGrep.Core;

public class RepoGrepEngine(SearchOptions options, IOutputFormatter? outputFormatter = null)
{
	private readonly IOutputFormatter outputFormatter = outputFormatter ?? new DefaultOutputFormatter(options);
	private Regex? _regex;
	private int totalMatches = 0;
	private int filesProcessed = 0;

	public int Execute()
	{
		var stopwatch = Stopwatch.StartNew();
		if (!TryBuildRegex(out _regex))
			return 0;

		var files = GetFiles(options.Directory, options.Recursive).ToList();
		LogVerbose($"Found {files.Count} files to process.");

		foreach (var file in files)
		{
			LogVerbose($"Processing file: {file}");
			totalMatches += ProcessFile(file);
			filesProcessed++;
			LogVerbose($"Finished file: {file}");
		}

		stopwatch.Stop();
		PrintConclusion(stopwatch.Elapsed);
		return totalMatches;
	}

	public int ExecuteParallel()
	{
		var stopwatch = Stopwatch.StartNew();
		if (!TryBuildRegex(out _regex))
			return 0;

		var files = GetFiles(options.Directory, options.Recursive).ToList();
		LogVerbose($"Found {files.Count} files to process.");

		object consoleLock = new();
		var matchCounter = new ConcurrentBag<int>();

		Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, filePath =>
		{
			LogVerbose($"[Thread {Task.CurrentId}] Processing file: {filePath}");
			int fileMatchCount = ProcessFile(filePath);
			matchCounter.Add(fileMatchCount);
			Interlocked.Increment(ref filesProcessed);
			LogVerbose($"[Thread {Task.CurrentId}] Finished file: {filePath}");
		});

		totalMatches = matchCounter.Sum();
		stopwatch.Stop();
		PrintConclusion(stopwatch.Elapsed);
		return totalMatches;
	}

	private bool TryBuildRegex(out Regex? regex)
	{
		regex = null;
		var regexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;
		if (options.IgnoreCase)
			regexOptions |= RegexOptions.IgnoreCase;

		try
		{
			regex = new Regex(options.Pattern, regexOptions);
			LogVerbose($"Compiled regex for pattern: {options.Pattern}");
			return true;
		}
		catch (ArgumentException ex)
		{
			Console.Error.WriteLine($"{GetTimestamp()} Invalid pattern '{options.Pattern}': {ex.Message}");
			return false;
		}
	}

	private IEnumerable<string> GetFiles(string directory, bool recursive)
	{
		var enumerationOptions = new EnumerationOptions
		{
			RecurseSubdirectories = recursive,
			IgnoreInaccessible = true
		};

		var allFiles = Directory.EnumerateFiles(directory, "*.*", enumerationOptions)
			.Where(f => !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}"));

		LogVerbose("All discovered files:");
		foreach (var file in allFiles)
		{
			LogVerbose(file);
		}

		if (options.AllowedExtensions != null && options.AllowedExtensions.Any())
		{
			allFiles = FileExtensions.FilterFilesByExtension(allFiles, options.AllowedExtensions, options.ExcludedExtensions);
		}
		else
		{
			LogVerbose("No file extension filtering set; processing all files.");
		}

		var filteredFiles = allFiles.ToList();
		LogVerbose("Filtered files:");
		foreach (var file in filteredFiles)
		{
			LogVerbose(file);
		}

		return filteredFiles;
	}


	private int ProcessFile(string filePath)
	{
		int lineNumber = 0;
		int fileMatches = 0;
		try
		{
			foreach (var line in File.ReadLines(filePath))
			{
				lineNumber++;
				if (_regex != null && _regex.IsMatch(line))
				{
					outputFormatter.PrintMatch(filePath, lineNumber, line);
					fileMatches++;
				}
			}
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine($"{GetTimestamp()} Error reading {filePath}: {ex.Message}");
		}
		return fileMatches;
	}

	private void PrintConclusion(TimeSpan elapsed)
	{
		Console.WriteLine($"{GetTimestamp()} Search completed in {elapsed.TotalSeconds:F2} seconds.");
		Console.WriteLine($"{GetTimestamp()} Processed {filesProcessed} file(s) with {totalMatches} match(es).");
		if (totalMatches == 0)
			Console.WriteLine($"{GetTimestamp()} No results found.");
	}

	private void LogVerbose(string message)
	{
		if (options.Verbose)
			Console.WriteLine($"{GetTimestamp()} [Verbose] {message}");
	}

	private static string GetTimestamp() => $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]";
}
