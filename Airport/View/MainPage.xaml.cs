using Microsoft.Services.Store.Engagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources.Core;
using Windows.Networking.PushNotifications;
using Windows.Services.Store;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using static Airport.Model.Account;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Airport.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string AccoutJson = "";
        JObject JSON;
        Rootobject account;
        String _Nations = "US";
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        ResourceContext resourceContext = ResourceContext.GetForViewIndependentUse();
        ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

        private StoreContext context = null;
        StoreProduct subscriptionStoreProduct;
        // Assign this variable to the Store ID of your subscription add-on.
        private string subscriptionStoreId = "9NLWR4XNTLJ3";
        private string productID = "";
        //默认没有订阅任何产品
        private bool userOwnsSubscription = false;
        bool isPro = false;

        //获取本地应用设置容器
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void Help_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpPage));
           // await Launcher.LaunchUriAsync(new Uri("http://520ss.xyz/user/announcement"));
        }
        private void Notice_Click(object sender, RoutedEventArgs e)
        { //点击公告后就将红点取消
            RedPoint.Visibility = Visibility.Collapsed;           
            //同时在系统配置中将红点取消下次显示
            settings.Values["Tip_RedPoint_Time"] = (int)JSON["tip_redpoint_time"];
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {//把Split的打开状态调整为相反
            MenuView.IsPaneOpen = !MenuView.IsPaneOpen;
        }

        private void Setting_Tapped(object sender, TappedRoutedEventArgs e)
        {//把打开状态调整为相反
            SettingsView.IsPaneOpen = !SettingsView.IsPaneOpen;
            MenuView.IsPaneOpen = !MenuView.IsPaneOpen;
        }

        private void Exit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void More_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://publisher/?name=MEP Studio"));
        }

        private async void Assess_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?Productid=9P72L2SPB7RF"));
        }

        private void Get_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Get_ss_true();
            if (_Nations == "US") {
                Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/美国.png"));
            }
            
            /*
            Get_Enable.ad_finashed = false;

            if (Get_Enable.ad_once == true)
            {
                Get_Enable.ad_once = false;
                var messageDig = new MessageDialog("完成广告任务获得账号，并同意遵守账号使用条例，是否同意并开始？");

                var btn_OK = new UICommand("同意并开始");
                var btn_NO = new UICommand("放弃");

                messageDig.Commands.Add(btn_OK);
                messageDig.Commands.Add(btn_NO);

                //展示窗口，获取按钮是否退出  
                var result = await messageDig.ShowAsync();
                //如果是确定退出就直接让应用程序退出  
                if (null != result && result.Label == "同意并开始")
                {
                    Frame.Navigate(typeof(TaskPage));
                }
            }
            else
            {
                Frame.Navigate(typeof(TaskPage));
            }
            */
        }

        bool IsError = false;  
        async void Get_ss_true()
        {
            // await Analysis_Json();
            Loading.IsActive = true;

            string Uri = "http://s.moreplay.cn/apps/ss/ss.json";
            Uri uri = new Uri(Uri);
            HttpClient client = new HttpClient();
            try
            {
                AccoutJson = await client.GetStringAsync(uri);
            }
            catch 
            { //  System.Diagnostics.Debug.WriteLine("ErrorCode: " + e);
                var NetError = resourceMap.GetValue("NetError", resourceContext);
                var messageDig = new MessageDialog(NetError.ValueAsString);
                //展示窗口，获取按钮是否退出  
                var result = await messageDig.ShowAsync();
                IsError = true;
            }
            if (IsError == false)
            {
                JObject JSON = JObject.Parse(AccoutJson);

                JsonSerializer json = JsonSerializer.Create();
                account = json.Deserialize<Rootobject>(new JsonTextReader(new StringReader(AccoutJson)));

                // 将Json对象解析为Json字符串
                //     ToJsonObject(AccoutJson);

                //如何订阅版账号
                if (isPro == true)
                {
                    ss_pro();
                    //  JArray ss_pro_array = JArray.Parse(JSON["ss_pro"].ToString());
                    //  JArray jlist = JArray.Parse(JSON["ss"].ToString());
                }
                else ss();

                    //获取JSON中SS账号的具体数量
                //    int Quantity_ss = (int)JSON["quantity_ss"];

                    //  System.Diagnostics.Debug.WriteLine(e.Message);
                    //将SS账号组成数组
            //        JArray jlist = JArray.Parse(JSON["ss"].ToString());

             //       Random rnd = new Random();
                    //从以下范围内随机取值
              //      int r = rnd.Next(0, Quantity_ss);
                    /*
                    jlist.ToObject<List<Items>>();
                    List<Items> items = ((JArray)jlist).Select(x => new Items
                    {
                        Ip = (string)x["ip"],
                        Port = (string)x["port"],
                        PW = (string)x["pw"]
                    }).ToList();
                    */
                    /*
                    string ss_Str = string.Join("", jlist[r]);//数组转成字符串
                    ss_Str = ss_Str.TrimEnd('"');
                    ss_Str = ss_Str.TrimStart('"');
                    Regex r1 = new Regex("\"\"");
                    ss_Str = r1.Replace(ss_Str, "\",\"", Quantity_ss - 2);
                    ss_Str = "{\"" + ss_Str + "\"}";
                    //将随机获取到的账号数组转化为json
                    JObject ss_Arr = (JObject)JsonConvert.DeserializeObject(ss_Str);

                IP.Text = (string)ss_Arr["ip"];
                Port.Text = (string)ss_Arr["port"];
                Password.Text = (string)ss_Arr["pw"];
                Encryption.Text = (string)ss_Arr["jm"];
                */
                Loading.IsActive = false;
                List.Visibility = Visibility.Visible;
                title.Visibility = Visibility.Collapsed;
                    /*    }
                        catch {
                            var messageDig = new MessageDialog("网络异常，请检查网络！");
                            //展示窗口，获取按钮是否退出  
                            var result = await messageDig.ShowAsync();
                        }
                        */               
            }
        }
        void ss_pro() {
            Random rnd = new Random();
            switch (_Nations)
            {
                case "US" :                   
                    //从获取JSON中SS账号的具体数量以下范围内随机取值
                    int random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.US.US_num));
                    IP.Text = account.ss_pro.US.US_list[random_num].ip;
                    Port.Text = account.ss_pro.US.US_list[random_num].port;
                    Password.Text = account.ss_pro.US.US_list[random_num].pw;
                    Encryption.Text = account.ss_pro.US.US_list[random_num].jm;
                    break;
                case "UK":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.UK.UK_num));
                    IP.Text = account.ss_pro.UK.UK_list[random_num].ip;
                    Port.Text = account.ss_pro.UK.UK_list[random_num].port;
                    Password.Text = account.ss_pro.UK.UK_list[random_num].pw;
                    Encryption.Text = account.ss_pro.UK.UK_list[random_num].jm;
                    break;
                case "HK":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.HK.HK_num));
                    IP.Text = account.ss_pro.HK.HK_list[random_num].ip;
                    Port.Text = account.ss_pro.HK.HK_list[random_num].port;
                    Password.Text = account.ss_pro.HK.HK_list[random_num].pw;
                    Encryption.Text = account.ss_pro.HK.HK_list[random_num].jm;
                    break;
                case "ZH":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.ZH.ZH_num));
                    IP.Text = account.ss_pro.ZH.ZH_list[random_num].ip;
                    Port.Text = account.ss_pro.ZH.ZH_list[random_num].port;
                    Password.Text = account.ss_pro.ZH.ZH_list[random_num].pw;
                    Encryption.Text = account.ss_pro.ZH.ZH_list[random_num].jm;
                    break;
                case "JP":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.JP.JP_num));
                    IP.Text = account.ss_pro.JP.JP_list[random_num].ip;
                    Port.Text = account.ss_pro.JP.JP_list[random_num].port;
                    Password.Text = account.ss_pro.JP.JP_list[random_num].pw;
                    Encryption.Text = account.ss_pro.JP.JP_list[random_num].jm;
                    break;
                case "KR":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss_pro.KR.KR_num));
                    IP.Text = account.ss_pro.KR.KR_list[random_num].ip;
                    Port.Text = account.ss_pro.KR.KR_list[random_num].port;
                    Password.Text = account.ss_pro.KR.KR_list[random_num].pw;
                    Encryption.Text = account.ss_pro.KR.KR_list[random_num].jm;
                    break;
            }         
        }

        void ss()
        {
            Random rnd = new Random();
            switch (_Nations)
            {
                case "US":
                    //从获取JSON中SS账号的具体数量以下范围内随机取值
                    int random_num = rnd.Next(0, Convert.ToInt32(account.ss.US.US_num));
                    IP.Text = account.ss.US.US_list[random_num].ip;
                    Port.Text = account.ss.US.US_list[random_num].port;
                    Password.Text = account.ss.US.US_list[random_num].pw;
                    Encryption.Text = account.ss.US.US_list[random_num].jm;
                    break;
                case "UK":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss.UK.UK_num));
                    IP.Text = account.ss.UK.UK_list[random_num].ip;
                    Port.Text = account.ss.UK.UK_list[random_num].port;
                    Password.Text = account.ss.UK.UK_list[random_num].pw;
                    Encryption.Text = account.ss.UK.UK_list[random_num].jm;
                    break;
                case "HK":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss.HK.HK_num));
                    IP.Text = account.ss.HK.HK_list[random_num].ip;
                    Port.Text = account.ss.HK.HK_list[random_num].port;
                    Password.Text = account.ss.HK.HK_list[random_num].pw;
                    Encryption.Text = account.ss.HK.HK_list[random_num].jm;
                    break;
                case "ZH":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss.ZH.ZH_num));
                    IP.Text = account.ss.ZH.ZH_list[random_num].ip;
                    Port.Text = account.ss.ZH.ZH_list[random_num].port;
                    Password.Text = account.ss.ZH.ZH_list[random_num].pw;
                    Encryption.Text = account.ss.ZH.ZH_list[random_num].jm;
                    break;
                case "JP":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss.JP.JP_num));
                    IP.Text = account.ss.JP.JP_list[random_num].ip;
                    Port.Text = account.ss.JP.JP_list[random_num].port;
                    Password.Text = account.ss.JP.JP_list[random_num].pw;
                    Encryption.Text = account.ss.JP.JP_list[random_num].jm;
                    break;
                case "KR":
                    random_num = rnd.Next(0, Convert.ToInt32(account.ss.KR.KR_num));
                    IP.Text = account.ss.KR.KR_list[random_num].ip;
                    Port.Text = account.ss.KR.KR_list[random_num].port;
                    Password.Text = account.ss.KR.KR_list[random_num].pw;
                    Encryption.Text = account.ss.KR.KR_list[random_num].jm;
                    break;
            }
        }

        private void Clear_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TaskPage));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //判断settings容器里面有没有"Tip_RedPoint_Time"这个键
            if (!settings.Values.ContainsKey("Tip_RedPoint_Time"))
            {
                settings.Values["Tip_RedPoint_Time"] = 0;
            }

            string Uri = "http://s.moreplay.cn/apps/ss/config.json";
            Uri uri = new Uri(Uri);
            HttpClient client = new HttpClient();
            try
            {
                var ControlJson = await client.GetStringAsync(uri);
                JSON = JObject.Parse(ControlJson);
                //ApplicationVersion
              //  string AppVersion = (string)JObject.Parse(ControlJson)["appversion"];
                //显示公告
                Notice.Text = "公告：\n" + (string)JObject.Parse(ControlJson)["notice"];

              //  bool Force_IsUpdate = (bool)JObject.Parse(ControlJson)["force_isupdate"];

                /*
                //判断是否完成广告任务
                if (Get_Enable.ad_finashed == true)
                {
                    Get_ss_true();
                }
                else
                {
                */
                    //初始化账号显示文本
                    IP.Text = "";
                    Port.Text = "";
                    Password.Text = "";
                    Encryption.Text = "";
               // }

                if ((int)JSON["tip_redpoint_time"] != (int)settings.Values["Tip_RedPoint_Time"])
                {
                    RedPoint.Visibility = Visibility.Visible;
                }
            }catch
            {
            }
            //判断是否打开推送开关,如何打开则显示开关
            if ((bool)settings.Values["Push_Enable"] == true)
            {
                Push_Switch.IsOn = true;
            }
        }
       
        private void Push_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    settings.Values["Push_Enable"] = true;
                }
                else
                {
                    settings.Values["Push_Enable"] = false;
                }
            }
        }

        private void Channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge://Badge通知
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(args.BadgeNotification);
                    break;
                case PushNotificationType.Tile: //Tile通知
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(args.TileNotification);
                    break;
                case PushNotificationType.TileFlyout: //TileFlyout通知

                    break;
                case PushNotificationType.Toast:  //Toast通知
                    ToastNotificationManager.CreateToastNotifier().Show(args.ToastNotification);
                    break;
            }
        }
        private async void Feedback_Tapped(object sender, TappedRoutedEventArgs e)
        {//调用系统级反馈
            var launcher = StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private async void Home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://520ss.xyz"));     
        }
        int n = 0;
        //切换订阅用户问题
        private void SwitchPro_Click(object sender, RoutedEventArgs e)
        {    
            if (n == 0)
            {
                free.Visibility = Visibility.Collapsed;
                pro.Visibility = Visibility.Visible;
                btn_pro_bg.Visibility = Visibility.Visible;
                isPro = true;
                n += 1;
            }else {
                free.Visibility = Visibility.Visible;
                pro.Visibility = Visibility.Collapsed;
                btn_pro_bg.Visibility = Visibility.Collapsed;
                isPro = false;
                n -= 1;
            }          
        }
       /* *
        * 以下为按钮特效
        * */
        private void Start_PointerMoved(object sender, PointerRoutedEventArgs e)
        {//鼠标移动进按键区域显示灰色
            btn_click_bg.Visibility = Visibility.Visible;
        }
        private void Start_PointerExited(object sender, PointerRoutedEventArgs e)
        {//鼠标移动出按键区域不显示灰色
            btn_click_bg.Visibility = Visibility.Collapsed;
        }
        private void Start_Holding(object sender, HoldingRoutedEventArgs e)
        {//按下按键区域显示灰色
            btn_click_bg.Visibility = Visibility.Visible;
        }
        private void Start_KeyDown(object sender, KeyRoutedEventArgs e)
        { //松下按键区域不显示灰色
            btn_click_bg.Visibility = Visibility.Collapsed;
        }


        private void Switch_Click(object sender, RoutedEventArgs e)
        {          
            ObservableCollection<data> listData = new ObservableCollection<data>();
            listData.Add(new data() { title = "US", picUrl = "ms-appx:///Assets/Images/prefix/美国.png" });
            listData.Add(new data() { title = "JP", picUrl = "ms-appx:///Assets/Images/prefix/日本.png" });
            listData.Add(new data() { title = "HK", picUrl = "ms-appx:///Assets/Images/prefix/香港.png" });
            listData.Add(new data() { title = "ZH", picUrl = "ms-appx:///Assets/Images/prefix/中国.png" });
            listData.Add(new data() { title = "UK", picUrl = "ms-appx:///Assets/Images/prefix/英国.png" });
            Nations.ItemsSource = listData;
          //  Nations.DataContext = listData;//设置DataContext
        }
        private void Nations_ItemClick(object sender, ItemClickEventArgs e)
        {       
            dynamic clickedItem = e.ClickedItem;
            switch (clickedItem.title) {
                case "US":
                    // Nations_img.Source=new ImageBrush("ms-appx:///Assets/Images/prefix/美国.png");
                    Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/美国.png"));
                    _Nations = "US";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                    break;
                case "JP":
                    Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/日本.png"));
                    _Nations = "JP";
                    break;
                case "HK":
                    Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/香港.png"));
                    _Nations = "HK";
                    break;
                case "ZH":
                    Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/中国.png"));
                    _Nations = "ZH";
                    break;
                case "UK":
                    Nations_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/prefix/英国.png"));
                    _Nations = "UK";
                    break;
            }
        }
        public class data
        {
            public string title { get; set; }
            public string picUrl { get; set; }
        }
        private async void SubscriptionMore_Tapped(object sender, TappedRoutedEventArgs e)
        { // Here you load the resource you need
            var resourceValue = resourceMap.GetValue("SubscriptMore", resourceContext);
            var messageDig = new MessageDialog(resourceValue.ValueAsString);         
            messageDig.Commands.Add(new UICommand("Yes"));
            messageDig.Commands.Add(new UICommand("No"));
            //展示窗口，获取按钮是否退出  
            var result = await messageDig.ShowAsync();
            //如果是确定退出就直接让应用程序退出  
            if (null != result && result.Label == "Yes")
            {
                await Launcher.LaunchUriAsync(new Uri("http://520ss.xyz"));
            }
        }
        private async void Subscription_Tapped(object sender, TappedRoutedEventArgs e)
        {// Here you load the resource you need
            var resourceValue = resourceMap.GetValue("SubscriptAccountInfo", resourceContext);
            var messageDig = new MessageDialog(resourceValue.ValueAsString);
            messageDig.Commands.Add(new UICommand("Yes"));
            messageDig.Commands.Add(new UICommand("No"));
            //展示窗口，获取按钮是否退出  
            var result = await messageDig.ShowAsync();
            //如果是确定退出就直接让应用程序退出  
            if (null != result && result.Label == "Yes")
            {
                Loading.IsActive = true; productID = subscriptionStoreId;
                // 购买加载项的一系列操作
                await SetupSubscriptionInfoAsync();
            }else
            {
                Loading.IsActive = false;
            }
        }
        /*
        private async void RemoveAD_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Here you load the resource you need
            var resourceValue = resourceMap.GetValue("RemoveADInfo", resourceContext);
            var messageDig = new MessageDialog(resourceValue.ValueAsString);
            messageDig.Commands.Add(new UICommand("Yes"));
            messageDig.Commands.Add(new UICommand("No"));
            //展示窗口，获取按钮是否退出  
            var result = await messageDig.ShowAsync();
            //如果是确定退出就直接让应用程序退出  
            if (null != result && result.Label == "Yes")
            {
                Loading.IsActive = true;
            }
        }
        */
        // This is the entry point method for the example.检查加载项信息，是否有可试用
        public async Task SetupSubscriptionInfoAsync()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            //检查用户是否有订阅的许可证传给userOwnsSubscription值
            userOwnsSubscription = await CheckIfUserHasSubscriptionAsync();
            if (userOwnsSubscription)
            {
                Loading.IsActive = false;
                var AlreadySubscript = resourceMap.GetValue("AlreadySubscript", resourceContext);
                var messageDig = new MessageDialog(AlreadySubscript.ValueAsString);
                //展示窗口，获取按钮是否退出  
                var result = await messageDig.ShowAsync();
                // 解锁所有加载项订阅的特性功能
                return;
            }

            //获取订阅信息.传给subscriptionStoreProduct值。
            subscriptionStoreProduct = await GetSubscriptionProductAsync();
            if (subscriptionStoreProduct == null)
            {
                Loading.IsActive = false;
                var NoSubscript = resourceMap.GetValue("NoSubscript", resourceContext);
                var messageDig = new MessageDialog(NoSubscript.ValueAsString);
                //展示窗口，获取按钮是否退出  
                var result = await messageDig.ShowAsync();
                return;
            }

            //检查第一个SKU是否是试用版，并通知客户试用版可用。
            //如果试用可用，Skus数组将始终具有两个可购买的SKU，以及
            // first one is the trial. Otherwise,第一个是审判。否则，该数组将只有一个SKU。
            StoreSku sku = subscriptionStoreProduct.Skus[0];
            if (sku.SubscriptionInfo.HasTrialPeriod)
            {
                //如果存在试用
                //您可以在这里向客户显示订阅购买信息。你可以使用
                // sku.SubscriptionInfo.BillingPeriod and sku.SubscriptionInfo.BillingPeriodUnit
                //提供续约详情。
            }
            else
            {
                //不存在试用
                //您可以在这里向客户显示订阅购买信息。你可以使用
                // sku.SubscriptionInfo.BillingPeriod and sku.SubscriptionInfo.BillingPeriodUnit
                //提供续约详情。

            }

            // Prompt the customer to purchase the subscription.
            await PromptUserToPurchaseAsync();
        }

        //检查用户是否有订阅的许可证
        private async Task<bool> CheckIfUserHasSubscriptionAsync()
        {
            StoreAppLicense appLicense = await context.GetAppLicenseAsync();
            //检查客户是否具有订阅权限。
            foreach (var addOnLicense in appLicense.AddOnLicenses)
            {
                StoreLicense license = addOnLicense.Value;
                if (license.SkuStoreId.StartsWith(productID))
                {
                    if (license.IsActive)
                    {
                        // The expiration date is available in the license.ExpirationDate property.                      
                        return true;
                        //客户有订阅的许可证。
                    }
                }
            }
            //客户没有订阅的许可证。
            return false;
        }

        //查找产品订阅信息
        private async Task<StoreProduct> GetSubscriptionProductAsync()
        {
            //加载此应用程序的可销售外接程序，并检查试用是否仍然存在
            //这个客户可以使用。如果他们以前获得过审判，他们就不会。
            //能够再次获得试用，Store..Skus属性将
            //仅包含一个SKU。
            StoreProductQueryResult result =
                await context.GetAssociatedStoreProductsAsync(new string[] { "Durable" });

            if (result.ExtendedError != null)
            {
                Loading.IsActive = false;
                var SubscriptError = resourceMap.GetValue("SubscriptError", resourceContext);
                var messageDig1 = new MessageDialog(SubscriptError.ValueAsString);
                //展示窗口，获取按钮是否退出  
                var result1 = await messageDig1.ShowAsync();
                return null;
            }
            //查找表示订阅的产品。
            foreach (var item in result.Products)
            {
                StoreProduct product = item.Value;
                if (product.StoreId == productID)
                {
                    return product;
                }
            }
            return null;
        }

        //购买订阅产品的结果。
        private async Task PromptUserToPurchaseAsync()
        {
            //请求购买订阅产品。如果有试用期，将提供试用期。
            //给客户。否则，将提供非审判SKU。
            StorePurchaseResult result = await subscriptionStoreProduct.RequestPurchaseAsync();
            // 捕获错误消息的操作，如果任何国家。
            string extendedError = string.Empty;
            if (result.ExtendedError != null)
            {
                extendedError = result.ExtendedError.Message;
                //错误代码
            }
            switch (result.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    //显示一个UI来确认客户已经购买了您的订阅
                    //并解锁订阅的特性。
                    Loading.IsActive = false;
                    var SuccessSubscript = resourceMap.GetValue("SuccessSubscript", resourceContext);
                    var messageDig1 = new MessageDialog(SuccessSubscript.ValueAsString);
                    //展示窗口，获取按钮是否退出  
                    var result1 = await messageDig1.ShowAsync();
                    break;

                case StorePurchaseStatus.NotPurchased:
                    Loading.IsActive = false;
                    var CancelSubscript = resourceMap.GetValue("CancelSubscript", resourceContext);
                    var messageDig2 = new MessageDialog(CancelSubscript.ValueAsString);
                    //展示窗口，获取按钮是否退出  
                    var result2 = await messageDig2.ShowAsync();
                    break;

                case StorePurchaseStatus.ServerError:
                case StorePurchaseStatus.NetworkError:
                    Loading.IsActive = false;
                    var NetErrorSubFail = resourceMap.GetValue("NetErrorSubFail", resourceContext);
                    var messageDig3 = new MessageDialog(NetErrorSubFail.ValueAsString);
                    //展示窗口，获取按钮是否退出  
                    var result3 = await messageDig3.ShowAsync();
                    break;

                case StorePurchaseStatus.AlreadyPurchased:
                    Loading.IsActive = false;
                    var AlreadySubscript = resourceMap.GetValue("AlreadySubscript", resourceContext);
                    var messageDig4 = new MessageDialog(AlreadySubscript.ValueAsString);
                    //展示窗口，获取按钮是否退出  
                    var result4 = await messageDig4.ShowAsync();
                    break;
            }
        }

        private void Share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }
        //设置想要共享的内容
        private void ShareRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();
            DataRequest request = args.Request;
            request.Data.Properties.Title = "分享";
            request.Data.SetText("我发现了一款科学上网的软件，可以免费访问Youtube、Facebook等网站哦！分享给你.");
            //flash.jpg是示例代码中Asssets文件夹中的图片，可以将其改为你自己的图片
            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Others/wxgzh.jpg")));
            deferral.Complete();
        }

        private async void Chinese_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["CurrentLanguage"] = "Chinese";
            var messageDig = new MessageDialog("切换成功，重启软件生效！");
            //展示窗口，获取按钮是否退出  
            var result = await messageDig.ShowAsync();
        }

        private async void English_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["CurrentLanguage"] ="English";
            var messageDig = new MessageDialog("Switch Successful, Restart Software Effective");
            //展示窗口，获取按钮是否退出  
            var result = await messageDig.ShowAsync();
        }
    }
}