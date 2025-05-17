using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Req
{
    public abstract class BaseReq<T>
    {
        public BaseReq()
        {
            Keyword = string.Empty;
        }
        public BaseReq(int id)
        {
            Id = id;
        }
        public BaseReq(string keyword)
        {
            Keyword = keyword;
        }
        public abstract T ToModel(int? CreateBy = null);
        public int Id { get; set; }
        public string Keyword { get; set; }

    }
}
