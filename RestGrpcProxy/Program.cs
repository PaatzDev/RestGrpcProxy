using RestGrpcProxy;
using RestGrpcProxy.Services;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApplicationPartManager(m =>
        m.FeatureProviders.Add(new ControllerFeatureProvider()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var configService = new ConfigurationService();

builder.Services.AddSwaggerGen(c => 
    {
        c.EnableAnnotations();
        c.SwaggerDoc(configService.Config.ServiceVersion, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = configService.Config.ServiceVersion,
            Title = configService.Config.ServiceName,
            Description = configService.Config.ServiceDescription,
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/{configService.Config.ServiceVersion}/swagger.json", configService.Config.ServiceVersion);
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();