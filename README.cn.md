<h1 align="center">AutoUpdaterCs</h1>
<div align="center">

[English](./README.md) | 简体中文

[![License](https://img.shields.io/badge/license-Apache%202-green.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![测试状态](https://github.com/www778878net/AutoUpdaterCs/actions/workflows/BuildandTest.yml/badge.svg?branch=main)](https://github.com/www778878net/AutoUpdaterCs/actions/workflows/BuildandTest.yml)
[![QQ群](https://img.shields.io/badge/QQ群-323397913-blue.svg?style=flat-square&color=12b7f5&logo=qq)](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)
</div>

## 反馈QQ群（点击加入）：[323397913](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)

## 1. `AutoUpdater` 类文档 

### 概述

`AutoUpdaterCs` 是一个用C#编写的自动更新工具，用于比较本地和远程文件的MD5值，并下载更新的文件。它支持初始化本地目录、生成MD5文件列表、更新文件等功能。

### 安装

克隆仓库到本地：

~~~
git clone https://github.com/www778878net/AutoUpdaterCs.git
cd AutoUpdaterCs
~~~

### 快速开始

以下是如何使用 `AutoUpdater` 的基本示例：

~~~csharp
using AutoUpdaterLib;

// 初始化本地目录
string localDir = @"C:\path\to\local\directory";
string remoteUrl = "http://example.com/remote/";
var updater = new AutoUpdater(localDir, remoteUrl);

// 初始化本地目录
string initResult = updater.InitializeLocalDirectory();
Console.WriteLine(initResult);

// 更新文件
string updateResult = await updater.UpdateFilesAsync();
Console.WriteLine(updateResult);
~~~

### 主要方法

- `InitializeLocalDirectory()`: 初始化本地目录。
- `UpdateFilesAsync()`: 更新本地文件。
- `GetFileListWithMd5(string directory)`: 获取指定目录下所有文件的MD5值。
- `SaveMd5Json(string outputFile = "md5.json")`: 生成并保存包含文件MD5值的JSON文件。
- `CheckForUpdatesAsync()`: 检查是否有可用更新。

### 示例：初始化目录

~~~csharp
var updater = new AutoUpdater(@"C:\path\to\local\directory", "http://example.com/remote/");
string result = updater.InitializeLocalDirectory();
Console.WriteLine(result);
~~~

### 示例：更新文件

~~~csharp
var updater = new AutoUpdater(@"C:\path\to\local\directory", "http://example.com/remote/");
string result = await updater.UpdateFilesAsync();
Console.WriteLine(result);
~~~

### 运行测试

要运行测试，请在项目根目录下执行以下命令：

~~~
dotnet test
~~~

### 其他

更多详细信息，请参阅项目的 [GitHub 仓库](https://github.com/www778878net/AutoUpdaterCs) 或 API 文档。
