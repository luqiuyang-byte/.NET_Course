using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        // this method can be viewed as an agrement, any other class that implement this interface has to support this method
        // and it has to take app user as an argument. and then return a string
        string CreateToken(AppUser user);
    }
}