using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;

namespace Chesslab.Service
{
    public class ProfileViewModelBuilder
    {
        private readonly LocalStorageService _localStorageService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfileViewModelBuilder(LocalStorageService localStorageService, IWebHostEnvironment webHostEnvironment)
        {
            _localStorageService = localStorageService;
            _webHostEnvironment = webHostEnvironment;
        }

        public  async Task<ProfileViewModel> Build(User user )
        {
            return await MapToProfileViewModel(user);
        }

        public  async Task<ProfileViewModel> MapToProfileViewModel(User user)
        {
            
            ProfileViewModel profileViewModel = new ProfileViewModel()
            {
                
                //avatar set here after download method realization and find out the approach of rendering user avatar
                About = await _localStorageService.DownloadAbout(user),
                Level = user.Level,
                Location = user.Location,
                NickName = user.NickName,
                Status = user.Status,
                TaskRating = user.TaskRating,
                AvatarLink = user.Avatar
                
            };
            
            return profileViewModel;
        }


        
    }
}
