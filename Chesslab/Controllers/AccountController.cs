using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Threading.Channels;
using System.Threading.Tasks;
using Chesslab.Configurations;
using Chesslab.Dao;
using Chesslab.Models;
using Chesslab.Service;
using Chesslab.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParseApi.Services;

namespace Chesslab.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IMessageEmailService _messageEmailService;
        private readonly LocalStorageService _localStorageService;
        private readonly ProfileViewModelBuilder _profileViewModelBuilder;
        private const string role = "user";
        private string[] supportedTypes = new[] { "jpg", "jpeg", "png", "bmp" };


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMessageEmailService emailService, StorageConfiguration storageConfiguration,
            IUserRepository userRepository, LocalStorageService localStorageService, ProfileViewModelBuilder profileViewModelBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageEmailService = emailService;
            _userRepository = userRepository;
            _localStorageService = localStorageService;
            _profileViewModelBuilder = profileViewModelBuilder;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterViewModel model)
        {
            var _userByName = await _userRepository.GetByName(model.NickName);
            var _userByEmail = await _userRepository.GetByEmail(model.Email);

            // model validation from register view model
            if (ModelState.IsValid)
            {
                if (_userByEmail == null)
                {
                    if (_userByName == null)
                    {
                        User user = new User {Email = model.Email, UserName = model.Email, NickName = model.NickName, RegisterDate = DateTime.Today, Location = "Earth"};

                        var result = await _userManager.CreateAsync(user, model.Password);
                        

                        if (result.Succeeded)
                        {
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                new {userId = user.Id, code = code},
                                protocol: HttpContext.Request.Scheme);
                            EmailService emailService = new EmailService();
                            await _messageEmailService.SendMessage(model.Email, "Подтверждение аккаунта",
                                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                            IdentityRole getRoleId = await _userRepository.GetRole(role);

                            var userRole = new IdentityUserRole<string>()
                                {UserId = user.Id, RoleId = getRoleId.Id};

                            await _userRepository.AddRole(userRole);
                            await _userRepository.Save();

                            return RedirectToAction("Index", "Home");
                        }

                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(String.Empty, error.Description);
                            }

                            
                            return View(model);

                        }


                    }

                    else
                    {
                        
                        //"Пользователь с таким ником уже существует"
                        return Content("Пользователь с таким ником уже существует");
                    }
                }
                else
                {
                    
                    //"Пользователь с таким email уже существует"
                    return Content("Пользователь с такими  уже существует");
                }
            }
            else
            {
                //"Некорректные данные"
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByEmail(model.Email);
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Пользователь неподтверждён");
                    return View(model);
                }

                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                
                if (result.Succeeded)
                {
                    
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {

                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            ProfileViewModel profileViewModel = await _profileViewModelBuilder.Build(user);
            return View(profileViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var editViewModel = new EditViewModel
                {NickName = user.NickName, About = await _localStorageService.DownloadAbout(user), Location = user.Location, UserView = user};
            
            return View(editViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(4194304)]
        public async Task<IActionResult> Edit(EditViewModel editViewModel)
        {
            
            var user = await _userManager.GetUserAsync(User);

            if (editViewModel.NickName != null)
            {
                var userByName = await _userRepository.GetByName(editViewModel.NickName);
                
                if(userByName == null)
                {
                     await _userRepository.EditNickName(user, editViewModel.NickName);
                     await _userRepository.Save();
                }
                else
                {
                    ModelState.AddModelError("", "пользователь с таким именем уже существует");
                    return View(editViewModel);
                }
            }

            if(editViewModel.Location!= null)
            {
                
                await _userRepository.EditLocation(user, editViewModel.Location);
                await _userRepository.Save();
            }

            if (editViewModel.UploadedAvatar != null)
            {
                var fileExtension = Path.GetExtension(editViewModel.UploadedAvatar.FileName).Substring(1);
                if (supportedTypes.Contains(fileExtension.ToLower()))
                {
                    await _localStorageService.UploadAvatar(editViewModel.UploadedAvatar, User, fileExtension);
                }
                else
                {
                    ModelState.AddModelError("", "Неподдерживаемый тип изображения");
                    return View(editViewModel);
                }
            }

            if (editViewModel.About != null)
            {
                await _localStorageService.UploadAbout(editViewModel.About, User);
            }
            //realization avatar and about service and finish the proccese of edit user
           
            return RedirectToAction("profile");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Content("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                _signInManager.SignInAsync(user, true);
                return  RedirectToAction("Index", "Home");
            }
            else
            {
                return Content("Почта не подтверждена");
            }

        }

    }

    
}

