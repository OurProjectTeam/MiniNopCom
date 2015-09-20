using System.Collections.Generic;
using Nop.Core.Domain.Configuration;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.Invite.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public List<CustomerInviteCode> List { get; set; } 
    }

}
