using Visum.Services.Mobile.Entities;
using FocalPoint.Modules.FrontCounter.Views;
using FocalPtMbl.MainMenu.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Net.Http;
using FocalPoint.Data;
using Newtonsoft.Json;

namespace FocalPoint.Modules.FrontCounter.ViewModels
{
    public class QuickOrderCreationOutlineViewModel : ThemeBaseViewModel
    {
        public List<RentMerchItem> Data { get; }
        public ObservableCollection<string> Greetings { get; set; }
        public ObservableCollection<Customer> selCust { get; set; }
        public QuickOrderCreationOutlineViewModel()
            {
            var httpClientCache = DependencyService.Resolve<MainMenu.Services.IHttpClientCacheService>();
            this.clientHttp = httpClientCache.GetHttpClientAsync();
            ord = new Order();
            Greetings = new ObservableCollection<string>();
            selCust = new ObservableCollection<Customer>();
            Recent = new ObservableCollection<OrderDtl>();

            MessagingCenter.Subscribe<SelectCustomerView, string>(this, "Hi", (sender, arg) =>
            {
                Greetings.Add("Hi " + arg);
            });
            MessagingCenter.Subscribe<SelectCustomerView, OrderUpdate>(this, "Hi", (sender, arg) =>
            {
                string displayCustType = "";
                if (arg.Order.Customer.CustomerType == "c")
                    displayCustType = "Cash";
                if (arg.Order.Customer.CustomerType == "C")
                    displayCustType = "Cash";
                //update api selected customer

                SelectedCustomerNameBox = arg.Order.Customer.CustomerName + " " + Regex.Replace(arg.Order.Customer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine  + arg.Order.Customer.CustomerCity + ", " + arg.Order.Customer.CustomerState + " " + arg.Order.Customer.CustomerZip + " " + "Type: " + displayCustType ;

                //UpdateCust(arg.Order.Customer);
                CurrentOrder = arg.Order;
                selCust.Clear();
                selCust.Add(arg.Order.Customer);
            });
            MessagingCenter.Subscribe<CustomerLupView, Customer>(this, "Hi", (sender, arg) =>
            {
                string displayCustType = "";
                if (arg.CustomerType == "c")
                    displayCustType = "Cash";
                if (arg.CustomerType == "C")
                    displayCustType = "Cash";
                SelectedCustomerNameBox = arg.CustomerName + " " + Regex.Replace(arg.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + arg.CustomerCity + ", " + arg.CustomerState + " " + arg.CustomerZip + " " + "Type: " + displayCustType;

                selCust.Add(arg);

            });
            MessagingCenter.Subscribe<QuickOrderHeaderView, string>(this, "Hi", (sender, arg) =>
            {
                SelectedHeaderBox = arg;

            });
            MessagingCenter.Subscribe<QuickOrderDetailsMerchView, OrderUpdate>(this, "Hi", (sender, arg) =>
            {
                //string displayCustType = "";
                //if (arg.CustomerType == "c")
                //    displayCustType = "Cash";
                //if (arg.CustomerType == "C")
                //    displayCustType = "Cash";
                //SelectedCustomerNameBox = arg.CustomerName + " " + Regex.Replace(arg.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + arg.CustomerCity + ", " + arg.CustomerState + " " + arg.CustomerZip + " " + "Type: " + displayCustType;
                //selCust.Add(arg);
                //update order
                UpdateCust(arg.Order.Customer);
                CurrentOrder = arg.Order;
                selCust.Clear();
                selCust.Add(arg.Order.Customer);
                Recent.Clear();
                foreach (var item in arg.Order.OrderDtls)
                    Recent.Add(item);

            });
            MessagingCenter.Subscribe<QuickOrderDetailsView, OrderUpdate>(this, "Hi", (sender, arg) =>
            {
                //string displayCustType = "";
                //if (arg.CustomerType == "c")
                //    displayCustType = "Cash";
                //if (arg.CustomerType == "C")
                //    displayCustType = "Cash";
                //SelectedCustomerNameBox = arg.CustomerName + " " + Regex.Replace(arg.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + arg.CustomerCity + ", " + arg.CustomerState + " " + arg.CustomerZip + " " + "Type: " + displayCustType;
                //selCust.Add(arg);
                //update order
                UpdateCust(arg.Order.Customer);
                CurrentOrder = arg.Order;
                selCust.Clear();
                selCust.Add(arg.Order.Customer);
                Recent.Clear();
                foreach (var item in arg.Order.OrderDtls)
                    Recent.Add(item);

            });
            MessagingCenter.Subscribe<QuickOrderHeaderView, OrderUpdate>(this, "Hi", (sender, arg) =>
            {
                //string displayCustType = "";
                //if (arg.CustomerType == "c")
                //    displayCustType = "Cash";
                //if (arg.CustomerType == "C")
                //    displayCustType = "Cash";
                //SelectedCustomerNameBox = arg.CustomerName + " " + Regex.Replace(arg.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + arg.CustomerCity + ", " + arg.CustomerState + " " + arg.CustomerZip + " " + "Type: " + displayCustType;
                //selCust.Add(arg);
                //update order
                UpdateHeader(arg);
                CurrentOrder = arg.Order;
                selCust.Clear();
                selCust.Add(arg.Order.Customer);
                Recent.Clear();
                foreach (var item in arg.Order.OrderDtls)
                    Recent.Add(item);

            });


        }

        private OrderUpdate UpdateHeader(OrderUpdate arg)
        {
            try
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder = arg.Order;
                    //CurrentOrder.Customer = arg.Order.Customer;
                    //CurrentOrder.LengthDscr = arg.Order.LengthDscr;
                    //CurrentOrder.OrderAmount = arg.Order.OrderAmount;
                    //CurrentOrder.OrderDDte = arg.Order.OrderDDte;
                    //CurrentOrder.OrderODte = arg.Order.OrderODte;
                    //CurrentOrder.OrderType = arg.Order.OrderType;
                    //CurrentOrder.OrderTaxExempt = arg.Order.OrderTaxExempt;
                    //CurrentOrder.OrderTaxCode
                    var Update = new OrderUpdate();
                    Update.Order = CurrentOrder;

                    Uri uriUpdateCust = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(new { Update }),
                          Encoding.UTF8,
                          "application/json");
                    var response = ClientHTTP.PutAsync(uriUpdateCust, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        orderUpdate = new OrderUpdate();
                        //Check for messages
                        string orderContent = response.Content.ReadAsStringAsync().Result;


                        var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);

                        //orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent);
                        //check if empty result
                        if (orderUpdate != null && orderUpdate.Order != null)
                        {
                            CurrentOrder = orderUpdate.Order;
                            //lock the order auto locked
                            //LockOrder(orderUpdate, true);
                            string displayCustType = "";
                            if (CurrentOrder.Customer.CustomerType == "c")
                                displayCustType = "Cash";
                            if (CurrentOrder.Customer.CustomerType == "C")
                                displayCustType = "Cash";
                            SelectedCustomerNameBox = CurrentOrder.Customer.CustomerName + " " + Regex.Replace(CurrentOrder.Customer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + "Type: " + displayCustType + " " + CurrentOrder.Customer.CustomerCity + ", " + CurrentOrder.Customer.CustomerState + " " + CurrentOrder.Customer.CustomerZip + " ";
                            selCust.Clear();
                            selCust.Add(CurrentOrder.Customer);
                            //check for customer message
                            if (orderUpdate.Answers != null && orderUpdate.Answers.Count > 0)
                                return orderUpdate;
                            if (orderUpdate.Notifications.Count > 0)
                                return orderUpdate;


                        }
                        else
                            throw new Exception("Order was not locked");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is QuestionFaultExceptiom)
                {
                    //return message error
                }
                if (ex.InnerException is string)
                {

                    //return message error
                }
                // back out order failed to create
            }
            return orderUpdate;
    }

