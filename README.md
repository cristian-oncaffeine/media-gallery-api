# media-gallery-api
A robust API for a media gallery application.
# overview
The media-gallery-api exposes various endpoints for CRUD operations on images, videos and documents. The API is integrated with a CDN (Azure CDN) for persistence. The media is downloaded from the CDN using the CDN endpoint (with a performance benefit in case the API is executed on different geographical regions), and uploaded to Azure Blob Storage by performing operations directly on the storage.
## implementation details
The source code is in the [MediaGaller.API](./src/MediaGallery.API/) folder.
Unit tests are in the [MediaGallery.API.Tests](./src/MediaGallery.API.Tests/) folder.
### security
Jwt authentication is supported. In order to be able to call the endpoints, a Bearer token should be present in the Authorization header, and depending on the verb, specific claims are required (scope claims CanRead for GET and CanWrite for POST, PUT and DELETE).
```
  private static void AddAuthorizationPolicies(this WebApplicationBuilder builder)
  {
    builder.Services.AddAuthorizationBuilder().AddPolicy("CanReadScope", policy => policy.RequireClaim("scope", "CanRead"));
    builder.Services.AddAuthorizationBuilder().AddPolicy("CanWriteScope", policy => policy.RequireClaim("scope", "CanWrite"));
  }
```
### domain
The supported media elements are currently documents, images, and videos, implemented all as wrappers around Streams. [MediaElement](./src/MediaGallery.API/Domain/MediaElement.cs) is the base class for all media elements, and holds an identity reference (file name) and a reference to a data stream.
### services
The endpoints are interacting with the CDN via a couple of specialized media services. A media service can perform basic CRUD operations on the CDN and are called by the application endpoints.
### endpoints
The endpoints are specialized by media element type, and use media type specific service interfaces. They provide the stream payload to the services, and return the media streams for download. See [swagger](./src/MediaGallery.API/swagger.json) for API details.
## configuration
For CDN integration, the CDN endpoint, and the CDN storage connection string should be configured as secrets (they are both of type string).

```
CDN:Endpoint
CDN:storageConnectionString
```
## testing
### manual testing
#### configuration
In case you have provisioned a CDN, you must pass the CDN endpoint url and a connection string to the Azure Blob Storage service to the Media Gallery application.

For testing in the local development environment, from the API project folder, run: 
```
dotnet user-secrets init
dotnet user-secrets set "CDN:Endpoint" cdn-endpoint-hostname
dotnet user-secrets set "CDN:storageConnectionString" "storage-connection-string"
```
#### generate a test JWT using user-jwts
For testing the API from the swagger interface, use the following command to generate a basic test JWT:
```
dotnet user-jwts create --output token
```
You can add specific claims, such as CanRead or CanWrite like this:
```
dotnet user-jwts create --scope CanRead --scope CanWrite --output token
```
Make sure your application is running -  go to the application project folder and type
```
dotnet run
```
Open the application url in the browser and go to /swagger/index.html
Click on the Authorize button (right), and enter `Bearer ` followed by the `JWT` value.
## limitations
### media service generalization
Currently there are separate media service interfaces and implementation per media type (e.g. images, videos)
These can be generealized by templating, passing the media element identity type, and the media element type as generic types, and having a common implementation.
### memory caching
The implementation is based on streams, no media elements are actually kept in memory, therefore although MemoryCaching is enabled, it is not used. For returning a stream to the blob on CDN, the code could look like this:
```
string key = string.Format("__video__{0}", id.Value);
Video? result = (Video?)await _cache.GetOrCreateAsync(key, async entry => {
  string url = PathBuilder.Build(_cdnConfig.CdnEndpoint, _cdnConfig.VideosContainer, id.Value);
  entry.Value = new Video(id, await GetStreamAsync(url));
  return entry.Value;
});
return result ?? new Video(id);
```