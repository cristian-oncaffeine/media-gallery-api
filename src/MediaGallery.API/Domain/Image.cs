namespace MediaGallery.API.Domain;

public class Image : MediaElement
{
  public ImageId Id { get; private set; }
  public Image(ImageId id) : base(id.Value)
  {
    MimeType = "image/jpeg";  
    Id = id;
  }
  public Image(ImageId id, Stream stream) : this(id)
  {
    Stream = stream;
  }
}
