using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MediaGallery.API.Domain;
using MediaGallery.API.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace MediaGallery.API.Services;

public class AzureDocumentMediaService : IDocumentMediaService
{
  private CdnConfigService _cdnConfig;
  private IMemoryCache _cache;
  private BlobServiceClient _blobServiceClient;
  private BlobContainerClient _documentContainerClient;

  public AzureDocumentMediaService(CdnConfigService config, IMemoryCache cache)
  {
    _cdnConfig = config;
    _cache = cache;

    _blobServiceClient = new BlobServiceClient(config.StorageConnectionString);
    _documentContainerClient = CreateContainerClient(config.DocumentsContainer);
  }

  public async Task<Image> GetAsync(ImageId id)
  {
    string url = PathBuilder.Build(_cdnConfig.CdnEndpoint, _cdnConfig.ImagesContainer, id.Value);
    return new Image(id, await GetStreamAsync(url));
  }

  public async Task<Document> GetAsync(DocumentId id)
  {
    string url = PathBuilder.Build(_cdnConfig.CdnEndpoint, _cdnConfig.DocumentsContainer, id.Value);
    return new Document(id, await GetStreamAsync(url));
  }
  public async Task CreateAsync(Document document)
  {
    BlobClient client = _documentContainerClient.GetBlobClient(document.Id.Value);
    await client.UploadAsync(document.Stream, false);
    await SetContentTypeAsync(client, document.MimeType);
  }
  public async Task UpdateAsync(Document document)
  {
    BlobClient client = _documentContainerClient.GetBlobClient(document.Id.Value);
    await client.UploadAsync(document.Stream, true);
    await SetContentTypeAsync(client, document.MimeType);
  }

  public async Task DeleteAsync(DocumentId id)
  {
    BlobClient client = _documentContainerClient.GetBlobClient(id.Value);
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
