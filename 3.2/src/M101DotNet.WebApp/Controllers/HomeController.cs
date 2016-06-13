using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MongoDB.Driver;
using M101DotNet.WebApp.Models;
using M101DotNet.WebApp.Models.Home;
using MongoDB.Bson;

namespace M101DotNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var blogContext = new BlogContext();
            // XXX WORK HERE
            // find the most recent 10 posts and order them
            // from newest to oldest

            var builders = Builders<Post>.Sort;
            var sort = builders.Descending(x => x.CreatedAtUtc);
            var last10Posts = await blogContext.Posts
                .Find(new BsonDocument())
                .Sort(sort)
                .Limit(10)               
                .ToListAsync();

            var model = new IndexModel
            {
                RecentPosts = last10Posts
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // Insert the post into the posts collection

            var post = Map(model);

            await blogContext.Posts.InsertOneAsync(post);   

            return RedirectToAction("Post", new { id = post.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Post(string id)
        {
            var blogContext = new BlogContext();

            // XXX WORK HERE
            // Find the post with the given identifier

            var builder = Builders<Post>.Filter;
            var filter = builder.Eq(x => x.Id, new ObjectId(id));
            var post = await blogContext.Posts.Find(filter).SingleAsync();

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PostModel
            {
                Post = post
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {
            var blogContext = new BlogContext();

            // XXX WORK HERE
            // Find all the posts with the given tag if it exists.
            // Otherwise, return all the posts.
            // Each of these results should be in descending order.

            var builder = Builders<Post>.Filter;
            var filter = builder.Where(x => x.Tags.Contains(tag));
            var posts = await blogContext.Posts
                .Find(filter)
                .SortByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
            
            if(posts.Count == 0)
                posts = await blogContext.Posts
                    .Find(new BsonDocument())
                    .SortByDescending(x => x.CreatedAtUtc)
                    .ToListAsync();

            return View(posts);
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }

            var blogContext = new BlogContext();
            // XXX WORK HERE
            // add a comment to the post identified by model.PostId.
            // you can get the author from "this.User.Identity.Name"

            var builder = Builders<Post>.Filter;
            var filter = builder.Eq(x => x.Id, new ObjectId(model.PostId));
            var post = await blogContext.Posts
                .Find(filter)
                .SingleAsync();

            var comments = post.Comments.ToList();
            comments.Add(new Comment()
            {
                Author = User.Identity.Name,
                Content = model.Content,
                CreatedAtUtc = DateTime.Now              
            });

            await blogContext.Posts.UpdateOneAsync(
                Builders<Post>.Filter.Eq(x => x.Id, new ObjectId(model.PostId)),
                Builders<Post>.Update.Set(x => x.Comments, comments));

            return RedirectToAction("Post", new { id = model.PostId });
        }

        private Post Map(NewPostModel model)
        {
            var tags = new List<string>();

            var tagsFromInput = model.Tags.Split(',');

            foreach (var tag in tagsFromInput)
            {
                tags.Add(tag.ToString());
            }

            var post = new Post()
            {
                Id = ObjectId.GenerateNewId(),
                Title = model.Title,
                Content = model.Content,
                Tags = tags,
                CreatedAtUtc = DateTime.Now,
                Comments = new List<Comment>(),
                Author = this.User.Identity.Name
            };

            return post;
        }
    }
}