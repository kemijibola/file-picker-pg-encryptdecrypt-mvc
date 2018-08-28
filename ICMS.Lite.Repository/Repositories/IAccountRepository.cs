using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;
using static ICMS.Lite.Repository.ViewModels.AccountViewModel;

namespace ICMS.Lite.Repository.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> AUTHENTICATEUSER(LoginViewModel userModel);
    }
}
