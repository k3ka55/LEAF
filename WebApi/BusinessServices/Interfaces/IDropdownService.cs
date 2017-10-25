using BusinessEntities;
using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IDropdownService
    {
        IEnumerable<Tally_Module> GetTallyModuleByName(string Tally_Module_Activity);
        IEnumerable<Tally_Activity> GetTallyDD();
        DropdownEntity GetDropdowns();
        List<Vehicle_No> GetVehicleNos();
        List<DCMasterModel> GetDC();
        DCLocationModel GetLocDCCombine();
        CustSuppDDEntity GetCustSuppDD();
        CustRegDDEntity GetCustRegDD();
        IEnumerable<Template_Type> GetTemplateType();
      //  List<DCMasterModel> getDCforSTINJDM(string ULocation);
        List<DCMasterModel> getDCforSTIJDM(string ULocation);
        List<Tuple<string>> getDispatchTypes();
        List<Tuple<string>> getInvoiceTypes();
        List<Tuple<string>> getSkuCategories();
        List<Tuple<string>> getPackTypes();
        //List<Pack_Size> GetPackSize(string packType);
        //PackSizeEntity GetPackSizeA(string packType);
        List<Tuple<string>> getMaterialResource();
        List<string> getLocations();
        List<Tuple<string>> getSkuTypes();
        //IEnumerable<BothSKUEntity> GetSKUs();
      //  dynamic GetSKUs();
        List<Tuple<string, string>> getSTI_Type();
        List<DCMasterModel> getLocation(int userId);
        List<PincodeEntity.PinTable> GetPincode(int id);
        string validateMaterialCode(Material_MasterEntity valma);
        string validateSKUName(SKUEntity valSku);
        string validateUserName(UserDetailsEntity valUsr);
        string validateRoleName(RoleEntity valRl);
        string validateDCName(DCMasterEntity valDC);
        string validateLocationName(LocationMasterEntity valLoc);
        string validateRegionName(RegionMasterEntity valReg);
        string validateRouteMaster(RouteMasterEntity valroute);
    }
}
