using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrendingPunjab.Models
{
    public class VideoModel
    {
            public long v_id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string v_title { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string v_description { get; set; }
            public string publishDate { get; set; }
            public string v_url { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string v_category { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string type { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string tags { get; set; }
            public string thumbnail { get; set; }
             public long views { get; set; }
             public string playTime { get; set; }
        public long userId { get; set; }
    }
    public class VideoListModel
    {
       public VideoListModel()
        {
            videoList = new List<VideoModel>();
        }
       public IList<VideoModel> videoList { get; set; }
    }
}