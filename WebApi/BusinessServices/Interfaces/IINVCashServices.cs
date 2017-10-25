using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface  IINVCashServices
    {
        List<INVCashEntity> Search(DateTime? From, DateTime? To, string DC_Code);
       
    }
}
