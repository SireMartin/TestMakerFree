using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.ViewModels;
using Newtonsoft.Json;
using TestMakerFree.Data;
using Mapster;

namespace TestMakerFree.Controllers
{
    public class QuizController : BaseApiController
    {
        public QuizController(ApplicationDbContext argDbContext) : base(argDbContext)
        {}

        //GET api/quiz/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {id} has not been found"
                });
            }
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        //GET api/quiz/latest
        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = DbContext.Quizzes.OrderByDescending(i => i.CreatedDate).Take(num).ToArray();
            return new JsonResult(latest.Adapt<QuizViewModel[]>(), JsonSettings);
        }

        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = DbContext.Quizzes.OrderBy(i => i.Title).Take(num).ToArray();
            return new JsonResult(byTitle.Adapt<QuizViewModel[]>(), JsonSettings);
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext.Quizzes.OrderBy(i => Guid.NewGuid()).Take(num).ToArray();
            return new JsonResult(random.Adapt<QuizViewModel[]>(), JsonSettings);
        }

        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }
            var quiz = new Data.Models.Quiz();
            //properties taken from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes; //until now not in the request
            //properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;
            //set a temp author as user id
            quiz.UserId = DbContext.Users.Where(x => x.UserName == "Admin").FirstOrDefault().Id;
            //add the new quiz
            DbContext.Quizzes.Add(quiz);
            //persist the changes in the DB
            DbContext.SaveChanges();
            //return the newly-created Quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }
            var quiz = DbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();
            //handle request for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {model.Id} has not been found"
                });
            }
            //handle the update (without object-mapping) by manually assigning the properties we want to accept from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes; //until now not in the request
            //properties set from server-side
            quiz.LastModifiedDate = DateTime.Now;
            //persist the changes in the DB
            DbContext.SaveChanges();
            //return the newly-created Quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the quiz from the database
            var quiz = DbContext.Quizzes.Where(x => x.Id == id).FirstOrDefault();
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {id} has not been found"
                });
            }
            //delete the object from the dbcontext
            DbContext.Quizzes.Remove(quiz);
            //persist the changes in the database
            DbContext.SaveChanges();
            //return an HTTP Status 200 (OK)
            return new OkResult();
        }
    }
}
