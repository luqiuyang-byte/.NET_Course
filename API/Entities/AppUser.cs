namespace API.Entities;

public class AppUser
{
    // These are  properties, they represent columns in the DB
    public int Id {get; set;}    
    //this property liturally named "ID" so the framework will automaticvally use it as primary key
    // alternatively we can manually specify primary using annotations, that is adding a line [key] above a property
    public string UserName {get; set;}
}
