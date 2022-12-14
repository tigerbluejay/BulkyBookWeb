using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // when we use ICategoryRepository the model will be Category
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
