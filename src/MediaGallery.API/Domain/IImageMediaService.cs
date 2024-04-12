namespace MediaGallery.API.Domain;

public interface IImageMediaService
{
  Task<Image> GetAsync(ImageId id);
  Task CreateAsync(Image image);
  Task UpdateAsync(Image image);
  Task DeleteAsync(ImageId id);
}
