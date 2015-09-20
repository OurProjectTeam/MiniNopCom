using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Configuration
{
    public class CustomerInviteCode : BaseEntity
    {
        public int CustomerId { get; set; }

        public string Code { get; set; }

        public bool IsUsed { get; set; }
    }

    public enum CodeType
    {
        Salty = 0,
        Guid = 1
    }
}
