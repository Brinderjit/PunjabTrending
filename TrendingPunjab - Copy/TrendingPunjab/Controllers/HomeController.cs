using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrendingPunjab.Models;
using TrendingPunjab.DataLayer;
using System.Diagnostics;
using NReco.VideoConverter;
using System.Drawing;
using System.Drawing.Imaging;

namespace TrendingPunjab.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly DataLogic _data;

       
        public HomeController()
        {
            
             _data = new DataLogic();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadMoreVideos(int size,string category)
        {
            IEnumerable<VideoModel> videoListByCategory = new List<VideoModel>();
            int modelCount = 0;
            try
            {
               
                if (category == "recent")
                {
                    VideoListModel list = new VideoListModel();
                    list = _data.selectAllVideo();
                    modelCount = list.videoList.Count();
                    videoListByCategory = list.videoList.OrderByDescending(p => p.v_id).Skip(size).Take(12);
                }
                else
                {
                    videoListByCategory = _data.getVideoByCategory(category).OrderByDescending(p => p.v_id).Skip(size).Take(12);
                    modelCount = _data.getVideoByCategory(category).Count();
                }

                if (videoListByCategory.Any())
                {
                    string modelString = RenderRazorViewToString("_Partial_LoadMore", videoListByCategory);

                    return Json(new { ModelString = modelString, ModelCount = modelCount });
                }
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
            }
            return Json(videoListByCategory);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public IEnumerable<VideoModel> getFilteredList(IList<VideoModel> vidlist ,int size,string category)
        {
            IEnumerable<VideoModel> filteredList = new List<VideoModel>();
           filteredList= (from s in vidlist where s.v_category == category orderby s.v_id descending select s).Take(size).ToList();
            return filteredList;
        }
        public ActionResult Index()
        {

            
            VideoListModel vidlist = new VideoListModel();
            try
            { 
                vidlist = _data.selectAllVideo();
                IEnumerable<VideoModel> filteredList = new List<VideoModel>();
                filteredList = (from s in vidlist.videoList select s).Take(6).ToList();
                ViewBag.recentUploads = filteredList;
                ViewBag.sportList = getFilteredList(vidlist.videoList, 8, "Sports");
                ViewBag.comedyList = getFilteredList(vidlist.videoList, 8, "Comedy");
                ViewBag.educationList = getFilteredList(vidlist.videoList, 8, "Education");
                ViewBag.gamingList = getFilteredList(vidlist.videoList, 8, "Gaming");
                ViewBag.Music = getFilteredList(vidlist.videoList, 8, "music");
                ViewBag.politics = getFilteredList(vidlist.videoList, 8, "News & Politics");
                ViewBag.social = getFilteredList(vidlist.videoList, 8, "Nonprofits & Activism");
                ViewBag.technology = getFilteredList(vidlist.videoList, 8, "Science & Technology");
                ViewBag.events = getFilteredList(vidlist.videoList, 8, "Travel & Events");
                ViewBag.autoAndVehicle = getFilteredList(vidlist.videoList, 8, "Autos & Vehicles");
                ViewBag.movies = getFilteredList(vidlist.videoList, 8, "Film & Animation");
                ViewBag.peopleAndBlogs = getFilteredList(vidlist.videoList, 8, "People & Blogs");
                ViewBag.animals = getFilteredList(vidlist.videoList, 8, "Pets & Animals");
                ViewBag.punjabi = getFilteredList(vidlist.videoList, 8, "Punjabi");
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
            }
            return View(vidlist);
        }
        public ActionResult SeeAllView()
        {
            IEnumerable<VideoModel> videoListByCategory = new List<VideoModel>();
            string category = Server.UrlDecode(Request.QueryString["category"]);
            try
            {


                if (category == "recent")
                {
                    VideoListModel list = new VideoListModel();
                    list = _data.selectAllVideo();
                    videoListByCategory = list.videoList.OrderByDescending(p => p.v_id).Take(12);
                }
                else
                {
                    videoListByCategory = _data.getVideoByCategory(category).OrderByDescending(p => p.v_id).Take(12);
                }
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);

            }
          
            ViewBag.Category = category;
            return View(videoListByCategory);   
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult uploads()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult upload()
        {
            
            ViewBag.categoryList = _data.getCategory();
            TempData["alertMessage"] = null;

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult upload(VideoModel vidInfo)
        {
           string userid;
            if(Session["role"]!=null)
            {
                if(Session["userid"]!=null)
                {
                    userid = (string)Session["userid"];
                    vidInfo.userId =Convert.ToInt64(userid);
                }
                try
                {
                    var videoFile = Request.Files[0];
                    var thumnailFile = Request.Files[1];
                    string postedFileName = null;
                    string withoutext;
                    string message = "";
                    string uniqueThumbnail = _data.getUniqueThumbnailName();
                    var ThumbnailuploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/default");
                    String resizedImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/resized");
                    string fullThumbnailUrl = ThumbnailuploadFilesDir + "\\" + uniqueThumbnail + ".jpeg";
                    String resizedImageFullPath = resizedImagePath + "\\" + uniqueThumbnail + ".jpeg";
                    if (vidInfo.type == "Sharing")
                    {
                        thumnailFile.SaveAs(fullThumbnailUrl);
                        Bitmap resizedThumnail = _data.CreateThumbnail(fullThumbnailUrl, 1200, 630);
                        vidInfo.playTime = "4.50";
                        vidInfo.thumbnail = "/Content/thumbnail/resized/" + uniqueThumbnail + ".jpeg";
                        message = _data.insertVideo(vidInfo);
                        if (message == "0")
                        {
                            resizedThumnail.Save(resizedImageFullPath, ImageFormat.Jpeg);
                            TempData["alertMessage"] = "video uploaded successfully";
                        }
                        else
                        {
                            message = "Video with same name already exists in database";
                            TempData["alertMessage"] = message;
                        }

                    }
                    else
                    {

                        if (videoFile != null)
                        {
                            string uniqueVideoName = _data.getUniqueVideoName();
                            string fileSavePath = null;
                            postedFileName = videoFile.FileName;
                            // Validate the uploaded file if you want like content length(optional)

                            // Get the complete file path
                            var VideouploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/Content/Videos");

                            if (!Directory.Exists(VideouploadFilesDir))
                            {
                                Directory.CreateDirectory(VideouploadFilesDir);
                            }
                            else if (!Directory.Exists(ThumbnailuploadFilesDir))
                            {
                                Directory.CreateDirectory(ThumbnailuploadFilesDir);
                            }

                            fileSavePath = Path.Combine(VideouploadFilesDir, videoFile.FileName);

                            videoFile.SaveAs(fileSavePath);
                            var ffProbe = new NReco.VideoInfo.FFProbe();
                            var videoInfo = ffProbe.GetMediaInfo(fileSavePath);
                            vidInfo.playTime = videoInfo.Duration.ToString();

                            thumnailFile.SaveAs(fullThumbnailUrl);
                            Bitmap resizedThumnail = _data.CreateThumbnail(fullThumbnailUrl, 320, 180);

                            // Save the uploaded file to "UploadedFiles" folder
                            if (!Directory.Exists(resizedImagePath))
                            {
                                Directory.CreateDirectory(resizedImagePath);
                            }

                            string inputfile, saveVideoPath, filargs;


                            //Get the file name without Extension
                            withoutext = Path.GetFileNameWithoutExtension(postedFileName);

                            //Input file path of uploaded image
                            inputfile = VideouploadFilesDir + "\\" + postedFileName;
                            string outputpath = VideouploadFilesDir + "\\ogg";
                            if (!Directory.Exists(outputpath))
                            {
                                Directory.CreateDirectory(VideouploadFilesDir);
                            }
                            //output file format in swf
                            saveVideoPath = outputpath + "\\" + uniqueVideoName + ".ogg";
                            vidInfo.v_url = "/Content/Videos/ogg/" + uniqueVideoName + ".ogg";
                            vidInfo.thumbnail = "/Content/thumbnail/resized/" + uniqueThumbnail + ".jpeg";
                            //  string thumbpath, thumbname;

                            //  thumbpath = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail");

                            //  thumbname = thumbpath +"\\" +withoutext + "%d" + ".jpg";
                            message = _data.insertVideo(vidInfo);
                            if (message == "0")
                            {
                                try
                                {


                                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                                    ffMpeg.ConvertMedia(inputfile, null, saveVideoPath, Format.ogg, new ConvertSettings()
                                    {
                                        CustomOutputArgs = " -b:v 2000k -bufsize 300k "
                                    });
                                    // ffMpeg.GetVideoThumbnail(inputfile, vidInfo.thumbnail);
                                    resizedThumnail.Save(resizedImageFullPath, ImageFormat.Jpeg);
                                    //File.Delete(inputfile);

                                }
                                catch (Exception ex)
                                {
                                    Response.Write(ex.Message);
                                    ExceptionLogging.SendExcepToDB(ex);
                                }


                                TempData["alertMessage"] = "video uploaded successfully";

                            }
                            else
                            {
                                message = "Video with same name already exists in database";
                                TempData["alertMessage"] = message;
                                return View(vidInfo);
                            }
                            //Start Converting






                        }
                    }
                }
                catch (Exception e)
                {
                    ExceptionLogging.SendExcepToDB(e);
                }
                ViewBag.categoryList = _data.getCategory();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           

            return View();
        }
    }
}