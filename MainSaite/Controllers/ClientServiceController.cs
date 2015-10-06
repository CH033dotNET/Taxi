using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainSaite.Controllers
{
    public class ClientServiceController : BaseController
    {
        //
        // GET: /ClientService/

        public ActionResult PeekClient()
        {
            return PartialView(tarifManager.GetTarifes());
        }

    }
}
