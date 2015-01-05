using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace DrawNotePad.Commons
{
    //pages navigation and pass parameters
    public static class NavigationHelper
    {
        //页面要传递的键值对集合参数
        //the list of parameters that need be passed
        private static List<KeyValuePair<string, object>> ListParam = new List<KeyValuePair<string, object>>();

        //通过uri跳转页面, navigate to another page by uri
        public static void Navigate(NavigationService ns, string path)
        {
            ns.Navigate(new Uri(path, UriKind.Relative));
        }


        //可通过uri后面带参数传递   uri ? 参数=值
        //navigate to another page by uri and pass parameter and value
        public static void Navigate(NavigationService ns, string path, string param, string value)
        {
            ns.Navigate(new Uri(string.Format("{0}?{1}={2}", path, param, value), UriKind.Relative));
        }

        //跳转页面并通过键值对传值
        //set value pair and  navigate to another page by uri 
        public static void NavigateExt(NavigationService ns, string path, string param, object value)
        {
            SetValue(param, value);
            ns.Navigate(new Uri(path, UriKind.Relative));
        }

        //传两个键值对
        //set value pairs and  navigate to another page by uri 
        public static void NavigateExt(NavigationService ns, string path, string param, object value, string param2, object value2)
        {
            SetValue(param, value);
            SetValue(param2, value2);
            ns.Navigate(new Uri(path, UriKind.Relative));
        }

        //返回上一页面
        public static bool NavigateGoBack(NavigationService ns)
        {
            return NavigateGoBack(ns, false);
        }

        //返回上一页面并传递参数
        // back to pervious page
        public static bool NavigateGoBack(NavigationService ns, bool force)
        {
            if (force)
            {
                ns.GoBack();
                return true;
            }
            if (ns.CanGoBack)
            {
                ns.GoBack();
                return true;
            }
            return false;
        }

        //返回上一页面并传递键值对参数
        //set value pairs and back to pervious page
        public static bool NavigateGoBackExt(NavigationService ns, string param, object value)
        {
            SetValue(param, value);
            return NavigateGoBack(ns);
        }

        //获取各种类型的参数方法
        //methods that get parameters:
        public static bool NavigationExtGetBoolValue(string key)
        {
            object obj2 = NavigationExtGetObjValue(key);
            bool result = false;
            if (obj2 == null)
            {
                return false;
            }
            return (bool.TryParse(obj2.ToString(), out result) && result);
        }

        public static int NavigationExtGetIntValue(string key)
        {
            object obj2 = NavigationExtGetObjValue(key);
            int result = 0;
            if ((obj2 != null) && int.TryParse(obj2.ToString(), out result))
            {
                return result;
            }
            return 0;
        }

        public static object NavigationExtGetObjValue(string key)
        {
            KeyValuePair<string, object> item = new KeyValuePair<string, object>();
            foreach (KeyValuePair<string, object> pair2 in ListParam)
            {
                if (pair2.Key.IsEqual(key))
                {
                    item = pair2;
                    break;
                }
            }
            ListParam.Remove(item);
            return item.Value;
        }

        public static string NavigationExtGetStringValue(string key)
        {
            object obj2 = NavigationExtGetObjValue(key);
            if (obj2 == null)
            {
                return string.Empty;
            }
            return obj2.ToString();
        }

        public static T NavigationExtGetValue<T>(string key) where T : class
        {
            object obj2 = NavigationExtGetObjValue(key);
            if (obj2 is T)
            {
                return (obj2 as T);
            }
            return default(T);
        }

        public static int NavigationQueryGetIntValue(NavigationContext nc, string param)
        {
            string s = NavigationQueryGetStringValue(nc, param);
            int result = 0;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            return 0;
        }

        public static string NavigationQueryGetStringValue(NavigationContext nc, string param)
        {
            try
            {
                return nc.QueryString[param];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //设置键值对集合的值
        //set value pairs
        private static void SetValue(string key, object value)
        {
            int index = -1;
            foreach (KeyValuePair<string, object> pair in ListParam)
            {
                if (pair.Key.IsEqual(key))
                {
                    index = ListParam.IndexOf(pair);
                    break;
                }
            }
            if (index >= 0)
            {
                ListParam.RemoveAt(index);
            }
            else if (index == -1)
            {
                ListParam.Add(new KeyValuePair<string, object>(key, value));
            }
        }
    }
}
