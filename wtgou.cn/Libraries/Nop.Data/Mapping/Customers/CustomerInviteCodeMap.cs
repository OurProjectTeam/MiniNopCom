using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Configuration;

namespace Nop.Data.Mapping.Customers
{
    public class CustomerInviteCodeMap : NopEntityTypeConfiguration<CustomerInviteCode>
    {
        public CustomerInviteCodeMap()
        {
            this.ToTable("CustomerInviteCode");
            this.HasKey(c => c.Id);
            this.Property(c => c.CustomerId);
            this.Property(c => c.Code);
            this.Property(c => c.IsUsed);
        }
    }
}
