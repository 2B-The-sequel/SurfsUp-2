using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Data.SqlClient;
using Microsoft.Web.Administration;
using NuGet.ContentModel;
using System.Text;

namespace SurfsUp.Controllers
{
    public class BackUpController : Controller
    {
        public async Task<string> SqlBackupServer()
        {
            string connectionString  = $"Server=(localdb)\\MSSQLLocalDB";

            SqlConnection conToDB = new SqlConnection(connectionString);

            conToDB.Open();
            string str = "USE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0];";
            string str1 = "BACKUP DATABASE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0] TO DISK = 'C:\\Backup\\BackupForDB.bak' ";
            SqlCommand cmd1 = new SqlCommand(str, conToDB);
            SqlCommand cmd2 = new SqlCommand(str1, conToDB);
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            conToDB.Close();
            return "Backup Completed";
        }

        public async Task<string> SqlImportServer()
        {
            string connectionString = $"Server=(localdb)\\MSSQLLocalDB";
            SqlConnection conToDB = new SqlConnection(connectionString);

            conToDB.Open();
            string UseMaster = "USE master";
            string Alter1 = @"ALTER DATABASE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0] SET Single_User WITH Rollback Immediate";
            string str1 = "RESTORE database [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0] FROM DISK = 'C:\\Backup\\BackupForDB.bak' ";
            string Alter2 = @"ALTER DATABASE [aspnet-SurfsUp-62C345C1-8594-471B-957A-82E86E937AC0] SET Multi_User";


            SqlCommand UseMasterCommand = new SqlCommand(UseMaster, conToDB);
            SqlCommand Alter1Cmd = new SqlCommand(Alter1, conToDB);
            SqlCommand cmd2 = new SqlCommand(str1, conToDB);
            SqlCommand Alter2Cmd = new SqlCommand(Alter2, conToDB);

            
            UseMasterCommand.ExecuteNonQuery();
            Alter1Cmd.ExecuteNonQuery();

            StringBuilder errorMessages = new StringBuilder();
            try
            {
                cmd2.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
            }
            Alter2Cmd.ExecuteNonQuery();
            conToDB.Close();
            return "Import completed";
        }
    }
   
}
