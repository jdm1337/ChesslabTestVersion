using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Channels;
using System.Threading.Tasks;
using Chesslab.Dao;
using Chesslab.Models;
using Chesslab.Service;
using Chesslab.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParseApi.Services;

namespace Chesslab.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IMessageEmailService _messageEmailService;
        private const string role = "user";

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMessageEmailService emailService,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageEmailService = emailService;
            _userRepository = userRepository;

        }

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterViewModel model)
        {


            var _userByName = await _userRepository.GetByName(model.NickName);
            var _userByEmail = await _userRepository.GetByEmail(model.Email);


            if (ModelState.IsValid)
            {
                if (_userByEmail == null)
                {
                    if (_userByName == null)
                    {

                        User user = new User {Email = model.Email, UserName = model.Email, NickName = model.NickName};


                        //StringBuilder sb = new StringBuilder();
                        var result = await _userManager.CreateAsync(user, model.Password);
                        //var errors = result.Errors;
                        //foreach (var error in errors)
                        // {
                        //     sb.Append(error.Code + " " + error.Description);
                        // }

                        // return Content(sb.ToString());

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

                            Console.WriteLine("result.succeseed=false");
                            return View(model);

                        }


                    }

                    else
                    {
                        Console.WriteLine("Пользователь существует с ником таким");
                        //"Пользователь с таким ником уже существует"
                        return Content("Пользователь с таким ником уже существует");
                    }
                }
                else
                {
                    Console.WriteLine("Пользователь существует с email м");
                    //"Пользователь с таким email уже существует"
                    return Content("Пользователь с такими  уже существует");
                }
            }
            else
            {
                Console.WriteLine("хуита");
                //"Некорректные данные"
                return View();
            }
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

