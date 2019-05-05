using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Airport
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //设置启动窗口和最小窗口大小
            ApplicationView.PreferredLaunchViewSize = new Size(450, 800);
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 600));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            //调用Azure推送添加到新的InitNotificationsAsync方法
            InitNotificationsAsync();
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {// 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(View.SplashPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
            //判断settings容器里面有没有"First"这个键
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("CurrentLanguage"))
            { //应用首次启动，必定不会含"First"这个键，让应用导航到GuidePage这个页面，GuidePage这个页面就是对应用的介绍啦
                ApplicationData.Current.LocalSettings.Values["CurrentLanguage"] = "null";
            }
            if (ApplicationData.Current.LocalSettings.Values["CurrentLanguage"] != null)
            {
               var strCurrentLanguage = ApplicationData.Current.LocalSettings.Values["CurrentLanguage"].ToString();
                if (strCurrentLanguage == "Chinese")
                {
                    ApplicationLanguages.PrimaryLanguageOverride = "zh-cn";
                }else if (strCurrentLanguage == "English")
                    ApplicationLanguages.PrimaryLanguageOverride = "en-us";
                else
                {
                    ApplicationLanguages.PrimaryLanguageOverride = "zh-cn";
                }
            }
            else
            {
                ApplicationLanguages.PrimaryLanguageOverride = "zh-cn";
            }
            InitializeComponent();

        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
        //Azure推送
        public static async void InitNotificationsAsync()
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            //获取本地应用设置容器
            //如果存在Push_Enable这个键再判断
            try
            {
                if (settings.Values.ContainsKey("Push_Enable"))
                {
                    var hub_name = "shadowsocks_push";
                    var DefaultListenSharedAccessSignature = "Endpoint=sb://shadowsocks.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=9O0/lbrxhrSWrViRUuvgfX897KhhvRXe8aBOdCALXNw=";
                    //注册推送通知
                    var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    var hub = new NotificationHub(hub_name, DefaultListenSharedAccessSignature);
                    //判断是否打开推送开关,如何打开则显示开并且注册推送通知,否则关闭
                    if ((bool)settings.Values["Push_Enable"] == true)
                    {//上传注册的通道
                        var result = await hub.RegisterNativeAsync(channel.Uri);
                    }
                    else
                    { //关闭通道
                        channel.Close();
                    }
                }
                else
                {//如果不存在Push_Enable这个键就说明默认没有打开接收通知，无需注册通知通道
                    settings.Values["Push_Enable"] = false;
                }
            }
            catch
            {/*
                var dialog = new MessageDialog("网络异常!请检查网络" );
                await dialog.ShowAsync();
                */
            }
            /*显示注册成功的id标识
            if (result.RegistrationId != null)
            {
                var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
            */
        }
  
    }
}