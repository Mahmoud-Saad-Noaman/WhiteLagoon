using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModel;

namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Helper Method
        private readonly UserManager<ApplicationUser> _userManager; // uesd to manage the user  
        private readonly SignInManager<ApplicationUser> _signInManager;   // Expected the user type
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl = null)
        {
            returnUrl??= Url.Content("~/");  // ?? -> If returnUrl is null, it will be assigned the value Url.Content("~/") .

            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl                  
            };
            return View(loginVM);
        }         
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");


            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                RedirectUrl = returnUrl
            };

            return View(registerVM);
        
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            ApplicationUser user = new()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
                NormalizedEmail = registerVM.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = registerVM.Email,
                CreatedAt = DateTime.Now,
            };

            // Create user in DB in ASPNetUsers table
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            // registration is Succesfull
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVM.Role))
                {
                    // Assign that role
                    await _userManager.AddToRoleAsync(user, registerVM.Role);
                }
                else
                {
                    // Assign role of customer
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }
            

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(registerVM.RedirectUrl);
                }
            }
            // if the result is not Succesfull
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            registerVM.RoleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            });
            

            return View(registerVM);

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            // Check form the DB  the userId and Pass Combination is valid or not
            if ((ModelState.IsValid))
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                
                // registration is Succesfull "UserId And Pass Combination was valid"
                if (result.Succeeded)
                {
                    // Retrieve The user object
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return LocalRedirect(loginVM.RedirectUrl);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }

            return View(loginVM);
        }
    }
}
