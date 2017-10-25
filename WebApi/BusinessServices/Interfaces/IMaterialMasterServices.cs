using BusinessEntities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IMaterialMasterServices
    {
        Material_MasterEntity get(string id);
        bool createMaterial(MaterialList materialEntity);
        bool updateMaterialMaster(int id, Material_MasterEntity materialEntity);
        bool deleteMaterial(string id, string reason);
        List<Material_MasterEntity> searchMatrerial(int? roleId, int reagionid, int locationid, int dcid, int skuTypeid, string Url);
        List<Material_MasterEntity> searchMatrerial(int reagionid, int locationid, int dcid, int skuTypeid);
        List<MIExcelFields> ExcelImportFromMI(fileImportMI fileDetail);
        List<Material_MasterEntity> CheckMaterialMaster(MaterialList material);
    }
}
