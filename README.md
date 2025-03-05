# RepoGrep

RepoGrep is a lightweight, text-based search tool built in C# using .NET 9. It is designed to work both as a stand-alone command-line tool and as a reusable library that you can integrate into your own projects.

RepoGrep is organized as a multi-project solution:
- **RepoGrep.Core**: A class library containing the core search engine functionality (file filtering, regex matching, verbose logging, etc.). This library can be used as a dependency in your own applications.
- **RepoGrep.CLI**: A console application that provides a command-line interface to RepoGrep.Core, using [System.CommandLine](https://learn.microsoft.com/en-us/dotnet/command-line/) for robust parsing.

## Features

- **Regex-Based Searching:**  
  Use powerful regular expressions to search text files, with optional case-insensitive matching.

- **File Extension Filtering:**  
  Limit searches to files with specified extensions (e.g., `.cs`, `.md`). The allowed extensions can be configured via a command‑line option.

- **Recursive Search:**  
  Optionally search through all subdirectories.

- **Parallel Processing:**  
  Leverage multi-core CPUs to speed up searches in large codebases.

- **Verbose Logging:**  
  Get detailed, timestamped logging to track file processing and overall progress.

- **Reusable Core Library:**  
  Integrate the search functionality into your own projects by referencing the RepoGrep.Core library.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Installation and Build

1. **Clone the repository:**

````bash
git clone https://github.com/yourusername/RepoGrep.git
cd RepoGrep
````

2. **Build the solution:**

````bash
dotnet build
````

## Running the CLI Tool

To run the command-line tool, use the `RepoGrep.CLI` project. The CLI expects a search pattern and a directory as its first two arguments, followed by additional options.

### Command-Line Arguments
````
RepoGrep.exe <pattern> <directory> [options]
````


#### Options:
- `-r`, `--recursive` — Perform a recursive search.
- `-i`, `--ignore-case` — Use case-insensitive matching.
- `-n`, `--line-numbers` — Show line numbers in the output.
- `-v`, `--verbose` — Enable verbose logging (with timestamps).
- `-p`, `--parallel` — Use parallel processing for file search.
- `-e`, `--extensions` — Comma-separated list of allowed file extensions (default: `.cs,.md`).

### Examples

- **Search for "class" in the repository root recursively:**

````bash
RepoGrep.exe "class" ..\..\.. -r -i -n
````

- **Search for "class" with verbose logging, parallel processing, and additional allowed extensions:**

````bash
RepoGrep.exe "class" ..\..\.. -r -i -n -v -p -e ".cs,.md,.txt"
````

> **Note:**  
> When running from Visual Studio, the default working directory is typically the build output folder (e.g., `RepoGrep.CLI\bin\Debug\net9.0`). Adjust the directory argument (e.g., using `..\..\..`) or set the working directory in launchSettings.json to point to your repository root.

## Using RepoGrep.Core as a Dependency

If you wish to integrate RepoGrep's functionality into your own application, simply reference the `RepoGrep.Core` library. Below is an example of how to use it:

````csharp
using RepoGrep.Core;

var options = new SearchOptions
{
    Pattern = "class",
    Directory = @"C:\Path\To\Your\Project",
    Recursive = true,
    IgnoreCase = true,
    ShowLineNumbers = true,
    Verbose = true,
    AllowedExtensions = new List<string> { ".cs", ".md", ".txt" }
};

var engine = new RepoGrepEngine(options);
int matchCount = engine.Execute();
Console.WriteLine($"Total matches: {matchCount}");
````

## Project Structure

````
RepoGrep/ ├─ RepoGrep.sln ├─ LICENSE ├─ README.md ├─ RepoGrep.Core/ │ ├─ RepoGrep.Core.csproj │ ├─ SearchOptions.cs │ ├─ RepoGrepEngine.cs │ ├─ IOutputFormatter.cs │ ├─ DefaultOutputFormatter.cs │ ├─ FileExtension.cs │ └─ FileExtensions.cs └─ RepoGrep.CLI/ ├─ RepoGrep.CLI.csproj └─ Program.cs
````


## Contributing

Contributions are welcome! Please fork this repository and submit pull requests for any improvements or bug fixes. For major changes, please open an issue first to discuss your ideas.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.