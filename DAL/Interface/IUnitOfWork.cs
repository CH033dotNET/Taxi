using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> UserRepo { get; }
        IGenericRepository<Role> RoleRepo { get; }

        void Dispose();
        void Save();
    }
}
