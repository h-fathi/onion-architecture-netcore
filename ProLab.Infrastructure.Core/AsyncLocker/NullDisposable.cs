#if TRY_LOCK_OUT_BOOL
using System;
using System.Collections.Generic;
using System.Text;

namespace ProLab.Infrastructure.Core.Locker
{
    sealed class NullDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
#endif
