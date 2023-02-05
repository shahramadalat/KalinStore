using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalinWinApp.Helper
{
    public class ExHandler
    {
        public static void CheckIfNullOrEmpty(Control[] controls, string[] placeholder)
        {
            foreach (Control control in controls)
            {
                if (string.IsNullOrEmpty(control.Text))
                {
                    throw new Exception("دڵنیابەرەوە لە تۆمارکردنی زانیاریەکان");
                }
                foreach (var item in placeholder)
                {
                    if (item == control.Text)
                    {
                        throw new Exception("دڵنیابەرەوە لە تۆمارکردنی زانیاریەکان");
                    }
                }
            }
        }
        public static void CheckWhithoutPlaceholder(Control[] controls)
        {
            foreach (Control control in controls)
            {
                if (string.IsNullOrEmpty(control.Text))
                {
                    throw new Exception("دڵنیابەرەوە لە تۆمارکردنی زانیاریەکان");
                }
            }
        }

        public static void IsIdSelected(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("هیچت هەڵنەبژاردوە، تکایە دووبارە هەوڵبدە");
            }
        }

        public static void StringHandler(string[] parameters)
        {
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new Exception("دڵنیابەرەوە لە تۆمارکردنی زانیاریەکان");
                }
            }
        }

    }
}
