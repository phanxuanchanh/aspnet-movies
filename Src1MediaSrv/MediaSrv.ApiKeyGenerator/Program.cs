using MediaSrv.ApiKeyGenerator;
using MediaSrv.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;

        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddSingleton<IConfiguration>(configuration);
        services.AddDbContextPool<MovieCdnContext>(o =>
            o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    })
    .Build();

Console.WriteLine("Hello!");
Console.WriteLine("This is the MovieCDN.ApiKeyGenerator project.");
Console.WriteLine("It is used to generate API keys for the MovieCDN service.");
Console.WriteLine("You can run this project to generate a new API key.\n");
Console.WriteLine("======================================================\n");
Console.WriteLine("To generate a new API key, type `generate`");

using IServiceScope scope = host.Services.CreateScope();
using MovieCdnContext context = scope.ServiceProvider.GetRequiredService<MovieCdnContext>();

do {
    string? command = Console.ReadLine();
    if(string.IsNullOrEmpty(command))
        continue;

    if (command != "generate")
        continue;
    
    Console.WriteLine("Generating new API key...");
    ApiKey apiKey = new ApiKey
    {
        ClientId = Helper.GenerateClientId(),
        SecretKey = Helper.GenerateClientSecret()
    };

    Console.WriteLine("\n\n****************************\n\n");
    Console.WriteLine("New API key generated:");
    Console.WriteLine($"ClientId: {apiKey.ClientId}");
    Console.WriteLine($"SecretKey: {apiKey.SecretKey}");
    Console.WriteLine("\n\n****************************\n\n");

    Console.WriteLine("Saving API key to database...");
    context.ApiKeys.Add(apiKey);
    await context.SaveChangesAsync();
    Console.WriteLine("API key saved successfully!");

    Console.WriteLine("....................................................................");
    Console.WriteLine("You can now use this API key in your MovieCDN application.");

} while (true);