using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Req
{
    public class SimpleReq : BaseReq<BaseModel>
    {
        public override BaseModel ToModel(int? CreateBy = null)
        {
            return new BaseModel(Id);
        }
        public SimpleReq() : base() { }
        public SimpleReq(int id) : base(id) { }
        public SimpleReq(string keyword) : base(keyword) { }
    }
}