        private HttpClient clientHttp;
        Order ord;
        OrderUpdate orderUpdate;
        OrderDtl ordDtl;

        string selectedCustomerNameBox = "Select Customer";
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public string SelectedCustomerNameBox
        {
            get { return selectedCustomerNameBox; }
            set
            {
                if (selectedCustomerNameBox != value)
                {
                    selectedCustomerNameBox = value;
                    OnPropertyChanged("SelectedCustomerNameBox");
                }
            }
        }
        string selectedHeaderBox = "Header";
        public string SelectedHeaderBox
        {
            get { return selectedHeaderBox; }
            set
            {
                if (selectedHeaderBox != value)
                {
                    selectedHeaderBox = value;
                    OnPropertyChanged("SelectedHeaderBox");
                }
            }
        }
        string currentTotal = "Total";
        public string CurrentTotal
        {
            get { return currentTotal; }
            set
            {
                if (currentTotal != value)
                {
                    currentTotal = value;
                    OnPropertyChanged("CurrentTotal");
                }
            }
        }
        public Order CurrentOrder
        {
            get => this.ord;
            set
            {
                this.ord = value;
            }
        }
        public OrderDtl CurrentOrderDtl
        {
            get => this.ordDtl;
            set
            {
                this.ordDtl = value;
            }
        }
        private ObservableCollection<string> selectedTypes;
        public ObservableCollection<string> SelectedTypes
        {
            get { return selectedTypes; }
            set { selectedTypes = value; }
        }
        private ObservableCollection<OrderDtl> recent;
        public ObservableCollection<OrderDtl> Recent
        {
            get { return recent; }
            set { recent = value; }
        }
        internal List<string> CreateNewOrder()
        {
            try
            {
                Uri uriOrderSettings = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderSettings/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));

