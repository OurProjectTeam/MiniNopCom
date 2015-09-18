using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.Invite.Controllers
{
    public class WidgetsInviteController:BasePluginController
    {
        public WidgetsInviteController()
        {
            
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            return View();
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(int i)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View();
        }

    }
}
