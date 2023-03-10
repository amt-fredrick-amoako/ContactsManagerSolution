using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email or User name shouldn't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in the proper format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
