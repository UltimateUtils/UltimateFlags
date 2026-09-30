using UltimateFlags.Api.v10.Utils;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app =
    builder
        .ConfigureServices()
        .ConfigurePipeline();

app.Run();
