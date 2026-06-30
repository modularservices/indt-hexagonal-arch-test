using Indt.Teste.PropostaService.Infra.Persistence.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.Sources.Clear();

            config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables();
        });

        builder.ConfigureServices(services =>
        {
            // remove DbContext original
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PropostaDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // adiciona DbContext usando banco de teste
            services.AddDbContext<PropostaDbContext>(options =>
            {
                var configuration = services.BuildServiceProvider()
                    .GetRequiredService<IConfiguration>();

                var connectionString =
                    configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString);
            });
        });
    }
}