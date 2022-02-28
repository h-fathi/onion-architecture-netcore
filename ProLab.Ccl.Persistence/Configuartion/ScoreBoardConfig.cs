using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProLab.Ccl.Persistence.Models;

namespace ProLab.Ccl.Persistence.Configuration
{

    public class ScoreBoardConfig
    {
        public ScoreBoardConfig(EntityTypeBuilder<ScoreBoardModel> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
        }
    }

}
