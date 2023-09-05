using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_Vinpearl.Areas.Admin.Controllers
{
    public class PermissionErrorController : Controller
    {
        // GET: Admin/PermissionError
        public ActionResult NotAllowPermission()
        {
            return View();
        }
    }
}