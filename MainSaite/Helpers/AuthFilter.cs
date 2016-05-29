using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MainSaite.Helpers
{
    public class AuthFilterAttribute : AuthorizeAttribute {

		private string[] allowedUsers = new string[] { };

		// this ugly long string is just converting enum data to string[]
		private string[] allowedRoles = Enum.GetValues(typeof(Common.Enum.AvailableRoles)).OfType<object>().Select(o => o.ToString()).ToArray();

		public AuthFilterAttribute() { }

		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			if (!String.IsNullOrEmpty(base.Users)) {
				allowedUsers = base.Users.Split(new char[] { ',' });
				for (int i = 0; i < allowedUsers.Length; i++) {
					allowedUsers[i] = allowedUsers[i].Trim();
				}
			}
			if (!String.IsNullOrEmpty(base.Roles)) {
				allowedRoles = base.Roles.Split(new char[] { ',' });
				for (int i = 0; i < allowedRoles.Length; i++) {
					allowedRoles[i] = allowedRoles[i].Trim();
				}
			}

			//return httpContext.Request.IsAuthenticated &&
			//		User(httpContext) && Role(httpContext);
			return Role(httpContext);
		}

		private bool User(HttpContextBase httpContext) {
			if (allowedUsers.Length > 0) {
				return allowedUsers.Contains(httpContext.User.Identity.Name);
			}
			return true;
		}

		private bool Role(HttpContextBase httpContext) {
			if (allowedRoles.Length > 0) {
				try {
					var usr = httpContext.Session["User"] as UserDTO;
					for (int i = 0; i < allowedRoles.Length; i++) {
						if (usr.Role.Name == allowedRoles[i])
							return true;
					}
				} catch {
					return false;
				}
				
			}
			return true;
		}

		
	}
}
