using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{   
    public class TallyMappingEntity
    {
        public int ID { get; set; }
        public string Activity { get; set; }
        public string Module_Name { get; set; }
        public string VOUCHER_TYPE { get; set; }
        public string LEDGER_CODE { get; set; }
        public string LEDGER_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public string COST_CENTRE_NAME { get; set; }
        public int? Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int is_Create { get; set; }
        public int is_Edit { get; set; }
        public int is_Approval { get; set; }
        public int is_View { get; set; }
        public int is_Delete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> Is_Delete { get; set; }
               
    }

}