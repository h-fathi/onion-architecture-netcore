using ProLab.Ccl.Domain.Enums;
using System;

namespace ProLab.Ccl.Domain.Models
{
    /// <summary>
    /// ریزتراکنش امتیازات کسب شده و استفاده شده
    /// </summary>
    public class ScoreBoardDetail 
    {
        public int Id { get; set; }

        public int IdentityId { get; set; }

        /// <summary>
        /// مقدار امتیاز
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// نوع جزییات تراکنش
        /// </summary>
        public ScoreBoardDetailTypeEnum ScoreBoardDetailTypeEnum { get; set; }
        
        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public DateTime EntryDate { get; set; }

    }
}
