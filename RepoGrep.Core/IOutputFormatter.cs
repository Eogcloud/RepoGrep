namespace RepoGrep.Core;

public interface IOutputFormatter
{
	void PrintMatch(string filePath, int lineNumber, string line);
}
