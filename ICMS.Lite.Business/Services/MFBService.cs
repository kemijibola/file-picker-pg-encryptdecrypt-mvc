using ICMS.Lite.Repository.Repositories;
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
    public class MFBService : IMFBService
    {
        public MFBService(IMFBRepository mfbRepository)
        {
            _mfbRepository = mfbRepository;
        }
        private IMFBRepository _mfbRepository;

        public async Task<DataResult<MfbViewModel>> UPLOADMFBEXCEL(HttpPostedFileBase upload,string userId)
        {
            var objMfb = await _mfbRepository.UPLOADMFBEXCEL(upload,userId);
            return objMfb;
        }
    }
}
