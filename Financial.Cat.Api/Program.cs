using Financial.Cat.Api.FluentValidators.FluentValidatorsResponses;
using Financial.Cat.Api.Middlewares;
using Financial.Cat.Api.Swagger.Filters;
using Financial.Cat.Application.Accessors;
using Financial.Cat.Application.Profiles;
using Financial.Cat.Application.UseCases.Services;
using Financial.Cat.Domain.Interfaces.Repositories;
using Financial.Cat.Domain.Interfaces.Services;
using Financial.Cat.Infrastructure.Configs;
using Financial.Cat.Infrastructure.DB.Repository;
using Financial.Cat.Infrastructure.ExternalProviders;
using Financial.Cat.Infrastructure.Generators;
using Financial.Cat.Infrustructure.Configs;
using Financial.Cat.Infrustructure.DB.Contexts;
using Financial.Cat.Infrustructure.DB.Repository;
using Financial.Cat.Infrustructure.Generators;
using Financial.Cat.Infrustructure.Logger.Queues;
using Financial.Cat.Infrustructure.Logger;
using Financial.Cat.Infrustructure.Providers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Financial.Cat.Application.UseCases;
using Financial.Cat.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	})
	.AddFluentValidation(opt =>
	{
		opt.ValidatorOptions.DefaultRuleLevelCascadeMode = CascadeMode.Continue;
		opt.ValidatorOptions.DefaultClassLevelCascadeMode = CascadeMode.Continue;
	}).ConfigureApiBehaviorOptions(options =>
	{
		options.InvalidModelStateResponseFactory = CustomProblemDetails.MakeValidationResponse;
	});

builder.Host.ConfigureLogging(opt =>
{
	opt.ClearProviders();
	opt.AddConsole();
	opt.AddDbLogger();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
		x => x.UseNetTopologySuite()));

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<OtpConfig>(builder.Configuration.GetSection("Otp"));
builder.Services.Configure<AccountConfig>(builder.Configuration.GetSection("Account"));

builder.Services.AddScoped<IAuthOperationRepository, AuthOperationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemNomenclatureRepository, ItemNomenclatureRepository>();
builder.Services.AddScoped<ISettingLimitRepository, SettingLimitRepository>();
builder.Services.AddScoped<IDbLogRepository, DbLogRepository>();

builder.Services.AddScoped<ExceptionGenerator>();
builder.Services.AddScoped<ResourceProvider>();
builder.Services.AddScoped<EmailExternalProvider>();
builder.Services.AddScoped<AccountPasswordGenerator>();
builder.Services.AddScoped<AccountTokenGenerator>();
builder.Services.AddScoped<OtpGenerator>();

builder.Services.AddScoped<IUserContextAccessor, UserContextAccessor>();

builder.Services.AddScoped<IFileWriterService, FileWriterService>();
builder.Services.AddScoped<DbLogger>();
builder.Services.AddTransient<DbLoggerDeliverQueue>();

builder.Services.AddAutoMapper(cfg =>
{
	cfg.AddProfile<ApplicationProfile>();
	cfg.AllowNullCollections = true;
	cfg.ShouldUseConstructor = ci => !ci.IsPrivate;
});

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Issuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
	});

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financial.Cat.Api", Version = "v1" });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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
			Array.Empty<string>()
		}
	});

	/*// Set the comments path for the Swagger JSON and UI.
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);

	c.SchemaFilter<EnumTypesSchemaFilter>(xmlPath);
	c.DocumentFilter<EnumTypesDocumentFilter>();*/
});

/*foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
	builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}*/

builder.Services.AddApplicationMediator();
builder.Services.AddInfrustructureMediator();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
	await context.Database.MigrateAsync();
}

var configuration = app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>();
configuration.AssertConfigurationIsValid();
configuration.CompileMappings();

app.UseDeveloperExceptionPage();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Financial.Cat.Api v1"));

app.UseHttpsRedirection();

app.UseCors(builder => builder
	.SetIsOriginAllowed(_ => true)
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials());

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<TransactionMiddleware>();


app.Run();