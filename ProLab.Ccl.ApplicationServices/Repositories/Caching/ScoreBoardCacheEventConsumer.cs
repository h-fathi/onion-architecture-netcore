using System.Threading.Tasks;
using ProLab.Ccl.Persistence.Models;
using ProLab.Infrastructure.Core.Caching.Event;

namespace ProLab.Ccl.ApplicationServices.Caching
{
    /// <summary>
    /// Represents a score board cache event consumer
    /// </summary>
    public partial class ScoreBoardCacheEventConsumer : CacheEventConsumer<ScoreBoardDetailModel>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(ScoreBoardDetailModel entity)
        {

            await base.ClearCacheAsync(entity);
        }
    }
}
