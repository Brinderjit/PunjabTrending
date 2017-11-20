using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrendingPunjab.Models;
namespace TrendingPunjab.DataLayer
{
   
    public class DataLogic
    {
        
        String strConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
       public List<long> getAdminList()
        {
            List<long> adminList = new List<long>();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getUserByRole";
            cmd.Parameters.Add("@role", SqlDbType.VarChar).Value = "admin";
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    adminList.Add(Convert.ToInt64(rdr["Userid"]));

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
            return adminList;
        }
        public int createUser(UserModel user)
        {
            int rowsEffected = 0;

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_CreateUser";
            cmd.Parameters.Add("@facebookid", SqlDbType.BigInt).Value = user.id;
            cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = user.first_name;
            cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = user.last_name;
            cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = user.gender;
            cmd.Parameters.Add("@locale", SqlDbType.VarChar).Value = user.locale;
            cmd.Parameters.Add("@link", SqlDbType.VarChar).Value = user.link;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = user.email ?? "null";
            cmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = user.middle_name;
            cmd.Parameters.Add("@identity", SqlDbType.VarChar).Value = user.identity;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@count";
            outPutParameter.SqlDbType = System.Data.SqlDbType.BigInt;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                rowsEffected = Convert.ToInt32(outPutParameter.Value);
            }
            catch (Exception ex)
            {
                rowsEffected = 0;
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return rowsEffected;
        }

        public ArticleModel getlArticleByID(long id)
        {
            ArticleModel article = new ArticleModel();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_GetArticleById";
            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   
                    article.articleId = Convert.ToInt32(rdr["articleId"]);
                    article.title = (string)rdr["title"];
                    article.thumbnail = (string)rdr["thumbnail"];
                    article.views = Convert.ToInt64(rdr["views"]);
                    article.content = (string)rdr["articleContent"];
                    article.userid = Convert.ToInt64(rdr["userid"]);
                    article.createDate = (string)rdr["createDate"];
                   
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
            return article;
        }
        public IEnumerable<ArticleModel> getAllArticles()
        {
            IList<ArticleModel> articleList = new List<ArticleModel>();
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_GetAllArticles";
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   ArticleModel article = new ArticleModel();
                    article.articleId = Convert.ToInt32(rdr["articleId"]);
                        article.title = (string)rdr["title"];
                    article.thumbnail = (string)rdr["thumbnail"];
                    article.views = Convert.ToInt64(rdr["views"]);
                    article.content = (string)rdr["articleContent"];
                    article.createDate = (string)rdr["createDate"];
                    article.userid = Convert.ToInt64(rdr["userid"]);
                    articleList.Add(article);
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
            return articleList;
        }
        public int deleteArticle(long id)
        {
            int rowsEffected = 0;

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_deleteArticle";
            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = id;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@count";
            outPutParameter.SqlDbType = System.Data.SqlDbType.BigInt;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                rowsEffected = Convert.ToInt32(outPutParameter.Value);
            }
            catch (Exception ex)
            {
                rowsEffected = 0;
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return rowsEffected;
        }
        public int updatetArticle(ArticleModel article)
        {
            int rowsEffected=0;

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_updateArticle";
            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = article.articleId;
            cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = article.title;
            cmd.Parameters.Add("@thumbnail", SqlDbType.VarChar).Value = article.thumbnail?? "null";
            cmd.Parameters.Add("@content", SqlDbType.VarChar).Value = article.content;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@count";
            outPutParameter.SqlDbType = System.Data.SqlDbType.BigInt;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                rowsEffected= Convert.ToInt32(outPutParameter.Value);
            }
            catch (Exception ex)
            {
                rowsEffected = 0;
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return rowsEffected;
        }
        public int insertArticle(ArticleModel article)
        {
            int rowsEffected = 0;

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_CreateArticle";
            cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = article.title;
            cmd.Parameters.Add("@thumbnail", SqlDbType.VarChar).Value = article.thumbnail ??"null";
            cmd.Parameters.Add("@content", SqlDbType.VarChar).Value = article.content;
            cmd.Parameters.Add("@userid", SqlDbType.BigInt).Value = article.userid;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@count";
            outPutParameter.SqlDbType = System.Data.SqlDbType.BigInt;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);

            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                rowsEffected = Convert.ToInt32(outPutParameter.Value);
            }
            catch (Exception ex)
            {
                rowsEffected = 0;
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return rowsEffected;
        }
        public  Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
        {
            System.Drawing.Bitmap bmpOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = lnWidth;
                int lnNewHeight = lnHeight;

                //*** If the image is smaller than a thumbnail just return it
             /*   if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)D:\GitRepos\TrendingPunjab\TrendingPunjab\DataLayer\DataLogic.cs
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }*/
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
            }

