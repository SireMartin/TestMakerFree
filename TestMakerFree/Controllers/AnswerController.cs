using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.ViewModels;
using TestMakerFree.Data;
using Mapster;

namespace TestMakerFree.Controllers
{
    public class AnswerController : BaseApiController
    {
        private ApplicationDbContext DbContext;

        public AnswerController(ApplicationDbContext argDbContext) : base(argDbContext)
        {}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers.Where(x => x.Id == id).FirstOrDefault();
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = $"no Answer ID {id} has been found"
                });
            }
            return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
        }

        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }
            //map the viewmodel to the model
            var answer = model.Adapt<Data.Models.Answer>();
            /*
            //override the properties that should be set from the server-side only
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;*/
            //properties from the server-side
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;
            //add the answer
            DbContext.Answers.Add(answer);
            DbContext.SaveChanges();
            return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }
            var answer = DbContext.Answers.Where(x => x.Id == model.Id).FirstOrDefault();
            if (answer == null)
            {
                return NotFound(new { Error = $"No answer ID {model.Id} has been found" });
            }
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;
            answer.LastModifiedDate = answer.CreatedDate;
            DbContext.SaveChanges();
            return new JsonResult(answer.Adapt<AnswerViewModel>(), JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = DbContext.Answers.Where(x => x.Id == id).FirstOrDefault();
            if (answer == null) { return new StatusCodeResult(500); }
            DbContext.Remove(answer);
            DbContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = DbContext.Answers.Where(x => x.QuestionId == questionId).ToArray();
            return new JsonResult(answers.Adapt<AnswerViewModel[]>(), JsonSettings);
        }
    }
}
