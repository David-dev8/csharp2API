using QuizRoyaleAPI.Extensions;
using QuizRoyaleAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Voeg Services toe aan de container

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// SignalR initieren
builder.Services.AddSignalR();

builder.Services.AddSwagger();

// Database initieren
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("QuizRoyaleDatabase"));

// Voeg services voor data toe
builder.Services.AddDataServices();

// JWT Tokens
builder.Services.AddJWT(builder.Configuration.GetSection("Authentication:Key").Value);

var app = builder.Build();

// Stel de HTTP request pipeline in.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Voeg een exceprion handler toe
app.UseExceptionHandler("/error");

// Maak de fotos in wwwroot beschikbaar
app.UseStaticFiles();

app.UseHttpsRedirection();

// Gebruik authenticatie en authorisatie
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Map de hub naar een endpoint
app.MapHub<GameHub>("/GameHub");

// Zet de serviceprovider in de State
QuizRoyaleAPI.Models.State.ServiceProvider = app.Services;

app.Run();
