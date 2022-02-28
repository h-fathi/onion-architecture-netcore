using ProLab.Infrastructure.Core.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices
{
    public class DomainExceptions: DomainExceptionManager, IDomainExceptionManager
    {

        public DomainExceptions()
        {
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-100", Message = "Body Can Not Be Empty", FaMessage = "متن نمی تواند خالی باشد." });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-101", Message = "Title Can Not Be Empty", FaMessage = "عنوان نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-102", Message = "Password Can Not Be Empty", FaMessage = "مقدار امتیاز نمی تواند خالی باشد." });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-103", Message = "Password Can Not Be Empty", FaMessage = "تعداد نمی تواند خالی باشد." });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-104", Message = "Password Can Not Be Empty", FaMessage = "عنوان انگلیسی نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-105", Message = "Password Can Not Be Empty", FaMessage = "عنوان نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-106", Message = "Password Can Not Be Empty", FaMessage = "مقدار امتیاز نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-107", Message = "Password Can Not Be Empty", FaMessage = "عنوان نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-108", Message = "Password Can Not Be Empty", FaMessage = "حداکثر نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-109", Message = "Password Can Not Be Empty", FaMessage = "حداقل نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-110", Message = "Password Can Not Be Empty", FaMessage = "عنوان انگلیسی نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-111", Message = "Password Can Not Be Empty", FaMessage = "عنوان نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-112", Message = "Password Can Not Be Empty", FaMessage = "مقدار نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-113", Message = "Password Can Not Be Empty", FaMessage = "امتیاز نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-114", Message = "Password Can Not Be Empty", FaMessage = "باقی مانده نمی تواند خالی باشد" });
            ErrorMessages.Add(new ErrorMessage() { ErrorCode = "Ccl-115", Message = "Password Can Not Be Empty", FaMessage = "امتیاز استفاده شده نمی تواند خالی باشد" });

        }
    }
}
