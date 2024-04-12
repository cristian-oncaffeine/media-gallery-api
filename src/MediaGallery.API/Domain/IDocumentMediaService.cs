namespace MediaGallery.API.Domain;

public interface IDocumentMediaService
{
  Task<Document> GetAsync(DocumentId id);
  Task CreateAsync(Document document);
  Task UpdateAsync(Document document);
  Task DeleteAsync(DocumentId id);
}
