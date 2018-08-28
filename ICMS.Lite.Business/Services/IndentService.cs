using ICMS.Lite.Repository.Repositories;
using ICMS.Lite.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ICMS.Lite.Repository.Utilities.General;

namespace ICMS.Lite.Business.Services
{
    public class IndentService : IIndentService
    {
        public IndentService(IIndentsRepository indentRepository)
        {
            _indentRepository = indentRepository;
        }
        private IIndentsRepository _indentRepository;

        public async Task<DataResult<IndentsToGenerate>> GENERATEDINDENTSANDENCRYPT(string userId)
        {
            var objIndents = await _indentRepository.GENERATEDINDENTSANDENCRYPT(userId);
            return objIndents;   
        }

        public async Task<DataResult<DecryptViewModel>> DECRYPTINDENTS(DecryptViewModel model)
        {
            var objIndents = await _indentRepository.DECRYPTINDENTS(model);
            return objIndents;
        }

        public async Task<DataResult<List<AvailableIndentsViewModel>>> GETINDENTSFORENCRYPTION()
        {
            var objIndents = await _indentRepository.GETINDENTSFORENCRYPTION();
            return objIndents;
        }
    }
}
