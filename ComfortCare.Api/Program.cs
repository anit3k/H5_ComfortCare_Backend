using ComfortCare.Data;
using ComfortCare.Domain.BusinessLogic;
using ComfortCare.Domain.BusinessLogic.interfaces;
using ComfortCare.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IDistanceRequest, RouteDistanceAPI>();

builder.Services.AddDbContext<ComfortCareDbContext>( opt =>
{
    var configuration = builder.Configuration;
    var connectionString = configuration.GetConnectionString("ComfortCareDbConnectionString");
    opt.UseSqlServer(connectionString);
}
 );

builder.Services.AddTransient<IPlanManager, PlanManager>();
builder.Services.AddTransient<IRouteConstructionRepo, ComfortCareRepository>();
builder.Services.AddTransient<IValidate, Validate>();
builder.Services.AddTransient<IGetSchema, GetSchema>();

var _allowAllOriginsForDevelopment = "_allowAllOriginsForDevelopment";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _allowAllOriginsForDevelopment,
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
