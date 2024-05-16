namespace API.Entities
{
    public class AppUser
    {
    // These are properties, they represent columns in the DB
    // Therefore, everytime new properties was created, we need to teel database to create additional column to accomodate them
    // That was done by creating a migration
    public int Id {get; set;}    
    //this property liturally named "Id" so the framework will automaticvally use it as primary key
    // alternatively we can manually specify primary using annotations, that is adding a line [key] above a property

    //[Required] // Marks User name and password are mandatory, and can't be blank
    // Another approach is apply check at DTO level, check RegisterDto
    public string UserName {get; set;}

    public byte[] PasswordHash {get; set;}
    // this is a byte array

    public byte[] PasswordSalt {get; set;}
    // To accomodate the above 2 properties, we need to run following commands in API folder
    // dotnet ed migrations add UserPasswordAdded
    // "UserPasswordAdded" is just a name, we can see the auto generated migration details in the new migration in API => Data => Migrations
    // Then run: "dotnet ef database update" to apply the migration 
    }
}

