using Jornada.Websocket.Hubs;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddSignalR()
    .AddStackExchangeRedis("localhost:6379,allowAdmin=true,password=redis-stack", opt =>
    {
        opt.Configuration.ChannelPrefix = RedisChannel.Literal("JornadaWebsocket");
        opt.ConnectionFactory = async writer =>
        {
            var config = new ConfigurationOptions
            {
                AbortOnConnectFail = false
            };
            config.EndPoints.Add("localhost", 0);
            config.SetDefaultPorts();
            config.AllowAdmin = true;
            config.Password = "redis-stack";
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
        b.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .WithMethods("GET", "POST")
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.MapHub<NotificacaoHub>("/notificacaohub");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
