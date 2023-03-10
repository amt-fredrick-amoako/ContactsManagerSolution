using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [Route("[action]")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            //Look for validation errors
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
                return View(register);
            }

            //Create an object of the ApplicationUser and populate the properties with the DTO's
            ApplicationUser user = new ApplicationUser()
            {
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                UserName = register.Email,
                PersonName = register.PersonName
            };

            //Invoke the UserManager.CreateAsync() with the user instance, this calls methods in the repository layer. and return result to the IdentityResult type
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);

            //redirect to the Index action if successful
            if (result.Succeeded)
            {
                //Sign in
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }

            //loop through errors
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    //add errors to the modelstate, the same will be sent to the view as part of the modelstate object, message will be displayed in the validation summary
                    ModelState.AddModelError("Register", error.Description);
                }

                return View(register);
            }

        }
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
                return View(login);
            }
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return LocalRedirect(ReturnUrl);

                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }

            ModelState.AddModelError("Login", "Invalid email or password");

            return View();
        }
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        [Route("[action]")]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return Json(true); //valid
            }
            else
            {
                return Json(false); //invalid
            }
        }
    }
}
