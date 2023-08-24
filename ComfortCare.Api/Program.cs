using ComfortCare.Data;
using ComfortCare.Data.Interfaces;
using ComfortCare.Domain.BusinessLogic;
using ComfortCare.Domain.BusinessLogic.interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ComfortCareDbContext>( opt =>
{
    var configuration = builder.Configuration;
    var connectionString = configuration.GetConnectionString("ComfortCareDbConnectionString");
    opt.UseSqlServer(connectionString);
}
 );

builder.Services.AddTransient<RouteGenerator>();
builder.Services.AddTransient<SchemaGenerator>();
builder.Services.AddTransient<IPlanManager, PlanManager>();
builder.Services.AddTransient<IRouteRepo, ComfortCareRepository>();
builder.Services.AddTransient<IEmployeesRepo, ComfortCareRepository>();
builder.Services.AddTransient<IValidate, Validate>();
builder.Services.AddTransient<ISchema, Schema>();
builder.Services.AddTransient<IPeriod, Period>();

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
