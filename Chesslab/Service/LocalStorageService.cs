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

        public async Task UploadAvatar(IFormFile image, ClaimsPrincipal claimUser, string fileExtension )
        {
            //avatar upload realization
            try
            {
                var currentUser = await _userManager.GetUserAsync(claimUser);
                string relativePath = _storageConfiguration.UserInfo + AvatarPart;
                string fileName = currentUser.Id + "." + fileExtension;

                var fileStream = new FileStream(Environment.CurrentDirectory + relativePath + fileName, FileMode.Create);
                await image.CopyToAsync(fileStream); 
                currentUser.Avatar = relativePath + fileName;
                await _userManager.UpdateAsync(currentUser);
                

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        //will do in the future
        public async Task DownloadAvatar(ClaimsPrincipal claimUser)
        {

        }

        public async Task UploadAbout(string textAbout, ClaimsPrincipal claimUser)
        {
            try
            {
                Console.WriteLine(textAbout);
                var currentUser = await _userManager.GetUserAsync(claimUser);
                string relativePath = _storageConfiguration.UserInfo + AboutPart;
                string fileName = currentUser.Id + ".txt";
                using (var fileStream =
                    new FileStream(Environment.CurrentDirectory + relativePath + fileName, FileMode.Create))
                {
                    byte[] array = Encoding.Default.GetBytes(textAbout);
                    currentUser.About = relativePath + fileName;
                    await fileStream.WriteAsync(array, 0, array.Length);
                }
                await _userManager.UpdateAsync(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string> DownloadAbout(User user)
        {
            try
            {
                using (FileStream fstream = File.OpenRead(Environment.CurrentDirectory + user.About))
                {

                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    string textFromFile = Encoding.Default.GetString(array);
                    return textFromFile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
