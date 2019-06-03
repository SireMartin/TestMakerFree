using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;
using TestMakerFree.Data;
using Mapster;

namespace TestMakerFree.Controllers
{
    public class QuestionController : BaseApiController
    {
        private ApplicationDbContext DbContext;

        public QuestionController(ApplicationDbContext argDbContext) : base(argDbContext)
        { }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.Where(x => x.Id == id).FirstOrDefault();
            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {id} has not been found"
                });
            }
            return new JsonResult(question.Adapt<QuizViewModel>(), JsonSettings);
        }

        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var question = model.Adapt<Data.Models.Question>();
            /*
            //override those properties that should be set from server-side only
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;*/
            //properties set from server-side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;
            //add the new question
            DbContext.Questions.Add(question);
            //persist the changes into the database
            DbContext.SaveChanges();
            //return the newly-created question to the client
            return new JsonResult(question.Adapt<QuestionViewModel>(), JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            var question = DbContext.Questions.Where(x => x.Id == model.Id).FirstOrDefault();
            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {model.Id} has not been found"
                });
            }
            //handle the update (without object mapping) by manually assigning the properties we want to add to the request
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            //properties set from server side
            question.LastModifiedDate = question.CreatedDate;
            //persist the changes into the database
            DbContext.SaveChanges();
            //return the updated Quiz to the client
            return new JsonResult(question.Adapt<QuestionViewModel>(), JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var question = DbContext.Questions.Where(x => x.Id == id).FirstOrDefault();
            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {id} has not been found"
                });
            }
            DbContext.Remove(question);
            DbContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = DbContext.Questions.Where(x => x.QuizId == quizId).ToArray();
            return new JsonResult(questions.Adapt<QuestionViewModel[]>(), JsonSettings);
        }
    }
}
