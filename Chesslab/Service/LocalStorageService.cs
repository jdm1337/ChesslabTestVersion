using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Chesslab.Configurations;
using Chesslab.Dao;
using Chesslab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Chesslab.Service
{
    public class LocalStorageService
    {
        private readonly UserManager<User> _userManager;
        private readonly StorageConfiguration _storageConfiguration;
        private string AvatarPart = "\\Avatars\\";
        private string AboutPart = "\\AboutUser\\";
        private readonly ApplicationContext _appContext;


        public LocalStorageService(UserManager<User> userManager, StorageConfiguration storageConfiguration, ApplicationContext appContext, IUserRepository userRepository)
        {
            _userManager = userManager;
            _storageConfiguration = storageConfiguration;

        }

        public async Task UploadAvatar(IFormFile image, ClaimsPrincipal claimUser )
        {
            //avatar upload realization
            try
            {
                var currentUser = await _userManager.GetUserAsync(claimUser);
                string userAvatarUploadPath = Environment.CurrentDirectory + _storageConfiguration.UserInfo + AvatarPart;

                var fileStream = new FileStream(userAvatarUploadPath + currentUser.Id + image.FileName, FileMode.Create);
                await image.CopyToAsync(fileStream); 
                currentUser.Avatar = currentUser.Id+image.FileName;
                await _userManager.UpdateAsync(currentUser);
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task DownloadAvatar(ClaimsPrincipal claimUser)
        {

        }

        public async Task UploadAbout(string textAbout, ClaimsPrincipal claimUser)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(claimUser);
                string userAvatarUploadPath = Environment.CurrentDirectory + _storageConfiguration.UserInfo + AboutPart;
                using (var fileStream =
                    new FileStream(userAvatarUploadPath + currentUser.Id + ".txt", FileMode.OpenOrCreate))
                {
                    byte[] array = Encoding.Default.GetBytes(textAbout);
                    currentUser.About = currentUser.Id + ".txt";
                    await fileStream.WriteAsync(array, 0, array.Length);
                }
                await _userManager.UpdateAsync(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
