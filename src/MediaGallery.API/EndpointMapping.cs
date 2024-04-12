using MediaGallery.API.Endpoints;

namespace MediaGallery.API;

public static class EndpointMapping
{
  public static void MapEndpoints(this WebApplication app)
  {
    app.MapImageEndpoints();
    app.MapVideoEndpoints();
    app.MapDocumentEndpoints();

    app.MapGet("/", () => "Media Gallery API").AllowAnonymous();
  }
  private static void MapImageEndpoints(this WebApplication app)
  {
    app.MapGet("/images/{filename}", Images.GetAsync).RequireAuthorization("CanReadScope");
    app.MapPost("images/create/{filename}", Images.CreateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("images/update/{filename}", Images.UpdateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("images/delete/{filename}", Images.DeleteAsync).RequireAuthorization("CanWriteScope");
  }
  private static void MapVideoEndpoints(this WebApplication app)
  {
    app.MapGet("/videos/{filename}", Videos.GetAsync).RequireAuthorization("CanReadScope");
    app.MapPost("videos/create/{filename}", Videos.CreateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("videos/update/{filename}", Videos.UpdateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("videos/delete/{filename}", Videos.DeleteAsync).RequireAuthorization("CanWriteScope");
  }
  private static void MapDocumentEndpoints(this WebApplication app)
  {
    app.MapGet("/documents/{filename}", Documents.GetAsync).RequireAuthorization("CanReadScope");
    app.MapPost("documents/create/{filename}", Documents.CreateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("documents/update/{filename}", Documents.UpdateAsync).RequireAuthorization("CanWriteScope");
    app.MapPost("documents/delete/{filename}", Documents.DeleteAsync).RequireAuthorization("CanWriteScope");
  }
}
