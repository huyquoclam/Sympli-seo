using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc.Authorization;
using Sympli.WebAPI.Bootstrap;
using Sympli.WebAPI.Filters;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilterAttribute>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Logging.ClearProviders();
builder.Logging.AddEventLog(); //Set provider as the Event Viewer

// default request timeout
builder.Services.AddRequestTimeouts(options => {
    options.DefaultPolicy = new RequestTimeoutPolicy 
    { 
        Timeout = TimeSpan.FromMilliseconds(30000),
        TimeoutStatusCode = 504
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
