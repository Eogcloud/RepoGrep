namespace RepoGrep.Core
{
	public class DefaultOutputFormatter(SearchOptions options) : IOutputFormatter
	{
		public void PrintMatch(string filePath, int lineNumber, string line)
		{
			if (options.ShowLineNumbers)
			{
				Console.WriteLine($"{filePath}:{lineNumber}: {line}");
			}
			else
			{
				Console.WriteLine($"{filePath}: {line}");
			}
		}
	}
}
