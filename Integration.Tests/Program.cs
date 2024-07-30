
using Integration.Tests;
using Microsoft.Extensions.Configuration;
using Sameposty.Services.Secrets;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();
var configuration = builder.Build();
var secrets = new Secrets();
secrets = configuration.GetSection("Secrets").Get<Secrets>() ?? throw new ArgumentNullException("No local secrets.json provided or error while mapping");
var secretsProvider = new SecretsProvider(secrets);

Console.WriteLine("Start tests");

await TestFacebookPixelNotifier();


Console.WriteLine("End tests.");
Console.ReadLine();

async Task TestFacebookPixelNotifier()
{
    var fp = new FacebookPixelNotifierTests(secretsProvider);
    await fp.NotifyNewLeadTest("jakubicki.m@gmail.com");
}