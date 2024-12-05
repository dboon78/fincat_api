using api.Data;
using api.Interfaces;
using api.Models;
using api.Repository;
using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "FinCat API", Version = "v1" });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });    
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
});



builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDBContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser,IdentityRole>(options=>{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength=12;
    options.Tokens.PasswordResetTokenProvider=TokenOptions.DefaultProvider;
})
.AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();  // Ensure token providers are added;

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(6)); 

builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options=>{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )

    };
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    if(builder.Environment.IsDevelopment()){
        googleOptions.ReturnUrlParameter=configuration["Website:Url"];
    }else{
        //googleOptions.ReturnUrlParameter    
    }
});
//Stateless Transient injection

//Stateful
builder.Services.AddScoped<IEmailSender,EmailService>();
builder.Services.AddScoped<IHoldingRepository,HoldingRepository>();
builder.Services.AddScoped<IStockRepository,StockRepository>();
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IPortfolioRepository,PortfolioRepository>();
builder.Services.AddScoped<IFMPService,FMPService>();
builder.Services.AddScoped<ITwelveDataService,TwelveDataService>();
builder.Services.AddScoped<IFinnhubService,FinnhubService>();
builder.Services.AddScoped<ICryptoPrices,CryptoPriceService>();
builder.Services.AddScoped<ICoinPaprika,CoinPaprikaService>();
builder.Services.AddScoped<ICoinApiService,CoinApiService>();
builder.Services.AddScoped<IFreeCurrencyService,FreeCurrencyService>();
builder.Services.AddTransient<IGovWatchService,GovWatchService>();
builder.Services.AddHttpClient<IFMPService,FMPService>();
builder.Services.AddHttpClient<ITwelveDataService,TwelveDataService>();
builder.Services.AddHttpClient<IFinnhubService,FinnhubService>();
builder.Services.AddHttpClient<ICryptoPrices,CryptoPriceService>();
builder.Services.AddHttpClient<ICoinPaprika,CoinPaprikaService>();
builder.Services.AddHttpClient<IFreeCurrencyService,FreeCurrencyService>();


// builder.Services.AddHttpClient<IFMPService,FMPService>();
builder.Services.AddControllers().AddNewtonsoftJson(options=>{
    options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "FinCat API";
    configure.PostProcess = document =>
    {
        document.Info = new NSwag.OpenApiInfo
        {
            Version = "v1",
            Title = "FinCat API",
            Description = "A finance portfolio management tool.",
            Contact = new NSwag.OpenApiContact
            {
                Name = "FinCat API Contract",
                Url = configuration["Website:Url"] + "/contact"

            },
            License = new NSwag.OpenApiLicense
            {
                Name = "FinCat API License",
                Url = configuration["Website:Url"] + "/license"
            }

        };
    };

  
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    // app.UseOpenApi((p)=>{
    //     p.Path= "/swagger/v1/swagger.yaml";
    // });
    app.UseSwagger();
      
    app.UseSwaggerUI((c)=>{
        // c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "FinCat API V1");
        c.DocumentTitle="FinCat API";
        
        //TBD custom swagger css and javasript
    });

    app.UseReDoc(options=>{
        options.Path="/redoc";
        options.DocumentTitle="FinCat ReDoc";
    });

}
app.UseHttpsRedirection();
app.UseCors(x=>x
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin=>true)
                );

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
