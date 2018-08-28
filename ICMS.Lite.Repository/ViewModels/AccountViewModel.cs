using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Repository.ViewModels
{
    public class AccountViewModel
    {
        public class UserViewModel : Auditable
        {
            public string USER_ID { get; set; }
            public string USER_NAME { get; set; }
            public string USER_ROLE { get; set; }
            public string APPROVED { get; set; }
            public string DELETED { get; set; }
            public string PASSWORD { get; set; }
        }

        public class LoginViewModel
        {
            public LoginViewModel() { }
            public LoginViewModel(ICoreRepository coreRepository)
            {
                _coreRepository = coreRepository;
            }
            private ICoreRepository _coreRepository;
            public string UserId { get; set; }
            public string Password { get; set; }

            public async Task<string> GETCURRENTUSER(string userId)
            {
                var objUser = await _coreRepository.GETUSERBYUSERID(userId);
                if (objUser != null)
                    return objUser.USER_ROLE;
                else
                    return string.Empty;
            }
        }

    }

}
