using MediaGallery.API.Domain;

namespace MediaGallery.API.Endpoints;

public static class Documents
{
  public static async Task<IResult> GetAsync(string filename, IDocumentMediaService service)
  {
    Document document = await service.GetAsync(new DocumentId(filename));
    return Results.Stream(document.Stream ?? new MemoryStream(), document.MimeType);
  }
  public static async Task CreateAsync(string filename, IFormFile file, IDocumentMediaService service)
  {
    Document document = new Document(new DocumentId(filename), file.OpenReadStream());
    await service.CreateAsync(document);
  }
  public static async Task UpdateAsync(string filename, IFormFile file, IDocumentMediaService service)
  {
    Document document = new Document(new DocumentId(filename), file.OpenReadStream());
    await service.UpdateAsync(document);
  }
  public static async Task DeleteAsync(string filename, IDocumentMediaService service)
  {
    await service.DeleteAsync(new DocumentId(filename));
  }
}
