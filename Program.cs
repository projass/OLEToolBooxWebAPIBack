using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OLEToolBoxWebAPIPruebas.Models;
using OLEToolBoxWebAPIPruebas.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Integramos el filtro de excepción para todos los controladores
    // options.Filters.Add(typeof(FiltroDeExcepcion));
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PruebasoletoolboxContext>(options =>
{

    options.UseSqlServer(connectionString);

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}

);

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<FileStorageManager>();
builder.Services.AddTransient<OperationsService>();
builder.Services.AddTransient<HashService>();

builder.Services.AddTransient<TokenService>();

var clave = builder.Configuration["ClaveJWT"];



// ----------------- TOKEN --------------------
// Configuramos la seguridad en el proyecto. Manifestamos que se va a implementar la seguridad
// mediante JWT firmados por la firma que está en el app.settings.development.json con el nombre ClaveJWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                   {
                   ValidateIssuer = false, // Esto es porque en el GenerarToken se puede poner el issuer: "Persona que lo envía" (Es opcional ponerlo en el GenerarToken)
                   ValidateAudience = false, // Esto es porque en el GenerarToken se puede poner el audience: "Quienes lo reciben" (Es opcional ponerlo en el GenerarToken)
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(clave))
                   });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        // builder.WithOrigins("https://www.almacenjuanluisusuario.com").WithMethods("GET").AllowAnyHeader();
        // builder.WithOrigins("https://www.almacenjuanluisadmin.com").AllowAnyMethod().AllowAnyHeader();
        // builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();
        // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

        /**
         * 
         * Politica CORS acepta todas las peticiones de cualquier IP, par cualquier método, con cualquier cabecera.
         * 
         **/

        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();


    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
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
app.UseCors();
app.MapControllers();

app.Run();
