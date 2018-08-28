using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Business.Services
{
    public interface IMFBService
    {
        Task<DataResult<MfbViewModel>> UPLOADMFBEXCEL(HttpPostedFileBase upload,string userId);
    }
}
