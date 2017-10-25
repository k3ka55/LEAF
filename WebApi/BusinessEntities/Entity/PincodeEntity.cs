using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PincodeEntity
    {
        public class PinTable
        {
            public string City { get; set; }
            public string State { get; set; }
        }
        public class PinModule
        {
            public IEnumerable<PinTable> Address { get; set; }
            public HttpStatusCode StatusCode { get; set; }
        }
    }
}
