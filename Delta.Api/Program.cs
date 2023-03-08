using Delta.Api.Dal;
using Delta.Api.DbContext;
using Delta.Api.IDal;
using Delta.Api.Model;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.Configure<ArangoDbSettingsModel>(builder.Configuration.GetSection("ArangoDbSettings"));

//builder.Services.AddSingleton<IDBContext, ArangoDBContext>();
builder.Services.AddTransient<IArangoDBContext, ArangoDBContext>();
builder.Services.AddTransient<IClientInfoDal, ClientInfoDal>();
builder.Services.AddTransient<IMdmInfoDal, MdmInfoDal>();
builder.Services.AddTransient<IPropertyInfoDal, PropertyInfoDal>();
builder.Services.AddTransient<IRelationInfoDal, RelationInfoDal>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
