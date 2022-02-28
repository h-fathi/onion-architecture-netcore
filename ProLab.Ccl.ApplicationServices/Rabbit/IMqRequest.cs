﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices
{
    public interface IMqRequest
    {
        /// <summary>
        /// Route key for finding queue
        /// </summary>
        string RouteKey { get; set; } 
    }
}
