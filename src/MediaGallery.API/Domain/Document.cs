namespace MediaGallery.API.Domain;

public class Document : MediaElement
{
  public DocumentId Id { get; private set; }
  public Document(DocumentId id) : base(id.Value)
  {
    Id = id;
    MimeType = "text/html";
  }
  public Document(DocumentId id, Stream stream) : this(id)
  {
    Stream = stream;
  }
}
