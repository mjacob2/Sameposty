using Sameposty.API.Endpoints.StripeWebhook;

namespace Sameposty.Services.Tests;
public class GetStreetNameWithNumbersTests
{
    [Fact]
    public void Should_Return_StreetName_With_BuildingNumber()
    {
        // Arrange
        string street = "Main Street";
        string buildingNumber = "123";

        // Act
        string result = StripeWebhookInvoicesEndpoint.GetStreetNameWithNumbers(street, buildingNumber, null);

        // Assert
        Assert.Equal("Main Street 123", result);
    }

    [Fact]
    public void Should_Return_StreetName_With_BuildingNumber_And_FlatNumber()
    {
        // Arrange
        string street = "Park Avenue";
        string buildingNumber = "456";
        string flatNumber = "101";

        // Act
        string result = StripeWebhookInvoicesEndpoint.GetStreetNameWithNumbers(street, buildingNumber, flatNumber);

        // Assert
        Assert.Equal("Park Avenue 456/101", result);
    }

    [Fact]
    public void Should_Return_StreetName_With_Empty_FlatNumber()
    {
        // Arrange
        string street = "Broadway";
        string buildingNumber = "789";
        string flatNumber = "";

        // Act
        string result = StripeWebhookInvoicesEndpoint.GetStreetNameWithNumbers(street, buildingNumber, flatNumber);

        // Assert
        Assert.Equal("Broadway 789", result);
    }

    [Fact]
    public void Should_Return_StreetName_With_Null_FlatNumber()
    {
        // Arrange
        string street = "High Street";
        string buildingNumber = "321";
        string flatNumber = null;

        // Act
        string result = StripeWebhookInvoicesEndpoint.GetStreetNameWithNumbers(street, buildingNumber, flatNumber);

        // Assert
        Assert.Equal("High Street 321", result);
    }
}
