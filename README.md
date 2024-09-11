<h1 align="center">AutoUpdaterCs</h1>
<div align="center">

English | [简体中文](./README.cn.md) 

[![License](https://img.shields.io/badge/license-Apache%202-green.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![Test Status](https://github.com/www778878net/AutoUpdaterCs/actions/workflows/BuildandTest.yml/badge.svg?branch=main)](https://github.com/www778878net/AutoUpdaterCs/actions/workflows/BuildandTest.yml)
[![QQ Group](https://img.shields.io/badge/QQ%20Group-323397913-blue.svg?style=flat-square&color=12b7f5&logo=qq)](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)
</div>

## Feedback QQ Group (Click to join): [323397913](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)

## 1. `AutoUpdater` Class Documentation

### Overview

`AutoUpdaterCs` is a C#-based automatic update tool used to compare MD5 values of local and remote files and download updated files. It supports initializing local directories, generating MD5 file lists, updating files, and more.

### Installation

Clone the repository to your local machine:

~~~
git clone https://github.com/www778878net/AutoUpdaterCs.git
cd AutoUpdaterCs
~~~

### Quick Start

Here's a basic example of how to use `AutoUpdater`:

~~~csharp
using AutoUpdaterLib;

// Initialize local directory
string localDir = @"C:\path\to\local\directory";
string remoteUrl = "http://example.com/remote/";
var updater = new AutoUpdater(localDir, remoteUrl);

// Initialize local directory
string initResult = updater.InitializeLocalDirectory();
Console.WriteLine(initResult);

// Update files
string updateResult = await updater.UpdateFilesAsync();
Console.WriteLine(updateResult);
~~~

### Main Methods

- `InitializeLocalDirectory()`: Initialize local directory.
- `UpdateFilesAsync()`: Update local files.
- `GetFileListWithMd5(string directory)`: Get MD5 values for all files in the specified directory.
- `SaveMd5Json(string outputFile = "md5.json")`: Generate and save a JSON file containing file MD5 values.
- `CheckForUpdatesAsync()`: Check if updates are available.

### Example: Initializing Directory

~~~csharp
var updater = new AutoUpdater(@"C:\path\to\local\directory", "http://example.com/remote/");
string result = updater.InitializeLocalDirectory();
Console.WriteLine(result);
~~~

### Example: Updating Files

~~~csharp
var updater = new AutoUpdater(@"C:\path\to\local\directory", "http://example.com/remote/");
string result = await updater.UpdateFilesAsync();
Console.WriteLine(result);
~~~

### Running Tests

To run tests, execute the following command in the project root directory:

~~~
dotnet test
~~~

### Other

For more detailed information, please refer to the project's [GitHub repository](https://github.com/www778878net/AutoUpdaterCs) or API documentation.