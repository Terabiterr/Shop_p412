using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class UsersContext : IdentityDbContext
{
    public UsersContext(DbContextOptions<UsersContext> options) : base(options) { }

}