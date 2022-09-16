using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using Microsoft.Web.Administration;

namespace SurfsUp.Controllers
{
    public class BackUpController : Controller
    {
        internal void BtnBackup_Click()
        {
            string connectionString  = $"Server=(localdb)\\MSSQLLocalDB";

            SqlConnection conToDB = new SqlConnection(connectionString);

            conToDB.Open();
            string str = "USE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0];";
            string str1 = "BACKUP DATABASE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0] TO DISK = 'C:\\Backup\\BackupForDB.bak' WITH FORMAT,MEDIANAME = 'Z_SQLServerBackups',NAME = 'Full Backup of aspnet-SurfsUp';";
            SqlCommand cmd1 = new SqlCommand(str, conToDB);
            SqlCommand cmd2 = new SqlCommand(str1, conToDB);
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            conToDB.Close();
        }
    }
   
}
