using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrendingPunjab.Models
{
    public class ExceptionModel
    {
        public long id { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public string source { get; set; }

        public string  url{ get; set; }
        public string logdate { get; set; }
    }
}