using Sameposty.Services.DTOs;

namespace Sameposty.API.Endpoints.Users.GetAllUsers;

public class GetAllUsersResponse
{
    public List<UserBasicInfo> Users { get; set; }
}