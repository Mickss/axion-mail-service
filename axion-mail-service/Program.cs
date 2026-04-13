using axion_mail_service.Services;
using System.Net;

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
        throw new InvalidOperationException("Brevo:ApiKey not configured");
    client.DefaultRequestHeaders.Add("api-key", apiKey);
})
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    ConnectCallback = async (context, ct) =>
    {
        var entry = await Dns.GetHostEntryAsync(context.DnsEndPoint.Host);
        var ipv4 = entry.AddressList.First(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        await socket.ConnectAsync(ipv4, context.DnsEndPoint.Port, ct);
        return new System.Net.Sockets.NetworkStream(socket, true);
    }
});

builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.Urls.Add("http://*:24004");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
