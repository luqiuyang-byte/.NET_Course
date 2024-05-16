namespace API.DTOs
{
    public class UserDto // this DTO will be used to return the JWT token
    {
        // only 2 properties needed
        public string Username {get; set;}
        public string Token {get; set;}
    }
}