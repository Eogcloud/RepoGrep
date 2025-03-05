namespace RepoGrep.Core;

public class SearchOptions
{
	public string Pattern { get; set; } = string.Empty;
	public string Directory { get; set; } = string.Empty;
	public bool IgnoreCase { get; set; }
	public bool Recursive { get; set; }
	public bool ShowLineNumbers { get; set; }
	public bool Verbose { get; set; }

	public List<string>? AllowedExtensions { get; set; }

	public List<string>? ExcludedExtensions { get; set; }
}

