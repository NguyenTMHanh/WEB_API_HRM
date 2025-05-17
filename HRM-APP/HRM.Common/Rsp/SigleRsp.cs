using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Rsp
{
    public class SigleRsp : BaseRsp
    {
        public SigleRsp() : base() { }
        public SigleRsp(string message) : base(message) { }
        public SigleRsp(string message, string titleError) : base(message, titleError) { }
        public void SetData(string code, object data)
        {
            code = code;
            data = data;
        }
        public object Data { get; set; }

    }
}
