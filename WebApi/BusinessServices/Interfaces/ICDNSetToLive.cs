using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface ICDNSetToLive
    {
        void CDNSet();
        void CDNGet();
        void CDN_Update();
    }
}
