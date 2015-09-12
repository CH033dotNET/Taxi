using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace BAL.UsersRolesHellpers
{
    public static class RoleHelper
    {
        /// <summary>
        /// Check is current role in Allow list of roles
        /// </summary>
        /// <param name="currentUserRole"> current role</param>
        /// <param name="roles">allowed roles </param>
        /// <returns></returns>
        public static bool RoleControll(this Role currentUserRole,string roles)
        {
            if (!String.IsNullOrEmpty(currentUserRole.Name) &&
                roles.ToUpper().IndexOf(currentUserRole.Name.ToUpper()) != -1
                )
                return true;
        
            return false;
        }
    }
}
