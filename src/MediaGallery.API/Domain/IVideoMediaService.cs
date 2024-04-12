namespace MediaGallery.API.Domain;

public interface IVideoMediaService
{
  Task<Video> GetAsync(VideoId id);
  Task CreateAsync(Video video);
  Task UpdateAsync(Video video);
  Task DeleteAsync(VideoId id);
}
