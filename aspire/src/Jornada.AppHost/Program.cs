var builder = DistributedApplication.CreateBuilder(args);

var bff = builder.AddProject<Projects.Jornada_BFF>("jornada-bff");

builder.AddProject<Projects.Jornada_Websocket>("jornada-websocket");

builder.AddProject<Projects.Jornada_API>("jornada-api");

builder.AddProject<Projects.Jornada_Worker>("jornada-worker");


builder.AddNpmApp("front-angular", "../../../front/client-websocket/")
    .WithReference(bff)
    .WaitFor(bff)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
