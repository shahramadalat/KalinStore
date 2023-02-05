using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace KalinWinApp.Database
{
    public class Connection
    {
        public static SqlConnection conn = new SqlConnection("Data Source=DESKTOP-TR9OH22\\SQLEXPRESS;Initial Catalog=KalinDB;Integrated Security=True");

    }
}
