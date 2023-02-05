using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace KalinWinApp.Helper
{
    public class DecimalValidator
    {
        public void onlyNumber(TextBox txt)
        {
            
            if (txt.Text.Length > 0)
            {
                Regex regex = new Regex(@"^[0-9]+$");
                Match match = regex.Match(txt.Text);
                if (!match.Success)
                {
                    MessageBox.Show("تەنها دەتوانی ژمارە داخڵ بکەی");
                    txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                    txt.SelectionStart = txt.Text.Length;
                    txt.SelectionLength = 0;
                }
            }
        }
        public void validate(TextBox txt)
        {
            if (txt.Text.Length > 0)
            {
                Regex regex = new Regex(@"^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
                Match match = regex.Match(txt.Text);
                if (!match.Success)
                {
                    MessageBox.Show("تەنها دەتوانی ژمارە داخڵ بکەی");
                    txt.Text = txt.Text.Remove(txt.Text.Length - 1, 1);
                    txt.SelectionStart = txt.Text.Length;
                    txt.SelectionLength = 0;
                }

            }
        }

    }
}
