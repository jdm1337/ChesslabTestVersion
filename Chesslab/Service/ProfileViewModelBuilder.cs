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
        public static async Task<ProfileViewModel> Build(User user )
        {
            return await MapToProfileViewModel(user);
        }

        public static async Task<ProfileViewModel> MapToProfileViewModel(User user)
        {
            ProfileViewModel profileViewModel = new ProfileViewModel()
            {
                About = user.About,
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
