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
            //if (storeScope > 0)
            //{
            //    model.CodeType = _settingService.SettingExists(inviteSettings, x => x.CodeType, storeScope);
            //    model.MaxCodeLimit = _settingService.SettingExists(inviteSettings, x => x.MaxCodeLimit, storeScope);
            //    model.EcommerceScript_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.EcommerceScript, storeScope);
            //    model.EcommerceDetailScript_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.EcommerceDetailScript, storeScope);
            //    model.IncludingTax_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.IncludingTax, storeScope);
            //}

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
            //model.Picture1Url = GetPictureUrl(nivoSliderSettings.Picture1Id);
            //model.Text1 = nivoSliderSettings.Text1;
            //model.Link1 = nivoSliderSettings.Link1;

            //model.Picture2Url = GetPictureUrl(nivoSliderSettings.Picture2Id);
            //model.Text2 = nivoSliderSettings.Text2;
            //model.Link2 = nivoSliderSettings.Link2;

            //model.Picture3Url = GetPictureUrl(nivoSliderSettings.Picture3Id);
            //model.Text3 = nivoSliderSettings.Text3;
            //model.Link3 = nivoSliderSettings.Link3;

            //model.Picture4Url = GetPictureUrl(nivoSliderSettings.Picture4Id);
            //model.Text4 = nivoSliderSettings.Text4;
            //model.Link4 = nivoSliderSettings.Link4;

            //model.Picture5Url = GetPictureUrl(nivoSliderSettings.Picture5Id);
            //model.Text5 = nivoSliderSettings.Text5;
            //model.Link5 = nivoSliderSettings.Link5;

            //if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
            //    string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
            //    string.IsNullOrEmpty(model.Picture5Url))
            //    //no pictures uploaded
            //    return Content("");


            return View("~/Plugins/Widgets.Invite/Views/WidgetsInvite/PublicInfo.cshtml", model);
        }

    }
}
