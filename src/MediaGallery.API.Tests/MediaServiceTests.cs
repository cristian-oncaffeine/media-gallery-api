using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaGallery.API.Domain;
using MediaGallery.API.Services;
using Moq;
using System.Text;
using System.Runtime.CompilerServices;

namespace MediaGallery.API.Tests;

[TestClass]
public class MediaServiceTests
{
    // private readonly CdnConfigService _config;
    private readonly Mock<IImageMediaService> _mockedService;

    // public MediaServiceTests()
    // {
    //     // _config = new CdnConfigService("/", "xxx", "images", "videos", "documents");
    // }

    public MediaServiceTests()
    {
        _mockedService = new Mock<IImageMediaService>();
    }

    [TestMethod]
    public async Task MediaServiceCanGetAnImageStream()
    {
        // Arrange
        ImageId id = new ImageId("hello.png");
        Image image = new Image(id);
        image.Stream = new MemoryStream(Encoding.UTF8.GetBytes("hello" ?? ""));
        
        _mockedService.Setup(p => p.GetAsync(id)).ReturnsAsync(image);

        // Act
        Image image2 = await _mockedService.Object.GetAsync(id);

        // Assert
        using(Stream stream = image2.Stream ?? new MemoryStream())
        using(var reader = new StreamReader(stream))
        {
            string result = reader.ReadToEnd();
            Assert.AreEqual("hello", result);
        }
    }
}