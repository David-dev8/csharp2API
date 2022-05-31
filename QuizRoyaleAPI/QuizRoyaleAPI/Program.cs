using QuizRoyaleAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Database
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("QuizRoyaleDatabase"));

// Voeg services voor data toe
builder.Services.AddDataServices();

// JWT Tokens
builder.Services.AddJWT(builder.Configuration.GetSection("Authentication:Key").Value);

builder.WebHost.UseKestrel().UseIIS().UseIISIntegration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();