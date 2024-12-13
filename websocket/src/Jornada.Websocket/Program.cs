using Jornada.Websocket.Hubs;
using Jornada.Websocket.Interfaces;
using Jornada.Websocket.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddScoped<IUsuarioRepo, UsuarioRepo>();


// Add services to the container.
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration.GetValue<string>("redis:connectionstring"), opt =>
    {
        opt.Configuration.ChannelPrefix = RedisChannel.Literal("JornadaWebsocket");
        opt.ConnectionFactory = async writer =>
        {
            var config = new ConfigurationOptions
            {
                AbortOnConnectFail = false
            };
            config.EndPoints.Add(builder.Configuration.GetValue<string>("redis:host"), builder.Configuration.GetValue<int>("redis:port"));
            config.AllowAdmin = true;
            config.Password = builder.Configuration.GetValue<string>("redis:password");
            var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
            connection.ConnectionFailed += (_, e) =>
            {
                Console.WriteLine("Connection to Redis failed.");
            };

            if (!connection.IsConnected)
            {
                Console.WriteLine("Did not connect to Redis.");
            }

            return connection;
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins("http://localhost:4200", "http://localhost:8085", "http://localhost:8084", "http://localhost:4200", "http://localhost:80", "http://localhost:8080", "http://front:80", "http://front:8080", "http://front:8085")
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "OPTIONS")
        .AllowCredentials();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseCors();
app.MapHub<NotificacaoHub>("/notificacaohub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
