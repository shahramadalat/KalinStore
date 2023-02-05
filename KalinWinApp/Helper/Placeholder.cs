using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KalinWinApp.Helper
{
    public class Placeholder 
    {

        private TextBox textBox;
        private string text;
        public Placeholder(TextBox textBox,string text)
        {

            this.textBox = textBox;
            this.text = text;
            //if (textBox.Text == text)
            //{
            //    textBox.Text = string.Empty;
            //}

            //if (string.IsNullOrWhiteSpace(textBox.Text))
            //{
            //    textBox.Text = text;
            //}
        }

        public void RemovePlace()
        {
            if (textBox.Text == text)
            {
                textBox.Text = string.Empty;
            }
        }

        public void MakePlace()
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = text;
            }
        }
    }
}
