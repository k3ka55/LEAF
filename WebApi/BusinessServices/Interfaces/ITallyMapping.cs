using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface ITallyMapping
    {
        TallyMappingEntity GetTallyMappingById(int skuId);
        // List<TallyMappingEntity> GetSKUByCategory(string skuCat);
        IEnumerable<TallyMappingEntity> GetAllTallyMapping(int? roleId, string Url);
        bool CreateTallyMapping(TallyMappingEntity skuEntity);
        bool UpdateTallyMapping(int skuId, TallyMappingEntity skuEntity);
        bool DeleteTallyMapping(int skuId);

    }
}