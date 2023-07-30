using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.UseStaticFiles();
app.MapControllers();
app.Run();

static void LoadConfiguration(WebApplication app)
{
    app.Configuration.GetValue<string>("JwtKey");
    app.Configuration.GetValue<string>("ApitKeyName");
    app.Configuration.GetValue<string>("ApitKey");

    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
}

static void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal;
    });

    builder
        .Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}