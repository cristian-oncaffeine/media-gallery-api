namespace MediaGallery.API.Services;

public class CdnConfigService
{
    private string _cdnEndpoint;
    private string _imagesContainer;
    private string _videosContainer;
    private string _documentsContainer;
    private string _storageConnectionString;
    public CdnConfigService(string cdnEndpoint, string storageConnectionString, string imagesContainer, string videosContainer, string documentsContainer) 
    {
      _cdnEndpoint = cdnEndpoint;
      _storageConnectionString = storageConnectionString;
      _imagesContainer = imagesContainer;
      _videosContainer = videosContainer;
      _documentsContainer = documentsContainer;
    }
    public string CdnEndpoint => _cdnEndpoint;
    public string StorageConnectionString => _storageConnectionString;
    public string ImagesContainer => _imagesContainer;
    public string VideosContainer => _videosContainer;
    public string DocumentsContainer => _documentsContainer;
}