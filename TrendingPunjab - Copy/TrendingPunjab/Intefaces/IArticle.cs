using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendingPunjab.Models;
namespace TrendingPunjab.Intefaces
{
    public interface IArticle
    {
        int  CreateArticle(ArticleModel article);

        ArticleModel ReadArticle(long id);

        int UpdateArticle(ArticleModel article);

        int Delete(long id);

        IEnumerable<ArticleModel> articleList();
    }
}