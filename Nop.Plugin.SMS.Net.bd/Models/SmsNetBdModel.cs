using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel;

namespace Nop.Plugin.Sms.Net.bd.Models
{
    public class SmsNetBdModel
    {
        // [NopResourceDisplayName("Plugins.Sms.Alpha.Fields.Enabled")]
        public bool Enabled { get; set; }
        [NopResourceDisplayName("Customer Enabled")]
        public bool CustomerEnabled { get; set; }
        //[NopResourceDisplayName("Plugins.Sms.Alpha.Fields.Email")]
        [NopResourceDisplayName("Confirm Order SMS For Owner Format")]
        public string ConfirmOrderSMSForOwnerFormat { get; set; }
        [NopResourceDisplayName("Confirm Order SMS For Customer Format")]
        public string ConfirmOrderSMSForCustomerFormat { get; set; }
        //[NopResourceDisplayName("Plugins.Sms.Alpha.Fields.TestMessage")]
        public string Number { get; set; }
        [NopResourceDisplayName("Customer RegOTP Enabled")]
        public bool CustomerRegOTPEnabled { get; set; }
        [NopResourceDisplayName("Customer RegOTP SMS Format")]
        public string CustomerRegOTPSMSFormat { get; set; }
        public string API_Url { get; set; }
        public string API_Key { get; set; }
        [NopResourceDisplayName("Owner Number")]
        public string OwnerNumber { get; set; }
        public string sender_id { get; set; }
        [NopResourceDisplayName("Owner Enabled")]
        public bool OwnerEnabled { get; set; }
        //registered
        [NopResourceDisplayName("Enabled Registered")]
        public bool EnabledRegistered { get; set; }
        [NopResourceDisplayName("Registered SMS Format")]
        public string RegisteredSMSFormat { get; set; }
        [NopResourceDisplayName("Send To Customer AccReg SMS Enabled")]
        public bool SendToCustomerAccRegSMSEnabled { get; set; }
        [NopResourceDisplayName("Send To Owner AccReg SMS Enabled")]
        public bool SendToOwnerAccRegSMSEnabled { get; set; }
        //ConfirmOrder
        [NopResourceDisplayName("Enabled Confirm Order")]
        public bool EnabledConfirmOrder { get; set; }
        [NopResourceDisplayName("Confirm Order SMS Format")]
        public string ConfirmOrderSMSFormat { get; set; }
        [NopResourceDisplayName("Send To Customer Confirm Order SMS Enabled")]
        public bool SendToCustomerConfirmOrderSMSEnabled { get; set; }
        [NopResourceDisplayName("Send To Owner Confirm Order SMS Enabled")]
        public bool SendToOwnerConfirmOrderSMSEnabled { get; set; }
        //Paymented
        [NopResourceDisplayName("Enabled Paymented")]
        public bool EnabledPaymented { get; set; }
        [NopResourceDisplayName("Paymented SMS Format")]
        public string PaymentedSMSFormat { get; set; }
        //Shipped
        [NopResourceDisplayName("Enabled Order Shipping")]
        public bool EnabledOrderShipping { get; set; }
        [NopResourceDisplayName("Order Shipping SMS Format")]
        public string OrderShippingSMSFormat { get; set; }
        //Completed
        [NopResourceDisplayName("Enabled Order Completed")]
        public bool EnabledOrderCompleted { get; set; }
        [NopResourceDisplayName("Order Completed SMSFormat")]
        public string OrderCompletedSMSFormat { get; set; }
        //Canceled
        [NopResourceDisplayName("Enabled Order Canceled")]
        public bool EnabledOrderCanceled { get; set; }
        [NopResourceDisplayName("Order Canceled SMS Format")]
        public string OrderCanceledSMSFormat { get; set; }
        [NopResourceDisplayName("Enable Order Refunded")]
        public bool EnableOrderRefunded { get; set; }
        [NopResourceDisplayName("Order Refunded SMS Format")]
        [Description("This is your help text for the nopeditor field.")]
        public string OrderRefundedSMSFormat { get; set; }

        [NopResourceDisplayName("Enable Order Paid")]
        public bool EnableOrderPaid { get; set; }
        [NopResourceDisplayName("Order Paid SMS Format")]
        public string OrderPaidSMSFormat { get; set; }

        public string Email { get; set; }

        public string TestMessage { get; set; }
    }
}