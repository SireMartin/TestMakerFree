using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var sampleQuestions = new List<QuestionViewModel>();
            sampleQuestions.Add(new QuestionViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What do you value most in life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });
            for (int i = 2; i <= 5; ++i)
            {
                sampleQuestions.Add(new QuestionViewModel()
                {
                    Id = i,
                    QuizId = quizId,
                    Text = $"Sample question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }
            return new JsonResult(sampleQuestions, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not implemented get (yet)");
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Content("Not implemented put (yet)");
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Content("Not implemented post (yet)");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Content("Not implemented delete (yet)");
        }
    }
}
