using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace DBViewer.UI.WPF
{
    class Util
    {
        public static void ShowMessage(string text)
        {
            MessageBox.Show(text, "数据库跟踪", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool ToBool(object o)
        {
            if (IsNull(o))
            {
                return false;
            }

            return Convert.ToBoolean(o);
        }

        public static int ToInt(object o)
        {
            if (IsNull(o))
            {
                return 0;
            }

            return Convert.ToInt32(o);
        }
        public static DateTime ToDateTime(object o)
        {
            if (IsNull(o))
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(o);
        }

        private static bool IsNull(object o)
        {
            return o == null || Convert.IsDBNull(o);
        }


        internal static string ToString(object o)
        {
            if (IsNull(o))
            {
                return string.Empty;
            }

            return Convert.ToString(o);
        }
    }
}
