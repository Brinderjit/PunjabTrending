using System;
using System.Drawing.Imaging;
using System.Web.Mvc;
using TrendingPunjab.DataLayer;
using TrendingPunjab.Models;
using TrendingPunjab.Intefaces;
using TrendingPunjab.Implementations;
using System.IO;
using System.Drawing;

namespace TrendingPunjab.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        private readonly IArticle _article;
        private readonly DataLogic _data;
        public ArticleController()
        {
            _data = new DataLogic();
            _article = new ArticleCRUD();
        }
        [HttpGet]
        public ActionResult uploadArticleView()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [ActionName("UploadArticle")]
        public ActionResult uploadArticleView(ArticleModel article)
        {
            string userid;
            if (Session["role"] != null)
            {
                if (Session["userid"] != null)
                {
                    userid = (string)Session["userid"];
                    article.userid =Convert.ToInt64( userid);
                }
                int rowcount;
                try
                {

                    var thumnailFile = Request.Files[1];
                    if (ModelState.IsValid)
                    {


                        string uniqueThumbnailName = _data.getUniqueArticleThumbnailName();
                        var ThumbnailuploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/default");
                        String resizedImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/resized");
                        string fullThumbnailUrl = ThumbnailuploadFilesDir + "\\" + uniqueThumbnailName + ".jpeg";
                        String resizedImageFullPath = resizedImagePath + "\\" + uniqueThumbnailName + ".jpeg";
                        article.thumbnail = "/Content/thumbnail/resized/" + uniqueThumbnailName + ".jpeg";
                        thumnailFile.SaveAs(fullThumbnailUrl);
                        Bitmap resizedThumnail = _data.CreateThumbnail(fullThumbnailUrl, 320, 180);

                        rowcount = _article.CreateArticle(article);
                        ViewBag.message = rowcount;
                        if (rowcount == 0)
                        {
                            return View("uploadArticleView", article);
                        }

                        resizedThumnail.Save(resizedImageFullPath, ImageFormat.Jpeg);
                        if (System.IO.File.Exists(fullThumbnailUrl))
                        {
                            System.IO.File.Delete(fullThumbnailUrl);
                        }
                    }
                    else
                    {
                        return View("uploadArticleView", article);
                    }
                }
                catch (Exception e)
                {
                    ExceptionLogging.SendExcepToDB(e);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View("uploadArticleView", new ArticleModel());
        }
       
        public ActionResult articleListView()
        {

            return View(_article.articleList());
        }
        public ActionResult articleDetails(int id)
        {
            ArticleModel article = new ArticleModel();
            if (id != 0)
            {
                article = _article.ReadArticle(id);
                _data.incrementViews(id, "article");
            }
            else
            {
                return RedirectToAction("articleListView", "Article");
            }
            return View(article);
        }
        [HttpGet]
        public ActionResult updateArticle(int id)
        {
            string userid;
            ArticleModel article = new ArticleModel();
            if (Session["role"] != null)
            {
                if (Session["userid"] != null)
                {
                    userid = (string)Session["userid"];

                }

                if (id != 0)
                {
                    article = _article.ReadArticle(id);

                }
                else
                {
                    return RedirectToAction("articleListView", "Article");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


                return View(article);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateArticle(ArticleModel article)
        {

            string userid;

            if (Session["role"] != null)
            {
                if (Session["userid"] != null)
                {
                    userid = (string)Session["userid"];

                }
                try
                {


                    if (ModelState.IsValid)
                    {
                        var thumnailFile = Request.Files[1];
                        string uniqueThumbnailName = _data.getUniqueThumbnailName();
                        var ThumbnailuploadFilesDir = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/default");
                        String resizedImagePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/thumbnail/resized");
                        string fullThumbnailUrl = ThumbnailuploadFilesDir + "\\" + uniqueThumbnailName + ".jpeg";
                        String resizedImageFullPath = resizedImagePath + "\\" + uniqueThumbnailName + ".jpeg";
                        article.thumbnail = "/Content/thumbnail/resized/" + uniqueThumbnailName + ".jpeg";
                        thumnailFile.SaveAs(fullThumbnailUrl);
                        Bitmap resizedThumnail = _data.CreateThumbnail(fullThumbnailUrl, 320, 180);
                        if (System.IO.File.Exists(fullThumbnailUrl))
                        {
                            System.IO.File.Delete(fullThumbnailUrl);
                        }
                        ViewBag.message = _article.UpdateArticle(article);
                        resizedThumnail.Save(resizedImageFullPath, ImageFormat.Jpeg);
                    }
                }
                catch (Exception e)
                {
                    ExceptionLogging.SendExcepToDB(e);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult deleteArticle (int id)
        { string userid;
            ArticleModel article = new ArticleModel();
            if (Session["role"] != null)
            {
                if (Session["userid"] != null)
                {
                    userid = (string)Session["userid"];

                }
                ViewBag.message = _data.deleteArticle(id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("articleListView", "Article");
           
        }
    }
}