using Microsoft.Net.Http.Headers;

namespace MediaGallery.API.Domain;

public class Video : MediaElement
{
  public VideoId Id { get; private set; }
  public Video(VideoId id) : base(id.Value)
  {
    Id = id;
    MimeType = "video/mp4";
  }
  public Video(VideoId id, Stream stream) : this(id)
  {
    Stream = stream;
  }
}
