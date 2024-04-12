using MediaGallery.API.Domain;
namespace MediaGallery.API.Endpoints;

public static class Videos
{
  public static async Task<IResult> GetAsync(string filename, IVideoMediaService service)
  {
    Video video = await service.GetAsync(new VideoId(filename));
    return Results.Stream(video.Stream ?? new MemoryStream(), video.MimeType);
  }
  public static async Task CreateAsync(string filename, IFormFile file, IVideoMediaService service)
  {
    Video video = new Video(new VideoId(filename), file.OpenReadStream());
    await service.CreateAsync(video);
  }
  public static async Task UpdateAsync(string filename, IFormFile file, IVideoMediaService service)
  {
    Video video = new Video(new VideoId(filename), file.OpenReadStream());
    await service.UpdateAsync(video);
  }
  public static async Task DeleteAsync(string filename, IVideoMediaService service)
  {
    await service.DeleteAsync(new VideoId(filename));
  }
}
