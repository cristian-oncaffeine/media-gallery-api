namespace MediaGallery.API.Domain;

public struct VideoId
{
  public string Value { get; private set; }
  public VideoId(string fileName)
  {
    Value = fileName;
  }
}
