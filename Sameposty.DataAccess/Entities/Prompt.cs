namespace Sameposty.DataAccess.Entities;
public class Prompt : EntityBase
{
    public string ImagePrompt { get; set; } = string.Empty;

    public int PostId { get; set; }
}
