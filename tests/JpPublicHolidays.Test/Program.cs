using Microsoft.Testing.Platform.Builder;
using NextUnit.Platform;

var builder = await TestApplication.CreateBuilderAsync(args);
builder.AddNextUnit();
using var app = await builder.BuildAsync();
return await app.RunAsync();
