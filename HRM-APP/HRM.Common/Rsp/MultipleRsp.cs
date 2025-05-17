using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Rsp
{
    public class MultipleRsp : BaseRsp
    {
        public MultipleRsp() : base() { }
        public MultipleRsp(string msg) : base(msg) { }
        public MultipleRsp(string message, string titleError) : base(message, titleError) { }
        public void SetData(string key, object o)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, object>();
            }
        }
        public void SetFailure(object o, string message)
        {
            var t = new Dto(o, message);
            SetData("failure", t);
        }
        public Dictionary<string, object> Data { get; private set; }
        public class Dto
        {
            public Dto(object data, string message)
            {
                data = data;
                message = message;
            }

            public object Data { get; private set; }
            public string Message { get; private set; }
        }
    }
}
