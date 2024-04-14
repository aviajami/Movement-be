using System.ComponentModel.DataAnnotations;

namespace Movement_be.AuthorizeHelpers
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}