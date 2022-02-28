namespace ProLab.Ccl.Domain.Enums
{
    /// <summary>
    /// نوع جزییات تراکنش امتیازات
    /// </summary>
    public enum ScoreBoardDetailTypeEnum : short
    {
        /// <summary>
        /// افزایش توسط کارمزد
        /// </summary>
        IncreaseByWage = 1,
        
        /// <summary>
        /// افزایش توسط ادمین
        /// </summary>
        IncreaseByAdmin=2,
        
        /// <summary>
        /// کاهش توسط دریافت پاداش
        /// </summary>
        DecreaseByTakeReward=3,

        /// <summary>
        ///  افزایش به وسیله لاگین در سایت
        /// </summary>
        IncreaseByLogin = 4
    }
}
