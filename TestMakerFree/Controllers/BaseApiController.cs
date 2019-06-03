using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestMakerFree.Data;
using Newtonsoft.Json;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        protected ApplicationDbContext DbContext;
        protected JsonSerializerSettings JsonSettings;

        public BaseApiController(ApplicationDbContext argDbContext)
        {
            DbContext = argDbContext;

            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }
    }
}
