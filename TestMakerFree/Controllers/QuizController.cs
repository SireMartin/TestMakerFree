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
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private ApplicationDbContext dbContext;

        public QuizController(ApplicationDbContext argDbContext)
        {
            this.dbContext = argDbContext;
        }

        //GET api/quiz/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = dbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();
            return new JsonResult(quiz.Adapt<QuizViewModel>(), new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        //GET api/quiz/latest
        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = dbContext.Quizzes.OrderBy(i => i.CreatedDate).Take(num).ToArray();
            return new JsonResult(latest.Adapt<QuizViewModel[]>(), new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = dbContext.Quizzes.OrderBy(i => i.Title).Take(num).ToArray();
            return new JsonResult(byTitle.Adapt<QuizViewModel[]>(), new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = dbContext.Quizzes.OrderBy(i => Guid.NewGuid()).Take(num).ToArray();
            return new JsonResult(random.Adapt<QuizViewModel[]>(), new JsonSerializerSettings() { Formatting = Formatting.Indented });
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
