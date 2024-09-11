using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoUpdaterNamespace
{
    public class AutoUpdater
    {
        private readonly string _localDirectory;
        private readonly string _remoteBaseUrl;
        private readonly string _remoteMd5Url;
        private HttpClient _httpClient;
        private readonly string _updateUrl;

        public AutoUpdater(string localDirectory, string remoteBaseUrl, string updateUrl)
        {
            _localDirectory = localDirectory;
            _remoteBaseUrl = remoteBaseUrl;
            _remoteMd5Url = new Uri(new Uri(remoteBaseUrl), "md5.json").ToString();
            _httpClient = new HttpClient();
            _updateUrl = updateUrl;
            Console.WriteLine($"AutoUpdater initialized with update URL: {_updateUrl}");
        }

        public void SetHttpClient(HttpClient client)
        {
            _httpClient = client;
        }

        private string CalculateFileMd5(string filepath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filepath);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private Dictionary<string, string> GetFileListWithMd5(string directory)
        {
            var fileList = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file) != "md5.json")
                {
                    var relativePath = Path.GetRelativePath(directory, file);
                    fileList[relativePath] = CalculateFileMd5(file);
                }
            }
            return fileList;
        }

        public string SaveMd5Json(string outputFile = "md5.json")
        {
            var md5List = GetFileListWithMd5(_localDirectory);
            var outputPath = Path.Combine(_localDirectory, outputFile);
            var jsonString = JsonSerializer.Serialize(md5List, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(outputPath, jsonString);
            return outputPath;
        }

        private async Task DownloadFileAsync(string url, string filepath)
        {
            var response = await _httpClient.GetByteArrayAsync(url);
            var directoryName = Path.GetDirectoryName(filepath);
            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            await File.WriteAllBytesAsync(filepath, response);
        }

        public async Task<string> UpdateFilesAsync()
        {
            try
            {
                Dictionary<string, string>? remoteMd5List = null;
                try
                {
                    var response = await _httpClient.GetStringAsync(_remoteMd5Url);
                    remoteMd5List = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                }
                catch (HttpRequestException)
                {
                    // 如果md5.json不存在，我们就只更新movies.json文件
                    remoteMd5List = new Dictionary<string, string> { { "movies.json", "remote_md5" } };
                }

                if (remoteMd5List == null)
                {
                    throw new Exception("Failed to get remote MD5 list");
                }

                var localMd5List = GetFileListWithMd5(_localDirectory);

                var updatedFiles = new List<string>();
                foreach (var (filepath, remoteMd5) in remoteMd5List)
                {
                    var localFilepath = Path.Combine(_localDirectory, filepath);
                    if (!localMd5List.ContainsKey(filepath) || localMd5List[filepath] != remoteMd5)
                    {
                        Console.WriteLine($"Updating file: {filepath}");
                        var remoteFileUrl = new Uri(new Uri(_remoteBaseUrl), filepath).ToString();
                        await DownloadFileAsync(remoteFileUrl, localFilepath);
                        updatedFiles.Add(filepath);
                    }
                }

                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    updated_files = updatedFiles
                });
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = e.Message
                });
            }
        }

        public string InitializeLocalDirectory()
        {
            try
            {
                Directory.CreateDirectory(_localDirectory);
                var localMd5File = SaveMd5Json();
                Console.WriteLine($"Generated md5.json file: {localMd5File}");

                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    message = $"Local directory '{_localDirectory}' has been initialized",
                    md5_file = localMd5File,
                    md5_content = GetFileListWithMd5(_localDirectory)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error initializing local directory: {e.Message}");
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = e.Message
                });
            }
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(_remoteMd5Url);
                var remoteMd5List = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                if (remoteMd5List == null)
                {
                    return false; // 无法获取远程MD5列表，假设没有更新
                }

                var localMd5List = GetFileListWithMd5(_localDirectory);

                foreach (var (filepath, remoteMd5) in remoteMd5List)
                {
                    if (filepath != "md5.json")
                    {
                        if (!localMd5List.ContainsKey(filepath) || localMd5List[filepath] != remoteMd5)
                        {
                            return true; // 有更新可用
                        }
                    }
                }

                return false; // 没有更新
            }
            catch (Exception)
            {
                return false; // 检查更新失败，假设没有更新
            }
        }
    }
}