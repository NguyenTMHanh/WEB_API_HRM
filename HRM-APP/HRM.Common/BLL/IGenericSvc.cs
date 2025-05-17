using HRM.Common.Rsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRM.BLL
{
    public interface IGenericSvc<T> where T : class
    {
        SigleRsp Create(T m);
        MultipleRsp Create(List<T> l);
        IQueryable<T> Read(Expression<Func<T, bool>> p);
        SigleRsp Read(string code);
        SigleRsp Update(T m);
        MultipleRsp Update(List<T> l);
        SigleRsp Delete(int id);
        SigleRsp Delete(string code);
        SigleRsp Restore(int id);
        int Remove(int id);
        IQueryable<T> All { get; }
    }
}
