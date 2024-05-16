using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        // 2 properties created
        // they will be automatically binded to the data received from API
        // in order to bind, the property name need to exact match field name, although case don't need to be same as JSON field name is always lower case
        [Required] // Marks that the user name is mandatory and can't be blank
        // we can also add other validators here like max length, etc, also I can use RegularExpression to create my own rules
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}