            return bmpOut;
        }
        public VideoListModel selectAllVideo()
        {
            VideoListModel videoList = new VideoListModel();
            string message = "";
         
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_Select_AllVideos";
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoModel vidObj = new VideoModel();
                    
                    // get the results of each column
                    vidObj.v_id = Convert.ToInt64(rdr["v_id"]);
                    vidObj.v_category = (string)rdr["categoryName"];
                    vidObj.publishDate = (string)rdr["publishDate"];
                    vidObj.type = (string)rdr["v_type"];
                    // print out the results
                    vidObj.v_title= (string)rdr["v_title"];
                    vidObj.v_description = (string)rdr["v_description"];
                    vidObj.v_url= (string)rdr["v_url"];
                    vidObj.views =Convert.ToInt64 ( rdr["views"]);
                    vidObj.thumbnail = (string)rdr["thumnail"];
                    vidObj.playTime = (string)rdr["playTime"];
                    vidObj.userId = Convert.ToInt64(rdr["userid"]);
                    videoList.videoList.Add(vidObj);

                }

            }
            catch (Exception ex)
            {
                message = "Error occurred";
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return videoList;
        }

        public IEnumerable<VideoModel> getVideoByCategory(string category)
        {
            IList <VideoModel> videoList = new List<VideoModel>();
            string message = "";
         
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getVideoByCategory";
            cmd.Parameters.Add("@category", SqlDbType.VarChar).Value =category;
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VideoModel vidObj = new VideoModel();

                    // get the results of each column
                    vidObj.v_id = Convert.ToInt64(rdr["v_id"]);
                    vidObj.v_category = (string)rdr["categoryName"];
                    vidObj.publishDate = (string)rdr["publishDate"];
                    vidObj.type = (string)rdr["v_type"];
                    // print out the results
                    vidObj.v_title = (string)rdr["v_title"];
                    vidObj.v_description = (string)rdr["v_description"];
                    vidObj.v_url = (string)rdr["v_url"];
                    vidObj.views = Convert.ToInt64(rdr["views"]);
                    vidObj.thumbnail = (string)rdr["thumnail"];
                    vidObj.playTime = (string)rdr["playTime"];
                    vidObj.userId = Convert.ToInt64(rdr["userid"]);

                    videoList.Add(vidObj);
                }

            }
            catch (SqlException ex)
            {
                message = "Error occurred";
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return videoList;
        }
        public List<SelectListItem> getCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string message = "";

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getCategory";
            cmd.Connection = con;
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Value = rdr["categoryId"].ToString(),
                        Text= rdr["categoryName"].ToString()
                    });
                }

            }
            catch (SqlException ex)
            {
                message = "Error occurred";
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return items;
        }
        public string getUniqueVideoName()
        {
            string name= "";
          
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getUniqueVideoName";
            cmd.Connection = con;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@uniqueName";
            outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
            outPutParameter.Size = 100;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                name = outPutParameter.Value.ToString();

            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return name;

        }
        public string getUniqueThumbnailName()
        {
            string name = "";

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getUniqueThumbnailName";
            cmd.Connection = con;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@uniqueName";
            outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
            outPutParameter.Size = 100;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                name = outPutParameter.Value.ToString();

            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return name;
        }
        public string getUniqueArticleThumbnailName()
        {
            string name = "";
         
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getUniqueArticleThumbnailName";
            cmd.Connection = con;
            SqlParameter outPutParameter = new SqlParameter();
            outPutParameter.ParameterName = "@uniqueName";
            outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
            outPutParameter.Size = 100;
            outPutParameter.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(outPutParameter);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                name = outPutParameter.Value.ToString();

            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return name;
        }
        
        public string incrementViews(long vid,string type)
        {
            string message = "";

            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_incrementViews";
            cmd.Parameters.Add("@vid", SqlDbType.BigInt).Value = vid;
            cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = type;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
              
            }
            catch (Exception ex)
            {
                message = "Error occurred";
               
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return message;
        }



        public string insertVideo(VideoModel vid)
        {
            string message = "";
 
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertVideo_Proc";
            cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = vid.v_title;
            cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = vid.v_category;
            cmd.Parameters.Add("@path", SqlDbType.VarChar).Value =vid.v_url;
            cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = vid.type;
            cmd.Parameters.Add("@description", SqlDbType.VarChar).Value =vid.v_description ;
            cmd.Parameters.Add("@tags", SqlDbType.VarChar).Value = vid.tags;
            cmd.Parameters.Add("@thumbnailUrl", SqlDbType.VarChar).Value = vid.thumbnail;
            cmd.Parameters.Add("@playTime", SqlDbType.VarChar).Value = vid.playTime;
            cmd.Parameters.Add("@userid", SqlDbType.BigInt).Value = vid.userId;
            SqlParameter outPutParameter = new SqlParameter();
             outPutParameter.ParameterName = "@count";
             outPutParameter.SqlDbType = System.Data.SqlDbType.BigInt;
             outPutParameter.Direction = System.Data.ParameterDirection.Output;
             cmd.Parameters.Add(outPutParameter);
            
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                message = outPutParameter.Value.ToString();
            }
            catch (Exception ex)
            {
                message = "Error occurred";
                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return message;
        }
        public VideoModel getVideoById(long vid)
        {
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_getVideoById";
            cmd.Parameters.Add("@vid", SqlDbType.BigInt).Value = vid;
            cmd.Connection = con;
            VideoModel vidObj = new VideoModel();
            try
            {
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                   

                    // get the results of each column
                    vidObj.v_id = Convert.ToInt64(rdr["v_id"]);
                    vidObj.v_category = (string)rdr["categoryName"];
                    vidObj.publishDate = (string)rdr["publishDate"];
                    vidObj.type = (string)rdr["v_type"];
                    // print out the results
                    vidObj.v_title = (string)rdr["v_title"];
                    vidObj.v_description = (string)rdr["v_description"];
                    vidObj.v_url = (string)rdr["v_url"];
                    vidObj.views = Convert.ToInt64(rdr["views"]);
                    vidObj.thumbnail = (string)rdr["thumnail"];
                    vidObj.playTime = (string)rdr["playTime"];
                    vidObj.userId = Convert.ToInt64(rdr["userid"]);


                }

            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);

            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return vidObj;
        }

    }
}