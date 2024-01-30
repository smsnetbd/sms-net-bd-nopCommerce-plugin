namespace Nop.Plugin.Sms.Net.bd.Models
{
    public class SmsAlphaModel
    {
        // [NopResourceDisplayName("Plugins.Sms.Alpha.Fields.Enabled")]
        public bool Enabled { get; set; }
        public bool CustomerEnabled { get; set; }
        //[NopResourceDisplayName("Plugins.Sms.Alpha.Fields.Email")]
        public string ConfirmOrderSMSForOwnerFormat { get; set; }
        public string ConfirmOrderSMSForCustomerFormat { get; set; }

        //[NopResourceDisplayName("Plugins.Sms.Alpha.Fields.TestMessage")]
        public string Number { get; set; }

        public bool CustomerRegOTPEnabled { get; set; }
        public string CustomerRegOTPSMSFormat { get; set; }

        public string API_Url { get; set; }

        public string API_Key { get; set; }

        public string OwnerNumber { get; set; }
        public string sender_id { get; set; }

        public bool OwnerEnabled { get; set; }

        //registered
        public bool EnabledRegistered { get; set; }
        public string RegisteredSMSFormat { get; set; }
        public bool SendToCustomerAccRegSMSEnabled { get; set; }
        public bool SendToOwnerAccRegSMSEnabled { get; set; }

        //ConfirmOrder
        public bool EnabledConfirmOrder { get; set; }

        public string ConfirmOrderSMSFormat { get; set; }
        public bool SendToCustomerConfirmOrderSMSEnabled { get; set; }
        public bool SendToOwnerConfirmOrderSMSEnabled { get; set; }

        //Paymented
        public bool EnabledPaymented { get; set; }

        public string PaymentedSMSFormat { get; set; }

        //Shipped
        public bool EnabledOrderShipping { get; set; }

        public string OrderShippingSMSFormat { get; set; }

        //Completed
        public bool EnabledOrderCompleted { get; set; }

        public string OrderCompletedSMSFormat { get; set; }


        //Canceled
        public bool EnabledOrderCanceled { get; set; }
        public string OrderCanceledSMSFormat { get; set; }

        public string Email { get; set; }

        public string TestMessage { get; set; }
    }
}