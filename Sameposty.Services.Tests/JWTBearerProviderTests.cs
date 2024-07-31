using FastEndpoints;
using Moq;
using Sameposty.Services.JWTService;
using Sameposty.Services.Secrets;

namespace Sameposty.Services.Tests;
public class JWTBearerProviderTests
{
    [Fact]
    public void ProvideAdminTokenWorks()
    {
        Factory.RegisterTestServices(s => { });
        var secretsProviderMock = new Mock<ISecretsProvider>();
        secretsProviderMock.Setup(x => x.JWTBearerTokenSignKey).Returns("hfgrtgdhgjhtudfghtyrewnhfjejwhufgdhgufghidgjid");

        var jwtBearerProvider = new JWTBearerProvider(secretsProviderMock.Object);

        var token = jwtBearerProvider.ProvideToken("0", "Admin", "Admin");

        Assert.NotNull(token);
    }

    [Fact]
    public void ProvideFreeUserTokenWorks()
    {
        Factory.RegisterTestServices(s => { });
        var secretsProviderMock = new Mock<ISecretsProvider>();
        secretsProviderMock.Setup(x => x.JWTBearerTokenSignKey).Returns("hfgrtgdhgjhtudfghtyrewnhfjejwhufgdhgufghidgjid");

        var jwtBearerProvider = new JWTBearerProvider(secretsProviderMock.Object);

        var token = jwtBearerProvider.ProvideToken("312", "user@user.com", "FreeUSer");

        Assert.NotNull(token);
    }
}
