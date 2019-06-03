using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.ViewModels;
using TestMakerFree.Data;
using Newtonsoft.Json;
using Mapster;

namespace TestMakerFree.Controllers
{
    public class ResultController : BaseApiController
    {
        public ResultController(ApplicationDbContext argDbContext) : base(argDbContext)
        {}

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = DbContext.Results.Where(x => x.Id == id).FirstOrDefault();
            if (result == null) { return NotFound(new { Error = $"No Result ID {id} has been found" }); }
            return new JsonResult(result.Adapt<ResultViewModel>(), JsonSettings);
        }

        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            if (model == null) { return new StatusCodeResult(500); }
            var result = model.Adapt<Data.Models.Result>();
            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;
            DbContext.Results.Add(result);
            DbContext.SaveChanges();
            return new JsonResult(result.Adapt<ResultViewModel>(), JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            if (model == null) { return new StatusCodeResult(500); }
            var result = DbContext.Results.Where(x => x.Id == model.Id).FirstOrDefault();
            if (result == null) { return NotFound(new { Error = $"No Result ID  {model.Id} has been found" }); }
            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;
            result.LastModifiedDate = result.CreatedDate;
            DbContext.SaveChanges();
            return new JsonResult(result.Adapt<ResultViewModel>(), JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = DbContext.Results.Where(x => x.Id == id).FirstOrDefault();
            if (result == null) { return NotFound(new { Error = $"No Result ID {id} has been found" }); }
            DbContext.Remove(result);
            DbContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = DbContext.Results.Where(x => x.QuizId == quizId).ToArray();
            return new JsonResult(results.Adapt<ResultViewModel[]>(), JsonSettings);
        }
    }
}
