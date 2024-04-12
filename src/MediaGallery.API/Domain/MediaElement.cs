namespace MediaGallery.API.Domain;

public abstract class MediaElement
{
  public string Name { get; }
  public Stream? Stream { get; set; }
  public string MimeType { get; set; }

  public MediaElement(string name)
  {
    Name = name;
    MimeType = "unknown";
  }
  public MediaElement(string id, Stream stream) : this (id)
  {
    Stream = stream;
  }
}
