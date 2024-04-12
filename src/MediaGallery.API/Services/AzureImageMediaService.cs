using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediaGallery.API.Domain;
using MediaGallery.API.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace MediaGallery.API.Services;

public class AzureImageMediaService : IImageMediaService
{
  private CdnConfigService _cdnConfig;
  private IMemoryCache _cache;
  private BlobServiceClient _blobServiceClient;
  private BlobContainerClient _imageContainerClient;

  public AzureImageMediaService(CdnConfigService config, IMemoryCache cache)
  {
    _cdnConfig = config;
    _cache = cache;

    _blobServiceClient = new BlobServiceClient(config.StorageConnectionString);
    _imageContainerClient = CreateContainerClient(config.ImagesContainer);
  }

  public async Task<Image> GetAsync(ImageId id)
  {
    string url = PathBuilder.Build(_cdnConfig.CdnEndpoint, _cdnConfig.ImagesContainer, id.Value);
    return new Image(id, await GetStreamAsync(url));
  }

  public async Task CreateAsync(Image image)
  {
    BlobClient client = _imageContainerClient.GetBlobClient(image.Id.Value);
    await client.UploadAsync(image.Stream, false);
    await SetContentTypeAsync(client, image.MimeType);
  }

  public async Task UpdateAsync(Image image)
  {
    BlobClient client = _imageContainerClient.GetBlobClient(image.Id.Value);
    await client.UploadAsync(image.Stream, true);
    await SetContentTypeAsync(client, image.MimeType);
  }

  public async Task DeleteAsync(ImageId id)
  {
    BlobClient client = _imageContainerClient.GetBlobClient(id.Value);
    await client.DeleteAsync();
  }

  private async Task SetContentTypeAsync(BlobClient blob, string contentType)
  {
    BlobHttpHeaders blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
    await blob.SetHttpHeadersAsync(blobHttpHeader);
  }

  private BlobContainerClient CreateContainerClient(string name)
  {
    var containerClient = _blobServiceClient.GetBlobContainerClient(name);
    containerClient.CreateIfNotExists();
    return containerClient;
  }

  private async Task<Stream> GetStreamAsync(string url)
  {
    HttpClient httpClient = new HttpClient();
    return await httpClient.GetStreamAsync(url);
  }
}
