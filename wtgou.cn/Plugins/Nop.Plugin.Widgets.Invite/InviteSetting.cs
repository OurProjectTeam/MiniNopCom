using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.Invite
{
    public class InviteSetting : ISettings
    {
        public CodeType CodeType { get; set; }

        public int MaxCodeLimit { get; set; }
    }

    public enum CodeType
    {
        Salty = 0,
        Guid = 1
    }

}
