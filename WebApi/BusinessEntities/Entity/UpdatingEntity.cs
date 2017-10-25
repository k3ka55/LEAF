using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class UpdatingEntity
    {
       public List<GRNUpdateIdEntity> GrnUpdateId { get; set; }
       public int id { get; set; }
    }
   public class GRNUpdateIdEntity
   {
       public int id { get; set; }
   }
}
