using ProLab.Ccl.Domain.Models;
using ProLab.Ccl.Domain.ServicesInterface;
using ProLab.Ccl.DomainServices.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProLab.Ccl.DomainServices
{
    public class ScoreBoardService : IScoreBoardService
    {

        private readonly IScoreBoardRepository _scoreBoardRepository;

        public ScoreBoardService(IScoreBoardRepository scoreBoardRepository)
        {
            _scoreBoardRepository = scoreBoardRepository;
        }


        #region Methods

        /// <summary>
        ///  get sum of scoreBoard details values
        /// </summary>
        /// <param name="scoreBoardDetails"></param>
        /// <returns>sum of value as int</returns>
        public async Task<int> GetSummationDetailsByIdentity(int identityId)
        {
            await _scoreBoardRepository.UpdateScoreBoardDetail();
            var scoreBoardDetails = await _scoreBoardRepository.GetDetailsByIdentityId(identityId);

            return scoreBoardDetails.Sum(x => x.Value);
        }

        public async Task<int> InsertAsync(int identityId)
        {
            await _scoreBoardRepository.Insert(identityId);
            throw new System.NotImplementedException();
        }


        #endregion
    }
}
