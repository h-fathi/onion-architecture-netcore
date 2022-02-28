using System;
using System.Collections.Generic;

namespace ProLab.Ccl.Domain.Models
{
    public class ScoreBoard 
    {

        public ScoreBoard()
        {
            Details = new List<ScoreBoardDetail>();
        }

        public int Id { get; set; }

        public int IdentityId { get; set; }

        /// <summary>
        /// سطح کاربری
        /// </summary>
        public int? GradeId { get; set; }

        /// <summary>
        /// /امتیاز اولیه غیر قابل برداشت
        /// </summary>
        public int? InitScore { get; set; }

        /// <summary>
        /// /مجموع امتیازات کسب شده تاکنون
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// مجموع امتیازات استفاده شده تاکنون
        /// </summary>
        public int ScoreUsed { get; set; }

        /// <summary>
        /// باقیمانده امتیازات
        /// </summary>
        public int ScoreRemaining { get; set; }

        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// اقلام تراکنش
        /// </summary>
        public ICollection<ScoreBoardDetail> Details { get; set; }
    }
}
