using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces
{
    public interface IStockGetFromLocal
    {
        void getSTK();
        void STK_Single_Record_Update(int stk_id);
        void STK_Update();
    }
}
