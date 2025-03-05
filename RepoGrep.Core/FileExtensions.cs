namespace RepoGrep.Core;

public static class FileExtensions
{
	public static IEnumerable<string> ToExtensionStrings(this IEnumerable<FileExtension> fileExtensions)
		=> fileExtensions.Select(ext => "." + ext.ToString().ToLower());

	public static IEnumerable<string> FilterFilesByExtension(
		IEnumerable<string> files,
		IEnumerable<string>? allowedExtensions,
		IEnumerable<string>? excludedExtensions)
	{
		if (allowedExtensions != null && allowedExtensions.Any())
		{
			files = files.Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()));
		}

		if (excludedExtensions != null && excludedExtensions.Any())
		{
			files = files.Where(f => !excludedExtensions.Contains(Path.GetExtension(f).ToLower()));
		}

		return files;
	}
}