using axion_mail_service;
using axion_mail_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailerSendSettings>(
    builder.Configuration.GetSection("MailerSend"));
builder.Services.AddHttpClient();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://localhost:25004");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
