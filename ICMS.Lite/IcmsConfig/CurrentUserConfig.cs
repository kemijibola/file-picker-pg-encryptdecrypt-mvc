using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.IcmsConfig
{
    public class CurrentUserConfig
    {
        private readonly ICoreRepository _coreRepository;
        private UserViewModel _userViewModel;


        public CurrentUserConfig(ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
            _userViewModel = new UserViewModel();
        }

        public UserViewModel GetCurrentUser()
        {
            if (!string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name) && Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                if (_userViewModel == null || string.IsNullOrEmpty(_userViewModel.USER_ID) || string.IsNullOrEmpty(_userViewModel.USER_ROLE))
                {
                    try
                    {
                        _userViewModel = Task.Run(async () => await _coreRepository.GETUSERBYUSERID(Thread.CurrentPrincipal.Identity.Name)).Result;
                    }
                    catch (System.Exception ex)
                    {
                        //Task.Run(async () => await ExceptionRefiner.LogError(ex));
                    }
                }
            }

            return _userViewModel;
        }

    }
}