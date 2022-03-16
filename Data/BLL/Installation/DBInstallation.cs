using Data.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.BLL.Installation
{
    public class DBInstallation
    {
        public static string SqlFilePath { get; set; }

        public static void Run()
        {
            if (string.IsNullOrEmpty(SqlFilePath))
                throw new Exception("@'SqlFilePath' must not be null");

            string sql = File.ReadAllText(SqlFilePath);
            IEnumerable<string> commandStrings = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (DBContext db = new DBContext())
            {
                foreach (string commandString in commandStrings)
                {
                    if (!string.IsNullOrWhiteSpace(commandString.Trim()))
                    {
                        db.ExecuteNonQuery(commandString, CommandType.Text);
                    }
                }
            }
        }

        public static async Task RunAsync()
        {
            if (string.IsNullOrEmpty(SqlFilePath))
                throw new Exception("@'SqlFilePath' must not be null");

            string sql = File.ReadAllText(SqlFilePath);
            IEnumerable<string> commandStrings = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (DBContext db = new DBContext())
            {
                foreach (string commandString in commandStrings)
                {
                    if (!string.IsNullOrWhiteSpace(commandString.Trim()))
                    {
                        await db.ExecuteNonQueryAsync(commandString, CommandType.Text);
                    }
                }
            }
        }
    }
}
