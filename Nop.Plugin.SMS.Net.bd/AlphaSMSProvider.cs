using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Nop.Plugin.SMS.Net.bd
{
    /// <summary>
    /// Represents the Alpha SMS provider
    /// </summary>
    public class AlphaSmsProvider : BasePlugin, IMiscPlugin
    {
        private readonly IEmailAccountService _emailAccountService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly AlphaSMSSettings _AlphaSMSSettings;

        public AlphaSmsProvider(IEmailAccountService emailAccountService,
            ILocalizationService localizationService,
            ILogger logger,
            IQueuedEmailService queuedEmailService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            EmailAccountSettings emailAccountSettings,
            AlphaSMSSettings AlphaSMSSettings)
        {
            this._emailAccountService = emailAccountService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._queuedEmailService = queuedEmailService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._webHelper = webHelper;
            this._emailAccountSettings = emailAccountSettings;
            this._AlphaSMSSettings = AlphaSMSSettings;
        }

        /// <summary>
        /// Sends SMS
        /// </summary>
        /// <param name="text">SMS text</param>
        /// <returns>Result</returns>
        public bool SendSms(string num, string meg, string sender_id = null)
        {
            if (num != null)
            {
                try
                {
                    using (var client = new HttpClient())
                    {

                        client.BaseAddress = new Uri(_AlphaSMSSettings.API_Url);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = client.GetAsync("?api_key=" + _AlphaSMSSettings.API_Key + "&msg=" + meg + "&to=" + num + "&sender_id=" + sender_id).Result;
                        using (HttpContent content = response.Content)
                        {
                            var bkresult = content.ReadAsStringAsync().Result;
                            dynamic stuff = JsonConvert.DeserializeObject(bkresult);
                            if (stuff.error == "0")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/SmsAlpha/Configure";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            var settings = new AlphaSMSSettings
            {
                API_Url = "https://api.sms.net.bd/sendsms?",
                Enabled = true,
                EnabledConfirmOrder = true,
                EnabledOrderCanceled = true,
                EnabledOrderCompleted = false,
                EnabledOrderShipping = false,
                EnabledPaymented = false,
                EnabledRegistered = false,
                ConfirmOrderSMSForCustomerFormat = "Your Order is Confirmed . Order ID is %[ID]%. Total Amount is %[OrderTotal]%. Please Pay Now. " + _storeContext.CurrentStore.Name
            };
            _settingService.SaveSetting(settings);

            //locales
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.TestFailed", "Test message sending failed");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.TestSuccess", "Test message was sent (queued)");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.Enabled", "Enabled");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.Enabled.Hint", "Check to enable SMS provider");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.Email", "Email");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.Email.Hint", "Alpha email address(e.g. your_phone_number@vtext.com)");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.TestMessage", "Message text");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.Fields.TestMessage.Hint", "Text of the test message");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.SendTest", "Send");
            //_localizationService.AddOrUpdatePluginLocaleResource("Plugins.Sms.Alpha.SendTest.Hint", "Send test message");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<AlphaSMSSettings>();

            //locales
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.TestFailed");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.TestSuccess");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.Enabled");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.Enabled.Hint");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.Email");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.Email.Hint");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.TestMessage");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.Fields.TestMessage.Hint");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.SendTest");
            //_localizationService.DeletePluginLocaleResource("Plugins.Sms.Alpha.SendTest.Hint");

            base.Uninstall();
        }
    }
}
