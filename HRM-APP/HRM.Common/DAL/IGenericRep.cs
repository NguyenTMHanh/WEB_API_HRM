using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.DAL
{
    public interface IGenericRep<T> where T : class
    {
        #region --method--
        /// <summary>
        /// Create the model
        /// </summary>
        /// <param name="m"></param>
        void Create(T m);

        /// <summary>
        /// Create list model
        /// </summary>
        /// <param name="l"></param>
        void Create(List<T> l);

        /// <summary>
        /// Read by
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IQueryable<T> Read(Expression<Func<T, bool>> p);

        /// <summary>
        /// Read single object 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Read(int id);

        /// <summary>
        /// Read single object
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        T Read(string code);

        /// <summary>
        /// Update the model
        /// </summary>
        /// <param name="m"></param>
        void Update(T m);

        /// <summary>
        /// Update list model
        /// </summary>
        /// <param name="l"></param>
        void Update(List<T> l);
        #endregion

        #region --properties--
        IQueryable<T> All { get; }

        #endregion
    }
}
