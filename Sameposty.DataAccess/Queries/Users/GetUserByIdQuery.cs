﻿using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.Users;
public class GetUserByIdQuery : QueryBase<User>
{
    public required int Id { get; set; }
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        return await db.Users.FindAsync(Id);
    }
}
