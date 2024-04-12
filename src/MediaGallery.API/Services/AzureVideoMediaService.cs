using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediaGallery.API.Domain;
using MediaGallery.API.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace MediaGallery.API.Services;

public class AzureVideoMediaService : IVideoMediaService
{
  private CdnConfigService _cdnConfig;
  private IMemoryCache _cache;
  private BlobServiceClient _blobServiceClient;
  private BlobContainerClient _videoContainerClient;

  public AzureVideoMediaService(CdnConfigService config, IMemoryCache cache)
  {
    _cdnConfig = config;
    _cache = cache;

    _blobServiceClient = new BlobServiceClient(config.StorageConnectionString);
    _videoContainerClient = CreateContainerClient(config.VideosContainer);
  }

  public async Task<Video> GetAsync(VideoId id)
  {
    string url = PathBuilder.Build(_cdnConfig.CdnEndpoint, _cdnConfig.VideosContainer, id.Value);
    return new Video(id, await GetStreamAsync(url));
  }
  public async Task CreateAsync(Video video)
  {
    BlobClient client = _videoContainerClient.GetBlobClient(video.Id.Value);
    await client.UploadAsync(video.Stream, false);
    await SetContentTypeAsync(client, video.MimeType);
  }

  public async Task UpdateAsync(Video video)
  {
    BlobClient client = _videoContainerClient.GetBlobClient(video.Id.Value);
    await client.UploadAsync(video.Stream, true);
    await SetContentTypeAsync(client, video.MimeType);
  }

  public async Task DeleteAsync(VideoId id)
  {
    BlobClient client = _videoContainerClient.GetBlobClient(id.Value);
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
