using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AW.Application.Services.Contracts;
using AW.Application.Dtos.NewsContent;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/NewsContent")]
    public class NewsContentController : Controller
    {
        private readonly INewsContent _newsContent;

        public NewsContentController(INewsContent newsContent)
        {
            _newsContent = newsContent;
        }

        // Get: api/NewsContent/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _newsContent.GetById(id);
            return Json(result);
        }

        //// Get: api/NewsContent/{}
        //[HttpGet]
        //public IActionResult Get(BaseSearchRequest data)
        //{
        //    var result = _system.GetAsync(data);
        //    return Json(result);
        //}

        // POST api/NewsContent
        [HttpPost]
        public IActionResult Post([FromBody] ContentAddDto data)
        {
            var result = _newsContent.AddAsync(data);
            return Json(result);
        }

        // Put api/NewsContent
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ContentAddDto data)
        {
            var result = _newsContent.AddAsync(data, id);
            return Json(result);
        }

        // POST api/NewsContent
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _newsContent.DeleteById(id);
            return Json(result);
        }

    }
}
