var builder = DistributedApplication.CreateBuilder(args);


var api = builder.AddProject<Projects.API>("api")
                 .WithUrlForEndpoint("https", url =>
                 {
                     url.DisplayText = "API";
                     url.Url += "/";
                 });

builder.AddProject<Projects.Client>("client")
       .WithUrlForEndpoint("https", url =>
       {
           url.DisplayText = "Client";
           url.Url += "/";
       })
       .WithReference(api)
       .WaitFor(api);

builder.Build().Run();
