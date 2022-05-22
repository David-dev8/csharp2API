using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizRoyaleDbContext>(options => 
options.UseMySql(
    builder.Configuration.GetConnectionString("QuizRoyaleDatabase"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("QuizRoyaleDatabase"))));

builder.Services.AddScoped<IQuestionService, DbQuestionService>();

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
