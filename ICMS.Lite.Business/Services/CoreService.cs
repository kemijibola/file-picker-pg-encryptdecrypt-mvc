using ICMS.Lite.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Business.Services
{
    public class CoreService : ICoreService
    {
        public CoreService(ICoreRepository coreRepository)
        {
            _coreRepository = coreRepository;
        }
        private ICoreRepository _coreRepository;

        public async Task<UserViewModel> GETUSERBYUSERID(string userId)
        {
            var coreRepo = await _coreRepository.GETUSERBYUSERID(userId);
            return coreRepo;
        }
    }
}
