using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface ISupplierServices
    {
        List<SupplierEntity> GetsupplierById(int supplierId);
        List<SupplierEntity> GetAllSupplier(int? roleId, string Url);
        bool CreateSupplier(BusinessEntities.SupplierEntity supplierEntity);
        bool UpdateSupplier(int supplierId, SupplierEntity supplierEntity);
        bool DeleteSupplier(int supplierId);
        //DCSupplier_Mapping getSupplierDCinfo(string dcCode);
        List<SupplierEntity> searchSupplier(string location);
    }
}
