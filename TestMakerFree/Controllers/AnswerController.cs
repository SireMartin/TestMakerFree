using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var sampleAnswers = new List<AnswerViewModel>();
            sampleAnswers.Add(new AnswerViewModel()
            {
                Id = 1,
                QuestionId = questionId,
                Text = "Friends and Family",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });
            for (int i = 2; i <= 5; ++i)
            {
                sampleAnswers.Add(new AnswerViewModel()
                {
                    Id = i,
                    QuestionId = questionId,
                    Text = $"Sample Answer {0}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }
            return new JsonResult(sampleAnswers, new JsonSerializerSettings() { Formatting = Formatting.Indented });
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
