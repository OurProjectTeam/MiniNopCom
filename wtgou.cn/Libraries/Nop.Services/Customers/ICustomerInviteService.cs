using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Configuration;

namespace Nop.Services.Customers
{
    public interface ICustomerInviteService
    {
        List<CustomerInviteCode> GetCustomerCode(int customerId);

        void CreateNewCode(int codeType, int maxCodeLimit);

        List<CustomerInviteCode> CustomerInviteCodes(int codeType, int maxCodeLimit, int customerId);
    }
}
