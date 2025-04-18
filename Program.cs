
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Microsoft.OpenApi.Models;


using Harendra_Ayurveda_HMS_WEB_API.Service.Branch;

using Harendra_Ayurveda_HMS_WEB_API.Helpers;
using Harendra_Ayurveda_HMS_WEB_API.Service.Auth;
using Harendra_Ayurveda_HMS_WEB_API;
using Harendra_Ayurveda_HMS_WEB_API.Middleware;
using Harendra_Ayurveda_HMS_WEB_API.Service.TreatmentCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.ClinicCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.DiseaseCode;
using Harendra_Ayurveda_HMS_WEB_API.Service.DrugCategory;
using Harendra_Ayurveda_HMS_WEB_API.Service.Treatment;
using Harendra_Ayurveda_HMS_WEB_API.Service.Patient;
using Harendra_Ayurveda_HMS_WEB_API.Service.Drug;
using Harendra_Ayurveda_HMS_WEB_API.Service.Rates;
using Harendra_Ayurveda_HMS_WEB_API.Service.Offers;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
var version = builder.Configuration.GetConnectionString("MySqlVersion");
var serverVersion = new MySqlServerVersion(ServerVersion.Parse(version));

GlobalAttributeHelper.mySqlConfiguration.connectionString = connectionString;

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ITreatmentCategoryService, TreatmentCategoryService>();
builder.Services.AddScoped<IClinicCategoryService, ClinicCategoryService>();
builder.Services.AddScoped<IDiseaseCodeService,DiseaseCodeService >();
builder.Services.AddScoped<IDrugCategoryService, DrugCategoryService>();
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IRatesService, RatesService>();
builder.Services.AddScoped<IOffersService, OffersService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db context for MySql
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseMySql(connectionString, serverVersion));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Harendra Ayurveda HMS WEB API",
        Description = ".NET 8 Web API"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173", "https://harendra-ayu-hms.cadvex.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.UseMiddleware<CreateByMiddleware>();

app.MapControllers();

// Create Db tables if not exists
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    dbContext.Database.EnsureCreated();

    // Create Pre-Defined functions
    app.CreateDatabase(dbContext);

    DbSeeder.Seed(Assembly.GetEntryAssembly(), dbContext);
}

app.Run();
