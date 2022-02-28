using ProLab.Infrastructure.Core.ExceptionHandling;
using System;
using System.Collections.Generic;

namespace ProLab.Infrastructure.Core
{
    public record ApiResponse
    {
        private string _message { get; set; }
        public ApiResponse()
        {
            Errors = new List<ErrorMessage>();
        }
        public long TotalRecords { get; set; }
        public ICollection<ErrorMessage> Errors { get; set; }
        public bool Success { get; set; }       
        public string Message
        {
            set { _message = value; }
            get
            {
                if (string.IsNullOrEmpty(_message) && Success)
                {
                    return "عملیات با موفقیت انجام شد";
                }
                return _message;
            }
        }
    }
    public record ApiResponse<T>: ApiResponse
    { 
        public T Result { get; set; }
    }
}
