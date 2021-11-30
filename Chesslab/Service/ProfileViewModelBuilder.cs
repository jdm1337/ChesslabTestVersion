using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chesslab.Models;
using Chesslab.ViewModels;

namespace Chesslab.Service
{
    public class ProfileViewModelBuilder
    {
        private readonly LocalStorageService _localStorageService;
        public ProfileViewModelBuilder(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
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
                TaskRating = user.TaskRating
            };
            return profileViewModel;
        }
    }
}
