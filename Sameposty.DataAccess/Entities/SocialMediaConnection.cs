namespace Sameposty.DataAccess.Entities;
public class SocialMediaConnection
{
    public enum SocialMediaPlatform
    {
        Facebook,
    }

    public int Id { get; set; }

    public SocialMediaPlatform Name { get; set; }

    public User User { get; set; }

    public int UserId { get; set; }
}
