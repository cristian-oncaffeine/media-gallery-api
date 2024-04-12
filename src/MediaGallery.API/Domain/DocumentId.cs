namespace MediaGallery.API.Domain;

public struct DocumentId
{
  public string Value { get; private set; }
  public DocumentId(string fileName)
  {
    Value = fileName;
  }
}
