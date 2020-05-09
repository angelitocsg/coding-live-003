# API documentation with Swagger | Coding Live #003

## Getting Started

These instructions is a part of a live coding video.

### Prerequisites

-   .NET Core 3.1 SDK - https://dotnet.microsoft.com/download

## Example project

Create a base folder `CodingLive003`.

Create the .gitignore file based on file https://github.com/github/gitignore/blob/master/VisualStudio.gitignore

### Project sequence

-   Create project
-   Add packages
-   Entities > Client
-   Requests > CreateClientRequest
-   Requests > UpdateClientRequest
-   Responses > ClientResponse
-   MockData > ClientMock
-   Controllers > ClientsController
-   Wwwroot > Html Documentation
-   Startup > Swagger configuration
-   Controllers > Data annotations

### Create an API project

```bash
dotnet new webapi --name MyApiWithDoc
```

#### Add packages

```bash
dotnet add package Swashbuckle.AspNetCore -v 5.0.0
dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson -v 3.1.3
```

#### Basic Swagger configuration

```csharp
...
private static string GetPathOfXmlFromAssembly() => Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
...
services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
    });
...
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API with Documentation", Version = "v1" });
    options.IncludeXmlComments(GetPathOfXmlFromAssembly());
});
```

```csharp
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API width Documentation V1");
    c.RoutePrefix = string.Empty;
});
```

## References

https://restfulapi.net/

https://swagger.io/

https://docs.microsoft.com/pt-br/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio-code
