using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Airport.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        ResourceContext resourceContext = ResourceContext.GetForViewIndependentUse();
        ResourceMap resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
        public HelpPage()
        {
            this.InitializeComponent();
            
            var resourceValue_Router = resourceMap.GetValue("Router", resourceContext);
            var resourceValue_Game = resourceMap.GetValue("Game", resourceContext);
            ObservableCollection<Data> listData = new ObservableCollection<Data>();
            listData.Add(new Data() { title = "Windows" });
            listData.Add(new Data() { title = "Android" });
            listData.Add(new Data() { title = "iOS" });
            listData.Add(new Data() { title = "MacOS" });
            listData.Add(new Data() { title = resourceValue_Router.ValueAsString });
            listData.Add(new Data() { title = resourceValue_Game.ValueAsString });
            Help_List.ItemsSource = listData;
            
        }
        //数据实体类
        public class Data
        {
            public string title { get; set; }
            public string picOpen { get; set; }
            public string picClose { get; set; }
        }

        bool temp = false;
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //把列表的选项转换为集合的实体类对象
            Data data = Help_List.SelectedItem as Data;
              
            //循环显示下拉箭头
            temp = !temp;
            if (temp == false)
            {
                //     open.Visibility = Visibility.Visible;
                //    close.Visibility = Visibility.Collapsed;
            }
            else
            {
           //     open.Visibility = Visibility.Collapsed;
            //    close.Visibility = Visibility.Visible;
            }
            
            //获取资源集中的内容
            var resourceValue_Router = resourceMap.GetValue("Router", resourceContext);
            var resourceValue_Game = resourceMap.GetValue("Game", resourceContext);
            //判断用户选中并操作具体事宜
            foreach (var item in e.AddedItems) {
                var n = ( item as Data).title;
                switch ((item as Data).title) {
                    case "Windows":
                        ;
                        break;
                    case "Android":
                        ;
                        break;
                    case "iOS":
                        ;
                        break;
                    case "Linux":
                        ;
                        break;
                    case "MacOS":
                        ;
                        break;
                    case "Router":
                    case "路由器":
                        ;
                        break;
                    case "Game":
                    case "游戏端":
                        ;
                        break;

                }
                   
            }

          
        }
    }
}
