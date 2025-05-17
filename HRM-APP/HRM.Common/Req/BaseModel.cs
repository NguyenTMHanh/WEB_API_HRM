using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Req
{
    public class BaseModel
    {
        public BaseModel(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
        public Enum Status { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get;set; }
    }
}
