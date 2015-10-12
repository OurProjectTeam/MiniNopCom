using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Blogs;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.News;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Polls;
using Nop.Data;
using Nop.Services.Common;
using Nop.Services.Events;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Nop.Services.Customers
{
    public class CustomerInviteService : ICustomerInviteService
    {
        #region Constants

        /// <summary>
        /// Customer Inviate Code template
        /// </summary>
        /// <remarks>
        /// {0} : Replace with CustomerId
        /// </remarks>
        private const string CUSTOMER_INVIETECODE_Signle = "Nop.Customer.InviteCode-{0}";
        /// <summary>
        /// ALL Customer Code
        /// </summary>
        private const string CUSTOMER_INVIETECODE_ALL = "Nop.Customer.InviteCode-All";

        #endregion

        #region Fields

        private readonly IRepository<CustomerInviteCode> _inviteCodeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        public CustomerInviteService(ICacheManager cacheManager,
            IRepository<CustomerInviteCode> inviteCodeRepository,
            IEventPublisher eventPublisher,
            IWorkContext workContext)
        {
            this._cacheManager = cacheManager;
            this._inviteCodeRepository = inviteCodeRepository;
            this._workContext = workContext;
            this._eventPublisher = eventPublisher;
        }

        public List<CustomerInviteCode> GetCustomerCode(int customerId)
        {
            string key = string.Format(CUSTOMER_INVIETECODE_Signle, customerId);
            return _cacheManager.Get(key, () =>
            {
                List<CustomerInviteCode> codelist = new List<CustomerInviteCode>();
                var query = from q in _inviteCodeRepository.Table where q.CustomerId == customerId select q;
                if (query.Any())
                {
                    foreach (var item in query.ToList())
                    {
                        codelist.Add(new CustomerInviteCode()
                        {
                            Code = item.Code,
                            IsUsed = item.IsUsed,
                            CustomerId = item.CustomerId
                        });
                    }
                }
                return codelist;
            });
        }

        public void CreateNewCode(int codeType, int maxCodeLimit)
        {
            if (maxCodeLimit > 0)
            {
                for (int i = 0; i < maxCodeLimit; i++)
                {
                    string code = string.Empty;
                    if (codeType == 0) //salty
                    {
                        code = CommonHelper.ShortUniqueCode();
                    }
                    else
                    {
                        code = CommonHelper.GuidCode();
                    }
                    _inviteCodeRepository.Insert(new CustomerInviteCode()
                    {
                        Code = code,
                        CustomerId = _workContext.CurrentCustomer.Id,
                        IsUsed = false
                    });
                }
                string key = string.Format(CUSTOMER_INVIETECODE_Signle, _workContext.CurrentCustomer.Id);
                _cacheManager.Remove(key);
            }
        }

        public List<CustomerInviteCode> CustomerInviteCodes(int codeType, int maxCodeLimit, int customerId)
        {
            var codes = GetCustomerCode(customerId);
            if (codes.Count > 0)
            {
                return codes;
            }
            CreateNewCode(codeType, maxCodeLimit);
            return GetCustomerCode(customerId);
        }

        public bool IsValidCustomerInviteCode(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    var list = from p in _inviteCodeRepository.Table where p.Code == code && !p.IsUsed select p;
                    if (list.Any())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetInviteCodeBeUsed(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    var invitecode = from p in _inviteCodeRepository.Table where p.Code == code && !p.IsUsed select p;
                    if (invitecode.Any() && invitecode.FirstOrDefault() != null)
                    {
                        var codeModel = invitecode.FirstOrDefault();
                        codeModel.IsUsed = true;
                        _inviteCodeRepository.Update(codeModel);

                        _eventPublisher.EntityUpdated(codeModel);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
