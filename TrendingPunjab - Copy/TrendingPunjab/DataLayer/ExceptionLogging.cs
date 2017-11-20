using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using context = System.Web.HttpContext;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TrendingPunjab.Models;

namespace TrendingPunjab.DataLayer
{
    public static class ExceptionLogging
    {
        private static String exepurl;
        static SqlConnection con;
       static string constr;
        private static void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["conString"].ToString();
            con = new SqlConnection(constr);
            con.Open();

        }
        public static void SendExcepToDB(Exception exdb)
        {
           
            connection();
            exepurl = context.Current.Request.Url.ToString();
            SqlCommand com = new SqlCommand("ExceptionLoggingToDataBase", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
            com.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
            com.Parameters.AddWithValue("@ExceptionURL", exepurl);
            com.Parameters.AddWithValue("@ExceptionSource", exdb.StackTrace.ToString());
            com.ExecuteNonQuery();
            con.Close();
            com.Dispose();


        }
        public static List<ExceptionModel> getExceptionList()
        {
            List<ExceptionModel> exList = new List<ExceptionModel>();
           
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getAllExceptions";
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ExceptionModel ex = new ExceptionModel();
                    ex.id = Convert.ToInt64(rdr["Logid"]);
                    ex.message = rdr["ExceptionMsg"].ToString();
                    ex.type = rdr["ExceptionType"].ToString();
                    ex.source = rdr["ExcetionSource"].ToString();
                    ex.url = rdr["ExceptionURL"].ToString();
                    ex.logdate = rdr["Logdate"].ToString();
                    exList.Add(ex);
                       
                }

            }
            catch (SqlException ex)
            {

                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return exList;
        }

    }
    
}