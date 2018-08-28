using ICMS.Lite.Repository.Data;
using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Business.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        private IAccountRepository _accountRepository;

        public async Task<bool> AUTHENTICATEUSER(LoginViewModel model)
        {
            var authRepo = await _accountRepository.AUTHENTICATEUSER(model);
            return authRepo;
        }
    }
}
