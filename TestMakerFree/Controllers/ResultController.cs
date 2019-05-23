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
    public class ResultController : Controller
    {
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var sampleResults = new List<ResultViewModel>();
            sampleResults.Add(new ResultViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What do you value most in your life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });
            for (int i = 2; i <= 5; ++i)
            {
                sampleResults.Add(new ResultViewModel()
                {
                    Id = i,
                    QuizId = quizId,
                    Text = $"Sample Question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }
            return new JsonResult(sampleResults, new JsonSerializerSettings() { Formatting = Formatting.Indented });
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
