using eAgenda.Infraestrutura.Orm;
using eAgenda.WebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eAgenda.Testes.Interface.Compartilhado;

public class SeleniumWebApplicationFactory : WebApplicationFactory<Program>
{
    private IHost? hostKestrel;

    public string UrlKestrel { get; private set; } = string.Empty;
    public IServiceProvider Servicos
    {
        get
        {
            if (hostKestrel is null)
                throw new InvalidOperationException("Servidor não iniciado");

            return hostKestrel.Services;
        }
    }

    public SeleniumWebApplicationFactory()
    {
        _ = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove o DbContext real configurado no Program
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            // Adiciona o DbContext em memória para os testes
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("eAgendaTestesDb"));
        });
    }

    // Cria DOIS hosts: um com TestServer (para o WebApplicationFactory)
    // e outro com Kestrel (para o Selenium).
    protected override IHost CreateHost(IHostBuilder builder)
    {
        IHost? hostTeste = null;

        try
        {
            hostTeste = builder.Build();

            // Ajusta o builder para usar Kestrel e porta aleatória
            builder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder.UseKestrel();
                webHostBuilder.UseUrls("http://127.0.0.1:0");
            });

            hostKestrel = builder.Build();
            hostKestrel.Start();

            // Descobre a URL gerada e guarda em UrlServidor
            var servidor = hostKestrel.Services.GetRequiredService<IServer>();
            var enderecosDoServidor = servidor.Features.Get<IServerAddressesFeature>();

            if (enderecosDoServidor is null)
                throw new InvalidOperationException("Não foi possível obter a URL do servidor");

            UrlKestrel = enderecosDoServidor.Addresses.Last();

            // Inicia o host de TestServer e devolve ele para o WebApplicationFactory
            hostTeste.Start();

            return hostTeste;
        }
        catch
        {
            hostKestrel?.Dispose();
            hostTeste?.Dispose();

            throw;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            hostKestrel?.Dispose();
    }
}
