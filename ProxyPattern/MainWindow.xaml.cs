using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProxyPattern
{


    public interface ICache
    {
        List<string> GetValue(string text);
        void SetData(string data);
    }

    public class FakeRepo
    {
        public static List<string> GetDatas()
        {
            var result = File.ReadAllText(@"~/../../../Files/Notes.txt").Split('\n');
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Remove(result[i].Length - 2, 2);
            }
            return result.ToList();
        }
    }
    public class CacheProxy : ICache
    {
        private List<string> CachedConfiguration;
        public CacheProxy()
        {
            CachedConfiguration = FakeRepo.GetDatas();
        }
        public List<string> GetValue(string key)
        {
            var List = new List<string>();
            foreach (var item in CachedConfiguration)
            {
                if (List.Count <= 10)
                {
                    if (item.StartsWith(key))
                    {
                        List.Add(item);
                    }
                }
                else
                {
                    break;
                }
            }
            return List;
        }

        public void SetData(string data)
        {
            CachedConfiguration.Insert(0, data);
        }
    }

    public partial class MainWindow : Window
    {
        ICache cache = new CacheProxy();
        List<string> Datas = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = Textbox.Text;
            if (text != string.Empty)
            {
                Datas = cache.GetValue(text);
                Listbox.ItemsSource = null;
                Listbox.ItemsSource = Datas;
            }
            else
            {
                Listbox.ItemsSource = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = Textbox.Text;
            cache.SetData(data);
        }
    }
}
