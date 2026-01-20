using axion_mail_service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("Brevo", client =>
{
    client.BaseAddress = new Uri("https://api.brevo.com/v3/");
    var apiKey = builder.Configuration["Brevo:ApiKey"];
    if (string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("Brevo:ApiKey not configured in appsettings.json");
    }
    client.DefaultRequestHeaders.Add("api-key", apiKey);
});

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.Urls.Add("http://localhost:24004");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
