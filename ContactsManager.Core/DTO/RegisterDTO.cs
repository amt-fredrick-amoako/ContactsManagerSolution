using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name shouldn't be blank")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email shouldn't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in the proper email format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = ("Email is already taken"))]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone shouldn't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers are required for the phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password shouldn't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password shouldn't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
