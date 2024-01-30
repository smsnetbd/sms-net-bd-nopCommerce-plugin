using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using System.Linq;

namespace Nop.Plugin.SMS.Net.bd
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly SmsNetBdSettings _AlphaSettings;
        private readonly IPluginService _pluginFinder;
        private readonly IOrderService _orderService;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;

        public OrderPlacedEventConsumer(ICustomerService customerService, SmsNetBdSettings AlphaSettings,
            IPluginService pluginFinder,
            IOrderService orderService,
            IStoreContext storeContext)
        {
            this._AlphaSettings = AlphaSettings;
            this._pluginFinder = pluginFinder;
            this._orderService = orderService;
            this._storeContext = storeContext;
            this._customerService = customerService;
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;
            //if (!_pluginFinder.AuthenticateStore(pluginDescriptor, _storeContext.CurrentStore.Id))
            //    return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;

            if (_AlphaSettings.Enabled && _AlphaSettings.EnabledConfirmOrder)
            {
                var order = eventMessage.Order;
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer

                //send SMS
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.SendToCustomerConfirmOrderSMSEnabled)
                {
                    string ConfirmOrderSMSFormat = _AlphaSettings.ConfirmOrderSMSForCustomerFormat;
                    if (ConfirmOrderSMSFormat != null && ConfirmOrderSMSFormat != null)
                    {
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[ID]%", order.Id.ToString());
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[OrderTotal]%", order.OrderTotal.ToString());
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[OwnerPhoneNumber]%", _AlphaSettings.OwnerNumber);

                    }
                    else
                    {
                        ConfirmOrderSMSFormat = _storeContext.CurrentStore.Name + "Order is Placed #" + order.Id.ToString() + " and Total Amount: " + order.OrderTotal.ToString();
                    }
                    if (plugin.SendSms(customer.PhoneNumber, ConfirmOrderSMSFormat))
                    {
                        //eventMessage.Order.note.Add(new OrderNote
                        //{
                        //    Note = "\"Order placed\" SMS alert (to store owner) has been sent",
                        //    DisplayToCustomer = false,
                        //    CreatedOnUtc = DateTime.UtcNow
                        //});
                        _orderService.UpdateOrder(order);
                    }
                }
                if (_AlphaSettings.OwnerEnabled && _AlphaSettings.SendToOwnerConfirmOrderSMSEnabled)
                {
                    string ConfirmOrderSMSFormat = _AlphaSettings.ConfirmOrderSMSForOwnerFormat;
                    if (ConfirmOrderSMSFormat != null && ConfirmOrderSMSFormat != "")
                    {
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[ID]%", order.Id.ToString());
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[OrderTotal]%", order.OrderTotal.ToString());
                        ConfirmOrderSMSFormat = ConfirmOrderSMSFormat.Replace("%[CustomerPhoneNumber]%", customer.PhoneNumber);
                    }
                    else
                    {
                        ConfirmOrderSMSFormat = _storeContext.CurrentStore.Name + "Order is Placed #" + order.Id.ToString() + " and Total Amount: " + order.OrderTotal.ToString();
                    }
                    if (plugin.SendSms(_AlphaSettings.OwnerNumber, ConfirmOrderSMSFormat))
                    {
                        //eventMessage.Order.note.Add(new OrderNote
                        //{
                        //    Note = "\"Order placed\" SMS alert (to store owner) has been sent",
                        //    DisplayToCustomer = false,
                        //    CreatedOnUtc = DateTime.UtcNow
                        //});
                        _orderService.UpdateOrder(order);
                    }
                }
            }
        }
    }
}