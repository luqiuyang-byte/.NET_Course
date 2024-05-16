using System.Reflection.Metadata.Ecma335;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace API.Controllers;
// Below are controllers
// Every controller will need the [ApiController] attribute
// Every controller need route
// Every controller need to derive from the ControllerBase class. (after change now it derives from BaseApiController class)
// However you can write your own base class as well, that's BaseApiController.cs we added into Controllers folder
[ApiController]
[Route("api/[controller]")] // to access this controller: https://localhost:5001/api/users where "users" is the first part of the class
[Authorize] // User can access these endpoints only if they are authorised
// There are other declarations like [AllowAnonymous] that enables anyone to access a endpoint, this overrides any other [Authorize] reqirement. So should only be used at low level
public class UsersController : BaseApiController

{

    private readonly DataContext _context;

    //this is a constructor
    public UsersController(DataContext context)
    {
        _context = context;
        //because we added the .editorconfig file, this formatting is automatically applied
        //in normal case, we would name our private field "context", then the above line should be "this.context = context"
    }

    // these are http end points
    // This is a simple single-threaded synchoronized request where one quert has to be completed before the next request can come in. We need to re-structure (Asynchronize) it so that it can do multi-threads at the same time.
    /*
    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        var users = _context.Users.ToList();

        return users;
        // the framework wil create correct http responses for us
    }

    
    [HttpGet("{id}")] // .../api/users/# any number regarded as id
    public ActionResult<AppUser> GetUser(int id)
    {
        var user = _context.Users.Find(id);

        return user;
    }
    */

    //This is asynchronized version of the endpointe above
    //what's different: 1. key word async
    //2. return a task instead of a action result
    //3. used await keyword
    //4. used async version of function in entity framework "ToListAsync()" and "FindAsync()"

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users;
        // the framework wil create correct http responses for us
    }

    
    [HttpGet("{id}")] // .../api/users/# any number regarded as id
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        return user;
    }
}
