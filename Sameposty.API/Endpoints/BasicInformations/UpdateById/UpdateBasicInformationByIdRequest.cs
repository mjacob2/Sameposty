namespace Sameposty.API.Endpoints.BasicInformations.UpdateById;

public class UpdateBasicInformationByIdRequest
{
    public int Id { get; set; }

    public string Branch { get; set; }

    public string ProductsAndServices { get; set; }

    public string Goals { get; set; }

    public string Assets { get; set; }

    public int UserId { get; set; }
}
