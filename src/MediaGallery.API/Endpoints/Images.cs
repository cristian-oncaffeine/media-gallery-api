using MediaGallery.API.Domain;

namespace MediaGallery.API.Endpoints;

public static class Images
{
  public static async Task<IResult> GetAsync(string filename, IImageMediaService service)
  {
    Image image = await service.GetAsync(new ImageId(filename));
    return Results.Stream(image.Stream ?? new MemoryStream(), image.MimeType);
  }
  public static async Task CreateAsync(string filename, IFormFile file, IImageMediaService service)
  {
    Image image = new Image(new ImageId(filename), file.OpenReadStream());
    await service.CreateAsync(image);
  }
  public static async Task UpdateAsync(string filename, IFormFile file, IImageMediaService service)
  {
    Image image = new Image(new ImageId(filename), file.OpenReadStream());
    await service.UpdateAsync(image);
  }
  public static async Task DeleteAsync(string filename, IImageMediaService service)
  {
    await service.DeleteAsync(new ImageId(filename));
  }
}
