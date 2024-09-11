using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoUpdaterLib;
using Moq;
using Moq.Protected;
using Xunit;

namespace AutoUpdater.Tests
{
    public class AutoUpdaterTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly AutoUpdaterLib.AutoUpdater _autoUpdater;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public AutoUpdaterTests()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_testDirectory);
            _autoUpdater = new AutoUpdaterLib.AutoUpdater(_testDirectory, "http://example.com");
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _autoUpdater.SetHttpClient(client);
        }

        public void Dispose()
        {
            Directory.Delete(_testDirectory, true);
        }

        [Fact]
        public void InitializeLocalDirectory_CreatesDirectoryAndMd5File()
        {
            var result = _autoUpdater.InitializeLocalDirectory();
            var response = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);

            Assert.Equal("success", response?["status"].GetString());
            Assert.True(File.Exists(Path.Combine(_testDirectory, "md5.json")));
        }

        [Fact]
        public async Task UpdateFilesAsync_DownloadsNewFiles()
        {
            var remoteMd5 = new Dictionary<string, string>
            {
                { "test.txt", "1234567890abcdef" }
            };

            _mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(remoteMd5))
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("Test file content")
                });

            var result = await _autoUpdater.UpdateFilesAsync();
            var response = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result);

            Assert.Equal("success", response?["status"].GetString());
            Assert.Contains("test.txt", response?["updated_files"].EnumerateArray().Select(x => x.GetString()));
        }

        [Fact]
        public async Task CheckForUpdatesAsync_ReturnsTrueWhenUpdatesAvailable()
        {
            // 创建一个本地文件
            File.WriteAllText(Path.Combine(_testDirectory, "test.txt"), "Local content");

            var remoteMd5 = new Dictionary<string, string>
            {
                { "test.txt", "1234567890abcdef" } // 与本地文件的 MD5 不同
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(remoteMd5))
                });

            var result = await _autoUpdater.CheckForUpdatesAsync();

            Assert.True(result);
        }
    }
}