using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ResponseEntity
    {
        public string csi_Number { get; set; }
        public string Dispatch_Number { get; set; }
        public int Dispatch_id { get; set; }
        public string po_Number { get; set; }
        public string grn_Number { get; set; }
        public string ws_Number { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public string sti_Number { get; set; }
        public int materialID { get; set; }
        public int invoice_Id { get; set; }
        public int TempId { get; set; }
        public int MappId { get; set; }
        public int MenuId { get; set; }
    }
}
