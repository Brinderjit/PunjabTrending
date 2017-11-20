using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendingPunjab.DataLayer;
using TrendingPunjab.Intefaces;
using TrendingPunjab.Models;

namespace TrendingPunjab.Implementations
{
    public class ArticleCRUD : IArticle
    {
       private readonly DataLogic _data;

        public ArticleCRUD()
        {
            _data = new DataLogic();
        }
        public IEnumerable<ArticleModel> articleList()
        {
            return _data.getAllArticles();
        }

        public int CreateArticle(ArticleModel article)
        {
            int rowcount= _data.insertArticle(article);
            
           
           return rowcount;
        }

        public int Delete(long id)
        {
            int rowcount = _data.deleteArticle(id);
            return rowcount;
        }

        public ArticleModel ReadArticle(long id)
        {
            
            return _data.getlArticleByID(id); ;
        }

        public int UpdateArticle(ArticleModel article)
        {
            int rowcount = _data.updatetArticle(article);
            return rowcount;
        }
    }
}