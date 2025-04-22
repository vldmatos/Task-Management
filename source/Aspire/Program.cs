var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
                      .WithDataVolume()
                      .WithLifetime(ContainerLifetime.Persistent)
                      .WithPgAdmin(resource =>
                      {
                          resource.WithUrlForEndpoint("http", url => url.DisplayText = "PG Admin");
                          resource.WithLifetime(ContainerLifetime.Persistent);
                      })
                      .AddDatabase("projects-database");

var infrastructure = builder.AddProject<Projects.Infrastructure>("infrastructure")
                            .WithReference(database)
                            .WaitFor(database);

var api = builder.AddProject<Projects.API>("api")
                 .WithUrlForEndpoint("https", url =>
                 {
                     url.DisplayText += "Scalar";
                     url.Url += "/scalar";
                 })
                 .WithReference(database)
                 .WaitFor(database)
                 .WaitFor(infrastructure);

builder.AddProject<Projects.Web>("web")
       .WithUrlForEndpoint("https", url =>
       {
           url.DisplayText = "Web";
           url.Url += "/";
       })
       .WithReference(api)
       .WaitFor(api)
       .WaitFor(infrastructure);

builder.Build().Run();
