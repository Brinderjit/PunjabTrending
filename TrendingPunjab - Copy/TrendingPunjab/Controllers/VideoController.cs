using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrendingPunjab.DataLayer;
using TrendingPunjab.Models;

namespace TrendingPunjab.Controllers
{
    public class VideoController : Controller
    {
        DataLogic _data = new DataLogic();
        // GET: Video
        [HttpGet]
        public ActionResult SingleView()
        {
            VideoModel video = new VideoModel();
            IEnumerable<VideoModel> filteredList = new List<VideoModel>();
            VideoListModel vidlist = new VideoListModel();
            try
            {
                string id = Request.QueryString["vid"];
                if (id != null)
                {
                    long vid = Convert.ToInt64(id);
                    video = _data.getVideoById(vid);
                    if (video.v_url != null || video.v_url != "")
                    {
                        _data.incrementViews(vid, "video");
                    }
                    vidlist = _data.selectAllVideo();
                    filteredList = (from s in vidlist.videoList where s.v_category == video.v_category orderby s.v_id descending select s).Take(12).ToList();
                    ViewBag.upNext = filteredList;
                }
            }
            catch(Exception e)
            {
                ExceptionLogging.SendExcepToDB(e);
            }
            return View(video);
        }


    }
}