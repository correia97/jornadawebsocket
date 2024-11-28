var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Jornada_BFF>("jornada-bff");

builder.AddProject<Projects.Jornada_Websocket>("jornada-websocket");

builder.AddProject<Projects.Jornada_API>("jornada-api");

builder.AddProject<Projects.Jornada_Worker>("jornada-worker");

builder.Build().Run();
