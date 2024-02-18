using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Product_API_Version_6.Database_Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();



// *********API Versioning*********************************
builder.Services.AddApiVersioning(options =>
{
    //Http response also contains the version number(nice to know which API is actually used
    options.ReportApiVersions = true;
    //Custom API version
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    //If no other version is called default version is used.
    options.AssumeDefaultVersionWhenUnspecified = true;

    //HTTP Header Versioning
    //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");

    //For QueryString API Versioning
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    // => (- HTTP Header Versioning)   =>just remove the HTTP Header versioning and controller part leave as it is.
    //https://localhost:7268/products?api-version=1.0
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//*************Add the database here => connecting the database and inmemory database as well*****************

builder.Services.AddDbContext<ShopContext>(options =>
{
    options.UseInMemoryDatabase("Shop");
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

// **********************For the minimal API**********************
//app.MapGet("/", () => "Hello World!");
app.Run();