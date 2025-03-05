# RepoGrep

RepoGrep is a lightweight, command-line tool built in C# using .NET 9. It searches for specified patterns in text files within Git repositories (or any directory), filtering by allowed file extensions. RepoGrep supports regular expressions, recursive search, case-insensitive matching, parallel processing, and verbose logging with timestamped output.

## Features

- **Regex-Based Searching:**  
  Search using powerful regular expressions with support for case-insensitivity.

- **File Extension Filtering:**  
  Only process files with specified extensions (e.g., `.cs`, `.md`).

- **Recursive Search:**  
  Optionally search directories and subdirectories.

- **Parallel Processing:**  
  Leverage multi-core CPUs for faster searching in large repositories.

- **Verbose Logging:**  
  Optionally log detailed, timestamped output to track the progress of file processing.

- **Summary & Timing:**  
  Display a summary including total files processed, match count, and elapsed time.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Installation

````bash
git clone https://github.com/yourusername/RepoGrep.git
cd RepoGrep
````

## Build

````bash
dotnet build
````

## Usage

RepoGrep requires at least two arguments: a search pattern and a directory to search. Additional options can be specified as flags.

### Command-Line Arguments
````
RepoGrep.exe <pattern> <directory> [options]
````

#### Options:
- `-r` — Perform a recursive search.
- `-i` — Use case-insensitive matching.
- `-n` — Show line numbers in the output.
- `-v` — Enable verbose logging (with timestamps).
- `-p` — Use parallel processing for file search.

*Allowed file extensions are defined in code (default: `.cs` and `.md`). Modify `SearchOptions.AllowedFileExtensions` in code as needed.*

### Examples

- **Search for "class" in the current directory (recursive, case-insensitive, with line numbers):**
  
````bash
RepoGrep.exe "class" . -r -i -n
````

- **Search with verbose logging and parallel processing:**
  
````bash
RepoGrep.exe "class" . -r -i -n -v -p
````

## How It Works

- **Pattern Matching:**  
  The tool compiles the provided regular expression pattern and searches line by line in text files.

- **File Filtering:**  
  Files are filtered based on allowed extensions. By default, only files ending in `.cs` or `.md` are processed.

- **Parallel & Verbose:**  
  When the `-p` flag is provided, RepoGrep processes files in parallel, logging progress with timestamps. At the end, a summary is displayed with the total number of files processed, the total matches found, and the elapsed time.

## Contributing

Contributions are welcome! Please fork this repository and submit pull requests for any improvements or bug fixes. If you have any questions or ideas, feel free to open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.