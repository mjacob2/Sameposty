using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sameposty.Services.Configurator;
public class Configurator(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : IConfigurator
{
    public string ApiBaseUrl { get; private set; } = configuration.GetConnectionString("ApiBaseUrl") ?? throw new ArgumentNullException("No ApiBaseUrl provided in sppsettings.json");

    public string WwwRoot { get; private set; } = webHostEnvironment.WebRootPath;

    public int NumberFirstPostsGenerated { get; private set; } = configuration.GetValue<int>("Settings:NumberFirstPostsGenerated");



}
