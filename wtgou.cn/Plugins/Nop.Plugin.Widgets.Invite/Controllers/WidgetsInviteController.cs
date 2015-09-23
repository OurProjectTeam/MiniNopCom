using System.Web.Mvc;
using Autofac;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.Invite.Models;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Nop.Plugin.Widgets.Invite.Controllers
{
    public class WidgetsInviteController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerInviteService _customerInviteService;
        private readonly IStoreService _storeService;

        public WidgetsInviteController(
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            ICustomerInviteService customerInviteService,
            IStoreService storeService,
            IWorkContext workContext)
        {
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._customerInviteService = customerInviteService;
            this._workContext = workContext;
            this._storeService = storeService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var inviteSettings = _settingService.LoadSetting<InviteSetting>(storeScope);
            var model = new ConfigurationModel();
            model.CodeType = inviteSettings.CodeType;
            model.MaxCodeLimit = inviteSettings.MaxCodeLimit;

            model.ActiveStoreScopeConfiguration = storeScope;

            return View("~/Plugins/Widgets.Invite/Views/WidgetsInvite/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var inviteSettings = _settingService.LoadSetting<InviteSetting>(storeScope);
            inviteSettings.CodeType = model.CodeType;
            inviteSettings.MaxCodeLimit = model.MaxCodeLimit;

            _settingService.SaveSetting(inviteSettings, x => x.CodeType, storeScope);
            _settingService.SaveSetting(inviteSettings, x => x.MaxCodeLimit, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var inviteSettings = _settingService.LoadSetting<InviteSetting>(storeScope);
            var customerCode = _customerInviteService.CustomerInviteCodes((int)inviteSettings.CodeType, inviteSettings.MaxCodeLimit, _workContext.CurrentCustomer.Id);
            var model = new PublicInfoModel();
            model.List = customerCode;

            return View("~/Plugins/Widgets.Invite/Views/WidgetsInvite/PublicInfo.cshtml", model);
        }

    }
}
