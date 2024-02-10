using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Sms.Net.bd.Models;
using Nop.Plugin.SMS.Net.bd;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;

namespace Nop.Plugin.Sms.Net.bd.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class SmsNetBdController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IPluginService _pluginFinder;
        private readonly ISettingService _settingService;
        private readonly SmsNetBdSettings _AlphaSettings;
        private readonly INotificationService _notificationService;

        public SmsNetBdController(ILocalizationService localizationService,
            IPermissionService permissionService,
            IPluginService pluginFinder,
            ISettingService settingService,
            INotificationService notificationService,
            SmsNetBdSettings AlphaSettings)
        {
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._pluginFinder = pluginFinder;
            this._settingService = settingService;
            this._AlphaSettings = AlphaSettings;
            _notificationService = notificationService;

        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            var model = new SmsNetBdModel
            {
                Enabled = _AlphaSettings.Enabled,
                Email = _AlphaSettings.Email,
                API_Key = _AlphaSettings.API_Key,
                API_Url = _AlphaSettings.API_Url==null? "https://api.sms.net.bd/sendsms?":_AlphaSettings.API_Url,
                //ConfirmOrderSMSFormat = _AlphaSettings.ConfirmOrderSMSFormat,
                EnabledConfirmOrder = _AlphaSettings.EnabledConfirmOrder,
                EnabledOrderCanceled = _AlphaSettings.EnabledOrderCanceled,
                EnabledOrderCompleted = _AlphaSettings.EnabledOrderCompleted,
                EnabledOrderShipping = _AlphaSettings.EnabledOrderShipping,
                EnabledPaymented = _AlphaSettings.EnabledPaymented,
                EnabledRegistered = _AlphaSettings.EnabledRegistered,
                EnableOrderPaid = _AlphaSettings.EnableOrderPaid,
                OrderCanceledSMSFormat = _AlphaSettings.OrderCanceledSMSFormat,
                OrderCompletedSMSFormat = _AlphaSettings.OrderCompletedSMSFormat,
                OrderShippingSMSFormat = _AlphaSettings.OrderShippingSMSFormat,
                PaymentedSMSFormat = _AlphaSettings.PaymentedSMSFormat,
                RegisteredSMSFormat = _AlphaSettings.RegisteredSMSFormat,
                sender_id = _AlphaSettings.sender_id,
                SendToOwnerConfirmOrderSMSEnabled = _AlphaSettings.SendToOwnerConfirmOrderSMSEnabled,
                SendToCustomerConfirmOrderSMSEnabled = _AlphaSettings.SendToCustomerConfirmOrderSMSEnabled,
                CustomerRegOTPSMSFormat = _AlphaSettings.CustomerRegOTPSMSFormat,
                CustomerRegOTPEnabled = _AlphaSettings.CustomerRegOTPEnabled,
                OwnerNumber = _AlphaSettings.OwnerNumber,
                OwnerEnabled = _AlphaSettings.OwnerEnabled,
                CustomerEnabled = _AlphaSettings.CustomerEnabled,
                SendToOwnerAccRegSMSEnabled = _AlphaSettings.SendToOwnerAccRegSMSEnabled,
                SendToCustomerAccRegSMSEnabled = _AlphaSettings.SendToCustomerAccRegSMSEnabled,
                ConfirmOrderSMSForOwnerFormat = _AlphaSettings.ConfirmOrderSMSForOwnerFormat,
                ConfirmOrderSMSForCustomerFormat = _AlphaSettings.ConfirmOrderSMSForCustomerFormat,
                EnableOrderRefunded = _AlphaSettings.EnableOrderRefunded,
                OrderRefundedSMSFormat = _AlphaSettings.OrderRefundedSMSFormat,
                OrderPaidSMSFormat = _AlphaSettings.OrderPaidSMSFormat,
            };

            return View("~/Plugins/SMS.Net.bd/Views/Configure.cshtml", model);
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public IActionResult ConfigurePOST(SmsNetBdModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            if (!ModelState.IsValid)
            {
                return Configure();
            }

            //save settings
            _AlphaSettings.Enabled = model.Enabled;
            _AlphaSettings.Email = model.Email;
            _AlphaSettings.sender_id = model.sender_id;


            _AlphaSettings.CustomerEnabled = model.CustomerEnabled;
            _AlphaSettings.OwnerEnabled = model.OwnerEnabled;
            _AlphaSettings.OwnerNumber = model.OwnerNumber;
            _AlphaSettings.CustomerRegOTPEnabled = model.CustomerRegOTPEnabled;
            _AlphaSettings.CustomerRegOTPSMSFormat = model.CustomerRegOTPSMSFormat;

            _AlphaSettings.OrderCanceledSMSFormat = model.OrderCanceledSMSFormat;
            _AlphaSettings.API_Key = model.API_Key;
            _AlphaSettings.API_Url = model.API_Url;

            // _AlphaSettings.ConfirmOrderSMSFormat = model.ConfirmOrderSMSFormat;
            _AlphaSettings.EnabledConfirmOrder = model.EnabledConfirmOrder;
            _AlphaSettings.SendToCustomerConfirmOrderSMSEnabled = model.SendToCustomerConfirmOrderSMSEnabled;
            _AlphaSettings.SendToOwnerConfirmOrderSMSEnabled = model.SendToOwnerConfirmOrderSMSEnabled;

            _AlphaSettings.EnabledOrderCanceled = model.EnabledOrderCanceled;
            _AlphaSettings.EnabledOrderCompleted = model.EnabledOrderCompleted;
            _AlphaSettings.EnabledOrderShipping = model.EnabledOrderShipping;
            _AlphaSettings.EnabledPaymented = model.EnabledPaymented;
            _AlphaSettings.EnabledRegistered = model.EnabledRegistered;
            _AlphaSettings.EnableOrderRefunded = model.EnableOrderRefunded;
            _AlphaSettings.EnableOrderPaid = model.EnableOrderPaid;
            _AlphaSettings.OrderCompletedSMSFormat = model.OrderCompletedSMSFormat;
            _AlphaSettings.OrderShippingSMSFormat = model.OrderShippingSMSFormat;
            _AlphaSettings.PaymentedSMSFormat = model.PaymentedSMSFormat;
            _AlphaSettings.RegisteredSMSFormat = model.RegisteredSMSFormat;
            _AlphaSettings.ConfirmOrderSMSForCustomerFormat = model.ConfirmOrderSMSForCustomerFormat;
            _AlphaSettings.ConfirmOrderSMSForOwnerFormat = model.ConfirmOrderSMSForOwnerFormat;
            _AlphaSettings.OrderRefundedSMSFormat = model.OrderRefundedSMSFormat;
            _AlphaSettings.OrderPaidSMSFormat = model.OrderPaidSMSFormat;
            _settingService.SaveSetting(_AlphaSettings);

            // SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [HttpPost, ActionName("Configure")]
        [FormValueRequired("test-sms")]
        public IActionResult TestSms(SmsNetBdModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            try
            {
                if (string.IsNullOrEmpty(model.TestMessage))
                {
                    //ErrorNotification("Enter test message");
                    _notificationService.ErrorNotification("Enter test message");
                }
                else
                {
                    var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
                    //_pluginFinder.GetPluginDescriptorBySystemName("Mobile.SMS.Alpha");
                    if (pluginDescriptor == null)
                        throw new Exception("Cannot load the plugin");
                    var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
                    if (plugin == null)
                        throw new Exception("Cannot load the plugin");

                    if (plugin.SendSms(model.Number, model.TestMessage))
                    {
                        _notificationService.SuccessNotification(_localizationService.GetResource("Plugins.Sms.Net.bd.TestSuccess"));

                    }
                    else
                    {

                        _notificationService.ErrorNotification(_localizationService.GetResource("Plugins.Sms.Net.bd.TestFailed"));
                    }
                }
            }
            catch (Exception exc)
            {
                _notificationService.ErrorNotification(exc.ToString());

            }

            return View("~/Plugins/SMS.Net.bd/Views/Configure.cshtml", model);
        }
    }
}