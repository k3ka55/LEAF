using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
   public interface IDataSyuncServices
    {
       void STIGet();
       void STNSet();
       void STI_Update_Local();

       void STISet();
       void STNGet();
       void STN_Update();
    }
}
