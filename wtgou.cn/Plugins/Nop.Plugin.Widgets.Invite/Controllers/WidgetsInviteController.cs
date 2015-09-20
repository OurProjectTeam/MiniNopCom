using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nop.Services.Customers;
using Nop.Web.Framework.Controllers;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.Invite.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Stores;

namespace Nop.Plugin.Widgets.Invite.Controllers
{
    public class WidgetsInviteController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerInviteService _customerInviteService;

        public WidgetsInviteController(
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            ICustomerInviteService customerInviteService,
            IWorkContext workContext)
        {
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
            this._customerInviteService = customerInviteService;
            this._workContext = workContext;
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
            model.MaxCodeLimit = 10;

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
            var nivoSliderSettings = _settingService.LoadSetting<InviteSetting>(storeScope);
            nivoSliderSettings.CodeType = model.CodeType;
            nivoSliderSettings.MaxCodeLimit = model.MaxCodeLimit;

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            //var nivoSliderSettings = _settingService.LoadSetting<InviteSetting>(_storeContext.CurrentStore.Id);
            var customerCode = _customerInviteService.CustomerInviteCodes(2, 10, _workContext.CurrentCustomer.Id);
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
