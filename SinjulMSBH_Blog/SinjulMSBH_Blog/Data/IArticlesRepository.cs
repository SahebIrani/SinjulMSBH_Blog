using SinjulMSBH_Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinjulMSBH_Blog.Data
{
    public interface IArticlesRepository
    {
        Task<List<Article>> GetAll();
        Task<List<Article>> GetLatest(int num);
        Task<Article> GetOne(int id);
        void Add(Article article);
        void Remove(Article article);
        Task SaveChanges();
    }
}
