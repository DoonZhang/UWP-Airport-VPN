using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static Airport.View.SplashPage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Airport.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TaskPage : Page
    {
        string ad_url_1;
        string ad_url_2;
        public TaskPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Web_AD_1.Navigate(new Uri("http://s.moreplay.cn/index.php?c=list"));

            try
            {
                string Uri = "http://s.moreplay.cn/Apps/ss/Control.json";
                Uri uri = new Uri(Uri);
                HttpClient client = new HttpClient();
                string ControlJson = await client.GetStringAsync(uri);
                JObject JSON = JObject.Parse(ControlJson);
                string ad_info_1 = (string)JSON["ad_info1"];
                string ad_info_2 = (string)JSON["ad_info2"];
                AD_info_1.Text = ad_info_1;
                AD_info_2.Text = ad_info_2;
                string ad_img_url_1 = (string)JSON["ad_img_url1"];
                string ad_img_url_2 = (string)JSON["ad_img_url2"];
                AD_img_1.Source = new BitmapImage(new Uri(ad_img_url_1));
                AD_img_2.Source = new BitmapImage(new Uri(ad_img_url_2));
                ad_url_1 = (string)JSON["ad_url1"];
                ad_url_2 = (string)JSON["ad_url2"];
                if (ad_url_1 != null)
                {
                    Loading.Visibility = Visibility.Collapsed;
                }
                if (ad_url_2 != null)
                {
                    Loading2.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                var messageDig = new MessageDialog("网络异常，请检查是否使用代理网络获取账号或无网络！");
                //展示窗口，获取按钮是否退出  
                var result = await messageDig.ShowAsync();
                Frame.GoBack();
            }

        }
        private async void AD_1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(ad_url_1));
            Frame.GoBack();

            Get_Enable.ad_finashed = true;
        }
        private async void AD_2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(ad_url_2));
            Frame.GoBack();
            Get_Enable.ad_finashed = true;
        }

        private void Web_AD_1_Loaded(object sender, RoutedEventArgs e)
        {
            Loading2.Visibility = Visibility.Collapsed;

        }

    }
}
