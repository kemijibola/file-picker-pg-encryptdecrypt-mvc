using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Business.Services
{
    public interface IAccountService
    {
        Task<bool> AUTHENTICATEUSER(LoginViewModel userModel);
    }
}
