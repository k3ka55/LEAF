using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IDashBoardServices
    {
        DashBoardEntity GetDCDashBoard(string DC_Code);

        List<DCDASHBOARD_CSISTIFORGRAPHEntity> GetDCDASHBOARD_CSISTIFORGRAPH(string DC_Code);
    }
}
