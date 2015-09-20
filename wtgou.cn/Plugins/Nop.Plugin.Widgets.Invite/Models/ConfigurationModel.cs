using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Configuration;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.Invite.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        public CodeType CodeType { get; set; }

        public int MaxCodeLimit { get; set; }
    }
    
}
