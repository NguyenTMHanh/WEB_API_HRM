using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Common.DAL;
using System.Linq.Expressions;
using HRM.Common.Rsp;
using HRM.BLL;


namespace HRM.Common.BLL
{
    public class GenericSvc<D, T> : IGenericSvc<T> where T : class where D : IGenericRep<T>, new()
    {
        public virtual SigleRsp Create(T m)
        {
            var res = new SigleRsp();
            try
            {
                var now = DateTime.Now;
                _rep.Create(m);
            }
            catch(Exception ex)
            {
                res.SetError(ex.StackTrace);
            }
            return res;
        }
        public virtual MultipleRsp Create(List<T> l) 
        {
            var res = new MultipleRsp();
            try
            {
                _rep.Create(l);
            }
            catch (Exception ex)
            {
                res.SetError(ex.StackTrace);
            }
            return res;
        }
        public IQueryable<T> Read(Expression<Func<T, bool>> p) 
        {
            return _rep.Read(p);
        }
        public virtual SigleRsp Read(int id)
        {
            return null;
        }
        public virtual SigleRsp Read(string code)
        {
            return null;
        }
        public virtual SigleRsp Update(T m)
        {
            var res = new SigleRsp();
            try
            {
                _rep.Update(m);
            }
            catch(Exception ex)
            {
                res.SetError(ex.StackTrace);
            }
            return res;
        }
        public virtual MultipleRsp Update(List<T> l)
        {
            var res = new MultipleRsp();
            try
            {
                _rep.Update(l);
            }
            catch (Exception ex) 
            {
                res.SetError(ex.StackTrace);
            }
            return res;
        }
        public virtual SigleRsp Delete(int id)
        {
            return null;
        }
        public virtual SigleRsp Delete(string code)
        {
            return null;
        }
        public virtual SigleRsp Restore(int id)
        {
            return null;
        }
        public virtual int Remove(int id)
        {
            return 0;
        }
        public IQueryable<T> All
        {
            get
            {
                return _rep.All;
            }
        }
        public GenericSvc()
        {
            _rep = new D();
        }
        protected D _rep;
    }
}
