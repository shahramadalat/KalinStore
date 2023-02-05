using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.Helper
{
    public class ExHandlerMessage
    {
        public static void SingleCheckIfNullOrEmpty(string text, string message)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception(message);
            }
        }
        public static void MultipleCheckIfNullOrEmpty(string[] text, string message)
        {
            foreach (var item in text)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new Exception(message);
                }
            }
        }

        public static void DatagridRowCount(DataGridView dataGridView)
        {
            if (dataGridView.Rows.Count<=0)
            {
                throw new Exception("هیچ تۆمارکتدنێک بۆنی نیە لە لیستەکەدا");
            }
        }

        public static void PerEachMessage(Dictionary<string,string> keyValuePairs)
        { 
            foreach(var pair in keyValuePairs)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    throw new Exception(pair.Value);
                }
            }
        }

      
    }
}