                var response = ClientHTTP.GetAsync(uriOrderSettings).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var Order = new Order();
                    var content = response.Content.ReadAsStringAsync().Result;

                    var currentSettings = JsonConvert.DeserializeObject<OrderSettings>(content);
                    CurrentOrder = SetDefaults(Order, currentSettings);
                    //var currentSettings = 

                    Uri uriOrderCreate = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));//"https://10.0.2.2:56883/Mobile/V1/Customers/"));//"https://visumaaron.fpsdns.com:56883/Mobile/V1/Customers/"));//"https://visumkirk.fpsdns.com:56883/Mobile/V1/Customers/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(new { Order }),
                          Encoding.UTF8,
                          "application/json");
                    var response2 = ClientHTTP.PostAsync(uriOrderCreate, stringContent).GetAwaiter().GetResult();
                    if (response2.IsSuccessStatusCode)
                    {
                        //JsonSerializerSettings settings = new JsonSerializerSettings();
                        //settings.Formatting = Formatting.Indented;
                        //settings.ContractResolver = new DictionaryAsArrayResolver();

                        var content2 = response2.Content.ReadAsStringAsync().Result;
                        string orderContent = response2.Content.ReadAsStringAsync().Result;


                        var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);

                        //orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent);
                        //check if empty result
                        if (orderUpdate != null && orderUpdate.Order != null)
                        {
                              CurrentOrder = orderUpdate.Order;
                            //lock the order auto locked
                            //LockOrder(orderUpdate, true);
                            string displayCustType = "";
                            if (CurrentOrder.Customer.CustomerType == "c")
                                displayCustType = "Cash";
                            if (CurrentOrder.Customer.CustomerType == "C")
                                displayCustType = "Cash";
                            SelectedCustomerNameBox = CurrentOrder.Customer.CustomerName + " " + Regex.Replace(CurrentOrder.Customer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + "Type: " + displayCustType + " " + CurrentOrder.Customer.CustomerCity + ", " + CurrentOrder.Customer.CustomerState + " " + CurrentOrder.Customer.CustomerZip + " ";
                            selCust.Clear();
                            selCust.Add(CurrentOrder.Customer);
                            //check for customer message
                            if (orderUpdate.Notifications.Count > 0)
                                return orderUpdate.Notifications;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
            }catch(Exception ex)
            {
                // order failed find out why\
                return null;
            }
            return new List<string>();
        }

        internal OrderUpdate UpdateCust(Customer cust)
        {
            try
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.Customer = cust;
                    var Update = new OrderUpdate();
                    Update.Order = CurrentOrder;

                    Uri uriUpdateCust = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(new { Update }),
                          Encoding.UTF8,
                          "application/json");
                    var response = ClientHTTP.PutAsync(uriUpdateCust, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        orderUpdate = new OrderUpdate();
                        //Check for messages
                        string orderContent = response.Content.ReadAsStringAsync().Result;


                        var settings = new JsonSerializerSettings { Converters = new JsonConverter[] { new JsonGenericDictionaryOrArrayConverter() } };

                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);
                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent, settings);

                        //orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(orderContent);
                        //check if empty result
                        if (orderUpdate != null && orderUpdate.Order != null)
                        {
                            CurrentOrder = orderUpdate.Order;
                            //lock the order auto locked
                            //LockOrder(orderUpdate, true);
                            string displayCustType = "";
                            if (CurrentOrder.Customer.CustomerType == "c")
                                displayCustType = "Cash";
                            if (CurrentOrder.Customer.CustomerType == "C")
                                displayCustType = "Cash";
                            SelectedCustomerNameBox = CurrentOrder.Customer.CustomerName + " " + Regex.Replace(CurrentOrder.Customer.CustomerPhone, @"(\d{3})(\d{3})(\d{4})", "($1)$2-$3") + Environment.NewLine + "Type: " + displayCustType + " " + CurrentOrder.Customer.CustomerCity + ", " + CurrentOrder.Customer.CustomerState + " " + CurrentOrder.Customer.CustomerZip + " ";
                            selCust.Clear();
                            selCust.Add(CurrentOrder.Customer);
                            //check for customer message
                            if (orderUpdate.Answers != null && orderUpdate.Answers.Count > 0)
                                return orderUpdate;
                            if (orderUpdate.Notifications.Count > 0)
                                return orderUpdate;


                        }
                        else
                            throw new Exception("Order was not locked");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is QuestionFaultExceptiom)
                {
                    //return message error
                }
                if (ex.InnerException is string)
                {

                    //return message error
                }
                // back out order failed to create
            }
            return orderUpdate;
        }

        internal bool VoidOrder()
        {
            try
            {
                if (CurrentOrder != null)
                {
                    CurrentOrder.OrderStatus = "V";
                    var Update = new OrderUpdate();
                    Update.Order = CurrentOrder;

                    Uri uriVoidOrder = new Uri(string.Format(DataManager.Settings.ApiUri + "Order/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(new { Update }),
                          Encoding.UTF8,
                          "application/json");
                    var response = ClientHTTP.PutAsync(uriVoidOrder, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        //exit order creation
                        return true;
                    }
                    else
                        throw new Exception("Order was not locked");
                }
            }
            catch (Exception ex)
            {
                if(ex.InnerException is QuestionFaultExceptiom)
                {
                    //return message error
                }
                if (ex.InnerException is string)
                {

                    //return message error
                }
                // back out order failed to create
            }
            return false;
        }
        internal void VoidDetailLine(OrderDtl  orderDetail)
        {
            
            try
            {
                if (CurrentOrder != null)
                {
                    var Update = new OrderDtlUpdate();
                    //Update.Order = CurrentOrder;
                    //if(Update.Order.OrderDtls.Contains(orderDetail))
                    //{
                    //    Update.Order.OrderDtls.Remove(orderDetail);
                    //    orderDetail.OrderDtlStatus = "V";
                    //    Update.Order.OrderDtls.Add(orderDetail);
                    //}
                    Uri uriUpdateDetail = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderDetail/"));
                    var stringContent = new StringContent(
                        JsonConvert.SerializeObject(Update),
                          Encoding.UTF8,
                          "application/json");
                    var response = ClientHTTP.PutAsync(uriUpdateDetail, stringContent).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        string orderContent = response.Content.ReadAsStringAsync().Result;
                        orderUpdate = JsonConvert.DeserializeObject<OrderUpdate>(content);
                        if (orderUpdate.Order != null)
                        {
                            CurrentOrder = orderUpdate.Order;
                        }
                    }
                    else
                        throw new Exception("Order was not locked");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is QuestionFaultExceptiom)
                {
                    //return message error
                }
                if (ex.InnerException is string)
                {

                    //return message error
                }
                // back out order failed to create
            }
        }
        private void ChangeHeader()
        {
            //if Customer number changes Update Order

            //if Tax Code changes Update Order

            //if Length Changes Update Order

            //ifOut Date Changes Update Order

            //if Due Date Update Order

            
        }
        private void LockOrder(OrderUpdate orderUpdate, bool isLocked)
        {
            try
            {
                if (orderUpdate.Order != null)
                {
                    Uri uriLockOrder = new Uri(string.Format(DataManager.Settings.ApiUri + "OrderLock / " + orderUpdate.Order.OrderNo + "/" + orderUpdate.Order.OrderNumberT + "/" + isLocked));
                    var response = ClientHTTP.GetAsync(uriLockOrder).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        CurrentOrder = orderUpdate.Order;
                    }
                    else
                        throw new Exception("Order was not locked");
                }
            }
            catch (Exception ex)
            {
                // back out order failed to create
            }
        }
        private Order SetDefaults(Order newOrder, OrderSettings currentSettings)
            {
                newOrder.OrderCustNo = currentSettings.Defaults.OrderCustNo;
                newOrder.OrderDDte = currentSettings.Defaults.OrderDDte;
                newOrder.OrderEDays = currentSettings.Defaults.OrderEDays;
                newOrder.OrderEventRate = currentSettings.Defaults.OrderEventRate;
                newOrder.OrderLength = currentSettings.Defaults.OrderLength;
                newOrder.OrderODte = currentSettings.Defaults.OrderODte;
                newOrder.OrderTaxCode = currentSettings.Defaults.OrderTaxCode;
                newOrder.OrderType = currentSettings.Defaults.OrderType;
                return newOrder;
            }
            }
    public class RentMerchItem
    {
        string description;
        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
            }
        }
        string equipmentID;
        public string EquipmentID
        {
            get => this.equipmentID;
            set
            {
                this.equipmentID = value;
            }
        }
        string store;
        public string Store
        {
            get => this.store;
            set { this.store = value; }
        }

        string own;
        public string Own 
        {
            get => this.own;
            set
            {
                this.own = value;
            }
        }
        string rent;
        public string Rent
        {
            get => this.Rent;
            set
            {
                this.Rent = value;
            }
        }
        string available;
        public string Available
        {
            get => this.available;
            set
            {
                this.available = value;
            }
        }
        string fullDetails;
        public string FullDetails
        {
            get => this.fullDetails;
            set
            {
                this.fullDetails = value;
            }
        }

        public RentMerchItem(string description, string equipmentID, string store, string own, string rent, string available)
        {
            Description = description;
            EquipmentID = equipmentID;
            Store = store;
            Own = own;
            Rent = rent;
            Available = available;
            FullDetails = description+ " " + equipmentID + " " + store + " " + own + " " + rent + " " + available + " ";
        }
    }
}
