using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using log4net;
using System.Configuration;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.DataAccess
{
    public class DBPersistence
    {
        //logger
        public static ILog logger = LogManager.GetLogger("app_log");

        //db connection
        public static MySqlConnection GetConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["mysql_connection"].ConnectionString;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(connectionString.Replace("password", "root"));
                LogSuccess("GetConnection", "database="+conn.Database);
            }
            catch(Exception e)
            {
                LogError("GetConnection", e.Message);
            }

            return conn;
        }

        //get all departments
        public static List<DepartmentModel> GetDepartments()
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();

            var sql = "SELECT * FROM departments ORDER BY name";

            try
            {
                using (MySqlConnection _conn = GetConnection())
                {
                    //open connection
                    _conn.Open();
                    //set command
                    using (MySqlCommand cmd = new MySqlCommand(sql, _conn))
                    {
                        //excute command and get data
                        using (MySqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    departments.Add(new DepartmentModel { 
                                        Id = int.Parse(dataReader["id"].ToString()),
                                        Name = dataReader["name"].ToString(),
                                        CreatedAt = dataReader["createdAt"].ToString(),
                                        UpdatedAt = dataReader["updatedAt"].ToString()
                                    });
                                }
                            }

                            //close the reader
                            dataReader.Close();
                        }
                        //dispose cmd
                        cmd.Dispose();
                    }
                    //close connection
                    _conn.Close();

                    //log
                    LogSuccess("GetDepartments", "records=" + departments.Count());
                }
            }
            catch(Exception e)
            {
                LogError("GetDepartments", e.Message);
            }
            return departments;
        }

        //log error
        public static void LogError(string refName, string error)
        {
            logger.Error("::ERROR - REF : " + refName + "::" + error);
        }

        public static void LogSuccess(string refName, string message)
        {
            logger.Info("::Success - REF : " + refName + "::" + message);
        }
    }
}