using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
using System.Net.Http;
using AutoUpdaterNamespace;

namespace AutoUpdater.Tests
{
    public class AutoUpdaterTests : IDisposable
    {
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly AutoUpdaterNamespace.AutoUpdater _autoUpdater;

        public AutoUpdaterTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _autoUpdater = new AutoUpdaterNamespace.AutoUpdater("localDirectory", "http://example.com", "http://example.com/update");
            _autoUpdater.SetHttpClient(_mockHttpClient.Object);
        }

        // 其他测试方法保持不变

        [Fact]
        public void Dispose()
        {
            // 如果需要进行清理,在这里添加清理代码
        }
    }
}