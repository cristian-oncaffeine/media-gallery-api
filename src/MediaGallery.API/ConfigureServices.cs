using MediaGallery.API.Domain;
using MediaGallery.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace MediaGallery.API;

public static class ConfigureServices
{
  public static void AddServices(this WebApplicationBuilder builder)
  {
    builder.AddJwtAuth();
    builder.AddCDNConfig();
    builder.AddMediaServices();
    builder.AddSwagger();
  }

  private static void AddJwtAuth(this WebApplicationBuilder builder)
  {
    builder.AddAuthorizationPolicies();
    builder.Services.AddAuthentication().AddJwtBearer();
    builder.Services.AddAuthorization();
  }

  private static void AddAuthorizationPolicies(this WebApplicationBuilder builder)
  {
    builder.Services.AddAuthorizationBuilder().AddPolicy("CanReadScope", policy => policy.RequireClaim("scope", "CanRead"));
    builder.Services.AddAuthorizationBuilder().AddPolicy("CanWriteScope", policy => policy.RequireClaim("scope", "CanWrite"));
  }

  private static void AddCDNConfig(this WebApplicationBuilder builder)
  {
    builder.Services.AddSingleton(
      new CdnConfigService(
        builder.Configuration["CDN:Endpoint"] ?? "", 
        builder.Configuration["CDN:storageConnectionString"] ?? "", 
        "images", "videos", "documents")
    );
  }
  private static void AddMediaServices(this WebApplicationBuilder builder)
  {
    builder.Services.AddSingleton<IImageMediaService, AzureImageMediaService>();
    builder.Services.AddSingleton<IVideoMediaService, AzureVideoMediaService>();
    builder.Services.AddSingleton<IDocumentMediaService, AzureDocumentMediaService>();
    builder.Services.AddMemoryCache();
  }

  private static void AddSwagger(this WebApplicationBuilder builder)
  {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwashbuckleServices();
  }
  private static IServiceCollection AddSwashbuckleServices(this IServiceCollection services)
  {
      services.AddSwaggerGen(options => {
      options.SwaggerDoc("v1", new OpenApiInfo { Title = "MediaGallery.API", Version = "v1" });
      OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme
      {
            Name = HeaderNames.Authorization,                    
            Type = SecuritySchemeType.ApiKey,                     
            In = ParameterLocation.Header,                       
            Description = "JWT Authorization header",              
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,        
                Type = ReferenceType.SecurityScheme                 
            }
      };
      options.AddSecurityDefinition(securityDefinition.Reference.Id, securityDefinition);
      options.AddSecurityRequirement(new OpenApiSecurityRequirement{{securityDefinition, Array.Empty<string>()}});
    });
    return services;
  }
}
