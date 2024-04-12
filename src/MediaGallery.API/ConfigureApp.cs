namespace MediaGallery.API;

public static class ConfigureApp
{
  public static void Configure(this WebApplication app)
  {
    app.AddAuth();
    app.AddSwagger();
  }
  private static void AddAuth(this WebApplication app)
  {
    app.UseAuthentication();
    app.UseAuthorization();
  }
  private static void AddSwagger(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
        // Expose the (previous) OpenAPI document
        app.UseSwagger();
        // Serve the Swagger UI
        app.UseSwaggerUI();
    }
  }
}
