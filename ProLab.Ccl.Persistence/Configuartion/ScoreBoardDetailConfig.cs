using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLab.Ccl.Persistence.Models;

namespace ProLab.Ccl.Persistence.Configuration
{
    public class ScoreBoardDetailConfig
    {
        public ScoreBoardDetailConfig(EntityTypeBuilder<ScoreBoardDetailModel> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
        }
    }
}
