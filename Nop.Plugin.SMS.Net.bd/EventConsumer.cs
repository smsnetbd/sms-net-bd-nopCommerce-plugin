using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Events;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Orders;
using Nop.Services.Orders.Caching;
using Nop.Services.Plugins;
using Nop.Services.Shipping.Caching;
using Nop.Services.Shipping.Tracking;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Linq;

namespace Nop.Plugin.SMS.Net.bd
{
    public class EventConsumer : IConsumer<OrderPlacedEvent>,
                                 IConsumer<OrderCancelledEvent>,
                                 IConsumer<OrderPaidEvent>,
                                 IConsumer<OrderRefundedEvent>,
                                 //IConsumer<EntityUpdatedEvent<Order>>,
                                 IConsumer<ShipmentSentEvent>,
                                 IConsumer<ShipmentDeliveredEvent>

    {
        private readonly SmsNetBdSettings _AlphaSettings;
        private readonly IPluginService _pluginFinder;
        private readonly IOrderService _orderService;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;

        public EventConsumer(ICustomerService customerService, SmsNetBdSettings AlphaSettings,
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
                    if (ConfirmOrderSMSFormat != null && ConfirmOrderSMSFormat != "")
                    {
                        ConfirmOrderSMSFormat = SMSFromat(ConfirmOrderSMSFormat, order, customer);

                    }
                    else
                    {
                        ConfirmOrderSMSFormat = _storeContext.CurrentStore.Name + "Order is Placed #" + order.Id.ToString() + " and Total Amount: " + order.OrderTotal.ToString();
                    }
                    if (plugin.SendSms(customer.PhoneNumber, ConfirmOrderSMSFormat))
                    {
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
                        
                    }
                    else
                    {
                        ConfirmOrderSMSFormat = _storeContext.CurrentStore.Name + "Order is Placed #" + order.Id.ToString() + " and Total Amount: " + order.OrderTotal.ToString();
                    }
                    if (plugin.SendSms(_AlphaSettings.OwnerNumber, ConfirmOrderSMSFormat))
                    {

                        _orderService.UpdateOrder(order);
                    }
                }
            }
        }
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderCancelledEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;
            if (_AlphaSettings.Enabled && _AlphaSettings.EnabledOrderCanceled)
            {
                var order = eventMessage.Order;
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer

                //send SMS
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.EnabledOrderCanceled)
                {
                    string OrderCanceledSMSFormat = _AlphaSettings.OrderCanceledSMSFormat;
                    if (OrderCanceledSMSFormat != null && OrderCanceledSMSFormat != "")
                    {
                        OrderCanceledSMSFormat = SMSFromat(OrderCanceledSMSFormat,order, customer);
                    }
                    else
                    {
                        OrderCanceledSMSFormat = _storeContext.CurrentStore.Name + "Your Order has been cancelled. Order ID is" + order.Id.ToString();
                    }
                    if (plugin.SendSms(customer.PhoneNumber, OrderCanceledSMSFormat))
                    {
                        _orderService.UpdateOrder(order);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPaidEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;
            if (_AlphaSettings.Enabled && _AlphaSettings.EnableOrderPaid)
            {
                var order = eventMessage.Order;
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer

                //send SMS
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.EnableOrderPaid)
                {
                    string OrderPaidSMSFormat = _AlphaSettings.OrderPaidSMSFormat;
                    if (OrderPaidSMSFormat != null && OrderPaidSMSFormat != "")
                    {
                        OrderPaidSMSFormat = SMSFromat(OrderPaidSMSFormat, order, customer);  

                    }
                    else
                    {
                        OrderPaidSMSFormat = _storeContext.CurrentStore.Name + "Paid";
                    }
                    if (plugin.SendSms(customer.PhoneNumber, OrderPaidSMSFormat))
                    {
                        _orderService.UpdateOrder(order);
                    }
                }
            }
        }
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderRefundedEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;
            if (_AlphaSettings.Enabled && _AlphaSettings.EnableOrderRefunded)
            {
                var order = eventMessage.Order;
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer

                //send SMS
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.EnableOrderRefunded)
                {
                    string OrderRefundedSMSFormat = _AlphaSettings.OrderRefundedSMSFormat;
                    if (OrderRefundedSMSFormat != null && OrderRefundedSMSFormat != "")
                    {
                        OrderRefundedSMSFormat = SMSFromat(OrderRefundedSMSFormat, order, customer);

                    }
                    else
                    {
                        OrderRefundedSMSFormat = _storeContext.CurrentStore.Name + "Sir , We wanted to inform you that your recent order :"+ order.Id.ToString() + " has been refunded.";
                    }
                    if (plugin.SendSms(customer.PhoneNumber, OrderRefundedSMSFormat))
                    {
                        _orderService.UpdateOrder(order);
                    }
                }
            }
        }
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(ShipmentSentEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;
            if (_AlphaSettings.Enabled && _AlphaSettings.EnabledOrderShipping)
            {
                var shipment = eventMessage.Shipment;
                var order = _orderService.GetOrderById(shipment.OrderId);
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.EnabledOrderShipping)
                {
                    string OrderShippingSMSFormat = _AlphaSettings.OrderShippingSMSFormat;
                    if (OrderShippingSMSFormat != null && OrderShippingSMSFormat != "")
                    {
                        OrderShippingSMSFormat = SMSFromat(OrderShippingSMSFormat,order,customer);

                    }
                    else
                    {
                        OrderShippingSMSFormat = "[" + _storeContext.CurrentStore.Name + "]" + customer.FirstName + ", Your order " + order.Id.ToString() + " has been " + (order.ShippingStatus.Equals(ShippingStatus.Shipped.ToString()) ? ShippingStatus.Shipped.ToString() : ShippingStatus.Delivered.ToString()) + ".";
                    }
                    if (plugin.SendSms(customer.PhoneNumber, OrderShippingSMSFormat))
                    {
                        _orderService.UpdateOrder(order);
                    }
                    //send SMS
                }
            }
        }
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(ShipmentDeliveredEvent eventMessage)
        {
            //is enabled?
            if (!_AlphaSettings.Enabled)
                return;

            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName<IPlugin>("Mobile.sms.net.bd", LoadPluginsMode.All);
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance<IPlugin>() as SmsNetBdProvider;
            if (plugin == null)
                return;
            if (_AlphaSettings.Enabled && _AlphaSettings.EnabledOrderShipping)
            {
                var shipment = eventMessage.Shipment;
                var order = _orderService.GetOrderById(shipment.OrderId);
                var customer = _customerService.GetAddressesByCustomerId(order.CustomerId).FirstOrDefault();
                //var GetCustomer
                if (_AlphaSettings.CustomerEnabled && _AlphaSettings.EnabledOrderShipping)
                {
                    string OrderShippingSMSFormat = _AlphaSettings.OrderShippingSMSFormat;
                    if (OrderShippingSMSFormat != null && OrderShippingSMSFormat != "")
                    {
                        OrderShippingSMSFormat = SMSFromat(OrderShippingSMSFormat, order, customer);
                    }
                    else
                    {
                        OrderShippingSMSFormat = "[" + _storeContext.CurrentStore.Name + "]" + customer.FirstName + ", Your order " + order.Id.ToString() + " has been " + (order.ShippingStatus.Equals(ShippingStatus.Shipped.ToString()) ? ShippingStatus.Shipped.ToString() : ShippingStatus.Delivered.ToString()) + ".";
                    }
                    if (plugin.SendSms(customer.PhoneNumber, OrderShippingSMSFormat))
                    {
                        _orderService.UpdateOrder(order);
                    }
                    //send SMS
                }
            }
        }

        public string SMSFromat(string smsFormat, Order order, Address customer)
        {
            smsFormat = smsFormat.Replace("%[ID]%", order.Id.ToString());
            smsFormat = smsFormat.Replace("%[OrderTotal]%", order.OrderTotal.ToString());
            smsFormat = smsFormat.Replace("%[OwnerPhoneNumber]%", _AlphaSettings.OwnerNumber);
            smsFormat = smsFormat.Replace("%[OrderStatus]%", order.OrderStatus.ToString());
            smsFormat = smsFormat.Replace("%[StoreName]%", _storeContext.CurrentStore.Name.ToString());
            smsFormat = smsFormat.Replace("%[ShippingStatus]%", order.ShippingStatus.ToString());
            smsFormat = smsFormat.Replace("%[CustomerPhoneNumber]%", customer.PhoneNumber);
            smsFormat = smsFormat.Replace("%[CustomerFirstName]%", customer.FirstName);
            return smsFormat;
        }
    }
}