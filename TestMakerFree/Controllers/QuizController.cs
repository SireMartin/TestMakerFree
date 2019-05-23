using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.ViewModels;
using Newtonsoft.Json;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        //GET api/quiz/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new JsonResult(new QuizViewModel()
            {
                Id = id,
                Title = $"Sample quiz with id {id}",
                Description = "Not a real quiz: it's just a sample",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            }, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        //GET api/quiz/latest
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num = 10)
        {
            var sampleQuizzes = new List<QuizViewModel>();
            sampleQuizzes.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which Certia character are you?",
                Description = "Certia personality test",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });
            for (int i = 2; i <= num; ++i)
            {
                sampleQuizzes.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = $"Sample Quiz {i}",
                    Description = "This is a sample quiz",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }
            return new JsonResult(sampleQuizzes, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(x => x.Title), new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(x => Guid.NewGuid()), new JsonSerializerSettings() { Formatting = Formatting.Indented });
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
