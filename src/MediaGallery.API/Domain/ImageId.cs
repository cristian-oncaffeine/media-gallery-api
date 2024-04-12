namespace MediaGallery.API.Domain;

public struct ImageId
{
  public string Value { get; private set; }
  public ImageId(string fileName)
  {
    Value = fileName;
  }
}
