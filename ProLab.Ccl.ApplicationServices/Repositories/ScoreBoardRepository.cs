using ProLab.Ccl.Domain.Models;
using ProLab.Ccl.DomainServices.RepositoryInterfaces;
using ProLab.Ccl.Persistence.Models;
using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Core.Caching;
using ProLab.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices
{
    public class ScoreBoardRepository : IScoreBoardRepository
    {
        private readonly IRepository<ScoreBoardDetailModel> _scoreBoardDetailRepository;
        public ScoreBoardRepository(IRepository<ScoreBoardDetailModel> scoreBoardDetailRepository)
        {
            _scoreBoardDetailRepository = scoreBoardDetailRepository;
        }
        public async Task<ICollection<ScoreBoardDetail>> GetDetailsByIdentityId(int identityId)
        {
            var details = await _scoreBoardDetailRepository.GetAllAsync(query =>
            {
                return from p in query
                       orderby p.IdentityId, p.Id
                       select p;
            });

            return details.Select(_ => new ScoreBoardDetail
            {
                Id = _.Id,
                IdentityId = _.IdentityId,
                Value = _.Value
            }).ToList();
        }
        public async Task UpdateScoreBoardDetail(int id = 51)
        {

            var transaction = await _scoreBoardDetailRepository.BeginTransactionAsync();

            var scoreBoardDetail = await _scoreBoardDetailRepository.GetByIdAsync(id);
            scoreBoardDetail.Value = 410000;
            scoreBoardDetail.EntryDate = DateTime.Now;
            await _scoreBoardDetailRepository.UpdateAsync(scoreBoardDetail);
            
            
            
            await transaction.CommitAsync();
        }

        public Task Insert(int identityId)
        {
            throw new NotImplementedException();
        }
    }
}
