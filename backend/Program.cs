using Microsoft.EntityFrameworkCore;
using ResumeAnalyser.Api.Configuration;
using ResumeAnalyser.Api.Data;
using ResumeAnalyser.Api.Infrastructure.FileValidation;
using ResumeAnalyser.Api.Services.Implementations;
using ResumeAnalyser.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenAiOptions>(builder.Configuration.GetSection(OpenAiOptions.SectionName));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("MySql");
var mysqlServerVersion = new MySqlServerVersion(new Version(8, 0, 36));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        options.UseMySql(
            "server=localhost;port=3306;database=resume_analyser;user=app_user;password=change_me;",
            mysqlServerVersion);
        return;
    }

    options.UseMySql(connectionString, mysqlServerVersion);
});

builder.Services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
builder.Services.AddScoped<IAiResumeAnalysisService, MockAiResumeAnalysisService>();
builder.Services.AddScoped<IResumeAnalysisService, ResumeAnalysisService>();
builder.Services.AddScoped<IResumeAnalysisHistoryService, ResumeAnalysisHistoryService>();
builder.Services.AddScoped<IPdfFileValidator, PdfFileValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendDev");
app.MapControllers();
app.Run();
