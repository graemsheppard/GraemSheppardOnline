using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GraemSheppardOnline.Code;
using GraemSheppardOnline.Services.MongoDBContext;
using GraemSheppardOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace GraemSheppardOnline.Controllers
{
    public class ArticlesController : Controller
    {
        [HttpPost]
        [Route("api/Articles/Post")]
        public async Task<IActionResult> Post(string data)
        {

            if (data != null)
            {
                var article = JsonSerializer.Deserialize<ArticleVM>(data);
                article.Title = article.Title.Trim();
                article.Content = article.Content.Trim();
                var regex = new Regex(@"\s+");
                article.Title = regex.Replace(article.Title, " ");
                article.Content = regex.Replace(article.Content, " ");

                var ctx = ContextManager.GetInstance().MongoContext;
                var updateBuilder = Builders<Article>.Update;
                var update = updateBuilder.Set(x => x.LastEdited, DateTime.UtcNow)
                                          .Set(x => x.Title, article.Title)
                                          .Set(x => x.Content, article.Content);

                await ctx.Articles.UpdateOneAsync(x => x.Id == article.Id, update);
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }



        
    }
}
