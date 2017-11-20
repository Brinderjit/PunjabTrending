using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrendingPunjab.Models
{
    public class ArticleModel
    {
        
        public long articleId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "required")]
        public string content { get; set; }
        public string thumbnail { get; set; }
        public string images { get; set; }
        public long views { get; set; }
        public string createDate { get; set; }
        public long userid { get; set; }
    }
}