using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IRateIndentService
    {
        List<RateTemplateSearchUniqueEntity> CheckUnion(RateIndentEntityUnique rate);
        int CreateRateIndent(RateIndentEntity rateEntity);
        List<RateIndentEntity> GetRateIndent(string id);
        bool DeleteRateIndent(string tId, string deleteReason);
        bool DeleteRateIndentLineItem(int Id);
        int UpdateRateIndent(int tId, RateIndentEntity rateEntity);
        List<RateTempateResponse> searchRateTemplate(string ULocation = "null", string UDCCode = "null", string Region = "null");
        //List<Template_SKU_List> GetTempSKUS(int TemplateID);
        List<Template_SKU_List> getSKUS(int ratetemplateID);
        List<Template_Fields_SKU> GetrateTemplates(int CustomerID, string region, string location, string dccode, string skutype);
        List<RateIndentEntity> searchTemplate(int? roleId, int region_id, string location, string dccode, string Url);
        List<SKUPrice> getPrice(string SKU_Type, string Location, string Grade, string SKU_Name,int tempID);
        List<RateTempateResponse> searchRateTemplateforEdit(string ULocation = "null", string UDCCode = "null", string Region = "null");
        RIExcelImport ExcelImportForri(rifileImport fileDetail);
        ReturnRate GetRateForCsi(RateInformation rateDetail);
    }
}
