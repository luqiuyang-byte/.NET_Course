
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")] // user access end-point: POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) // this methods registers new user
        {
            // first check if the user name exists
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");


            // now we will hash the password with a hashing algorithm, we will user .NET provided hashing algorithm hmac
            using var hmac = new HMACSHA512();
            // we are using this "using" keyword to register a new instance of class, so that when we finish using it, memory can be released
            // suitable for class that will be used only once
            // how to tell if a method should be disposed after use? Go to parent (of inherit) and check if it uses IDisposble

            var user = new AppUser
            {
                // specifying properties now
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // hashing the user entered password
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user); // this doesn't do anything in database, it only track new entity in memory. Telling Entity framework we want to add user
            await _context.SaveChangesAsync(); // actually saves the change in database
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")] // this method returns whether login is valid, but doesn't mean that user is logged in. Because REST API is stateless, it only returns backend response but doesn't remember a status
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            // The above SingleOrDefaultAsync method returns the only matching result from the query, if no matching it will return default value which is null
            // If there are 2 matching results, then the session will freeze. But that's OK because all user names should be unique

            // Alternatively, use FirstOrDefaultAsync method that returns the first matching result, or default result which is null

            // Another method called "FirstAsync" is not suitable, because if there is no matching result, it will throw session error

            if (user == null) return Unauthorized("Invalid username.");

            // Calculates Hashed password using the user entered password, and the password salt stored in the database for that user
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Now compare the calculated hashed password with the password in database (both items bit-array)
            // loop through each element of the bit-array and compare corresponding elements
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password.");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        // the following method checks whether a user name already exists when registering a new one
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); // return True if user name exists
        }
    }
}