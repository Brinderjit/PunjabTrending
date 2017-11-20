using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrendingPunjab.DataLayer;
using TrendingPunjab.Implementations;
using TrendingPunjab.Intefaces;
using TrendingPunjab.Models;

namespace TrendingPunjab.Controllers
{
    public class AuthenticateUserController : Controller
    {
        private readonly IUser _user;
       
        public AuthenticateUserController()
        {
            _user = new UserCRUD();
        }
        // GET: Login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult logIn(string accessToken,string status,string identity)
        {
            string message="hyihujikl";
            try
            {
                if (status.Equals("connected"))
                {

                    Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link,middle_name,email&access_token=" + accessToken);
                    HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

                   // Read the returned JSON object response
                    StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
                    string jsonResponse = string.Empty;
                    jsonResponse = userInfo.ReadToEnd();

                    // Deserialize and convert the JSON object to the Facebook.User object type
                    JavaScriptSerializer sr = new JavaScriptSerializer();
                    string jsondata = jsonResponse;
                    UserModel userProfile = sr.Deserialize<UserModel>(jsondata);
                    userProfile.identity =identity;
                    _user.createUser(userProfile);
                    //  Write the user data to a List
                     List<UserModel> currentUser = new List<UserModel>();
                    currentUser.Add(userProfile);
                    message = "Inside connected" + userProfile.first_name + " " + userProfile.middle_name + " " + userProfile.last_name;
                    if (Session["userName"] == null)
                    {
                        List<long> adminlist=_user.getAdminList();
                      
                        if (adminlist.Contains(Convert.ToInt64(userProfile.id)))
                        {
                            Session["role"] = "admin";
                        }

                        Session["userid"] = userProfile.id;
                        Session["userName"] = userProfile.first_name+" "+userProfile.middle_name+" "+userProfile.last_name;
                        //   Session["adminlist"] = adminlist;
                      //  RedirectToAction("Index", "Home");

                    }

                }
                else

                {
                    message = "inside unknown";
                    Session["userid"] = null;
                    Session["userName"] = null;
                    Session["role"] = null;
                    //RedirectToAction("Index", "Home");
                }
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
                message = e.Message;
            }
            return Json(new { someString = message });

        }
        public JsonResult logout(string accessToken, string status)
        {
            string message = "hyihujikl";
            try
            {
                if (status.Equals("connected"))
                {

                    Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link,middle_name,email&access_token=" + accessToken);
                    HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

                    // Read the returned JSON object response
                    StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
                    string jsonResponse = string.Empty;
                    jsonResponse = userInfo.ReadToEnd();

                    // Deserialize and convert the JSON object to the Facebook.User object type
                    JavaScriptSerializer sr = new JavaScriptSerializer();
                    string jsondata = jsonResponse;
                    UserModel userProfile = sr.Deserialize<UserModel>(jsondata);
                    _user.createUser(userProfile);
                    //  Write the user data to a List
                    List<UserModel> currentUser = new List<UserModel>();
                    currentUser.Add(userProfile);
                    message = "Inside connected";
                    if (Session["userName"] == null)
                    {
                        List<string> adminlist;
                        adminlist = new List<string>();
                        adminlist.Add("1629544897108123");
                        if (adminlist.Contains(userProfile.id.ToString()))
                        {
                            Session["role"] = "admin";
                        }

                        Session["userid"] = userProfile.id;
                        Session["userName"] = userProfile.first_name + " " + userProfile.middle_name + " " + userProfile.last_name;
                        //   Session["adminlist"] = adminlist;
                        //  RedirectToAction("Index", "Home");

                    }

                }
                else

                {
                    message = "inside unknown";
                    Session["userid"] = null;
                    Session["userName"] = null;
                    Session["role"] = null;
                    //RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
                message = e.Message;
            }
            return Json(new { someString = message });

        }
    }
}