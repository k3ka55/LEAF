using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public interface ISKUService
    {
        SKUEntity GetskuById(int skuId);
        List<SKUEntity> GetSKUByCategory(string skuCat);
        IEnumerable<SKUEntity> GetAllSku(int? roleId, string Url);
        bool CreateSKU(SKUEntity skuEntity);
        bool UpdateSKU(int skuId, SKUEntity skuEntity);
        bool DeleteSKU(int skuId);
        List<skuReturnMappingEntity> getskuBasemainsub(int skuMappingId);
    }
}