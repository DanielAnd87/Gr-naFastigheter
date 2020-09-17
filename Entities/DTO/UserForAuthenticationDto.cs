using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTO
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public override string ToString()
        {
            return "Username=" + this.Username + "&Password=" + this.Password;
        }
    }
}
