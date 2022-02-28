using ProLab.Infrastructure.Core;
using System;

namespace ProLab.Ccl.Persistence.Models
{
    public class ScoreBoardModel : BaseEntity
    {
        public int IdentityId { get; set; }

        public int? GradeId { get; set; }

        public int? InitScore { get; set; }

        public int Score { get; set; }

        public int ScoreUsed { get; set; }

        public int ScoreRemaining { get; set; }

        public DateTime EntryDate { get; set; }
    }
}
