using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("验证码类型")]
        public CodeType CodeType { get; set; }

        [DisplayName("验证码个数")]
        public int MaxCodeLimit { get; set; }
    }
    
}
