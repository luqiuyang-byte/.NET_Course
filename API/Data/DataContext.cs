using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    //this is an empty constructor. Constructor is run everytime an instance was created
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    // this is a property. AppUser as entity name. Property name is "Users". It's going to represent the name of the table in DB when created.
    public DbSet<AppUser> Users{ get; set; }
}
