using MediaGallery.API;
using MediaGallery.API.Endpoints;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServices();
        var app = builder.Build();
        app.Configure();
        app.MapEndpoints();
        app.Run();
    }
}