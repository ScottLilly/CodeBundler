# CodeBundler
<img align="left" width="100" height="100" style="color:white" src="https://github.com/ScottLilly/CodeBundler/blob/master/CodeBundler/Images/CodeBundler_Transparent.png">
WPF and command line tool to bundle C# or VB.NET code files into a single file or string.<br/><br/>
This was created to combine source code files into a single text file, or string to be copied into a clipboard, and pasted into an LLM, to ask questions about the source code and find problems or areas for improvement.<br/><br/>
If you select a Visual Studio C# or VB.NET solution or project file as the source, CodeBundler will read all non-generated .cs or .vb files within the solution or project.<br/><br/>

![Build Status](https://github.com/ScottLilly/CodeBundler/actions/workflows/ci.yml/badge.svg)
[![License](https://img.shields.io/github/license/ScottLilly/CodeBundler)](https://github.com/ScottLilly/CodeBundler/LICENSE)

## How to use
### Command Line Interface (CLI)
Run CodeBundler.Console.exe with the following parameters:
```
-o <output file> (this is a required parameter)

At least one of the following parameters is required:
-s <Solution [.sln] source file>
-p <Project [.csproj or .vbproj] source file>
-d <Directories> (reads all child directories too)
-f <Files>

Notes:
Source parameters are listed in order of precedence.
If you specify both a solution and a project, the solution will be used.
To use multiple directories or files, separate them with a semi-colon ";".

Examples:
CodeBundler.Console -s D:\CodeBundler\CodeBundler.sln -o D:\Output\combined.txt

CodeBundler.Console -d D:\CodeBundler\CodeBundler.Engine;D:\CodeBundler\CodeBundler.Console -o D:\Output\combined.txt
```
### WPF Application
Run CodeBundler.exe

This will open a WPF application where you can select the source solution, project, directories, or files.
The consolidated code will be displayed in a text box, and you can right-click to copy it to the clipboard, for pasting into an LLM.

## Requirements
- .NET 8.0

## Contributing
Contributions are welcome. Please submit issues or pull requests to the GitHub repository.

## License
This project is licensed under the MIT License. See the [LICENSE file](https://github.com/ScottLilly/CodeBundler/blob/master/LICENSE.txt) for details.

## Contact
For questions or feedback, please [open an issue here on GitHub](https://github.com/ScottLilly/CodeBundler/issues).
