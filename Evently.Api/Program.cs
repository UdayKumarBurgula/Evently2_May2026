using Evently.Api.Extensions;
using Events.Module.Events.Api;
using MassTransit;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();

}

app.MapHealthChecks("/health");


EventsModule.MapEndPoint(app);


app.Run();
