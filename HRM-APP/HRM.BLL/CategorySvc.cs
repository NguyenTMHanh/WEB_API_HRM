using HRM.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.BLL
{
    public class CategorySvc
    {
        private CategoryRep categoryRep;
        public CategorySvc() 
        { 
            categoryRep = new CategoryRep();
        }
    }
}
