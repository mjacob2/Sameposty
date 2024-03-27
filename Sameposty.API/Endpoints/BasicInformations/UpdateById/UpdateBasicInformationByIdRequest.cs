namespace Sameposty.API.Endpoints.BasicInformations.UpdateById;

public class UpdateBasicInformationByIdRequest
{
    public int Id { get; set; }

    public string BrandName { get; set; }

    public string Audience { get; set; }

    public string Mission { get; set; }

    public string ProductsAndServices { get; set; }

    public string Goals { get; set; }

    public string Assets { get; set; }

    public int UserId { get; set; }
}
