using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Repository.Repositories
{
    public interface IIndentsRepository
    {
        Task<DataResult<IndentsToGenerate>> GENERATEDINDENTSANDENCRYPT(string userId);
        Task<DataResult<List<AvailableIndentsViewModel>>> GETINDENTSFORENCRYPTION();
        Task<DataResult<DecryptViewModel>> DECRYPTINDENTS(DecryptViewModel model);
        //Task<bool> UPDATEGENERATEDINDENTS(Lis)
    }
}
