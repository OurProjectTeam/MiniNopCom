using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;
using Nop.Core.Domain.Configuration;
using Nop.Plugin.Widgets.Invite.Models;

namespace Nop.Plugin.Widgets.Invite
{
    public class InviteSetting : ISettings
    {
        public CodeType CodeType { get; set; }

        public int MaxCodeLimit { get; set; }
    }

}
