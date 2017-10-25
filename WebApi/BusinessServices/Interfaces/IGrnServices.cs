using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IGrnServices
    {
        string CreateGrn(GrnEntity Grn_Entity);
        List<GrnEntity> GetGrnLineItem(string id);
        string UpdateGRN(int grnId, GrnEntity Grn_Entity);
        bool DeleteGRN(int grnId);
        //bool POLUpdateById(int id);
        bool POUpdateById(POBULKAPPROVAL grnEntity);
        bool DeleteGRNOrderLineItem(int Id);
        List<poNumber> GetPoNumbers(DateTime? date, string Ulocation);
        List<GrnEntity> SearchGRNCR(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation, string Url = "null");
        List<cdnNumber> GetCdnNumbersforGRN(string Ulocation, string CustomerCode);//
        List<stnNumber> GetStnNumbers(DateTime? date, string Ulocation);
        List<stiNumber> GetStiNumbers(DateTime? date, string Ulocation);
        List<stiNumber> GetStiNumbersforSTN(DateTime? date, string Ulocation);
        //List<GrnEntity> GetGRNAND(DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation);
        //List<GrnEntity> GetGRNOR(DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation);
        List<GrnEntity> SearchGRN(int? roleId, DateTime? startDate, DateTime? endDate, string supplierName, string Ulocation, string Url = "null");
        List<GrnEntity> GetGRN(string Ulocation);
    }
}
