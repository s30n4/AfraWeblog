using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AW.Application.Services.Contracts;
using AW.Application.Dtos.NewsCategory;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/NewsCategory")]
    public class NewsCategoryController : Controller
    {
        private readonly INewsCategory _newsCategory;

        public NewsCategoryController(INewsCategory newsCategory)
        {
            _newsCategory = newsCategory;
        }

        // Get: api/NewsCategory/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _newsCategory.GetById(id);
            return Json(result);
        }

        //// Get: api/NewsCategory/{}
        //[HttpGet]
        //public IActionResult Get(BaseSearchRequest data)
        //{
        //    var result = _system.GetAsync(data);
        //    return Json(result);
        //}

        // POST api/NewsCategory
        [HttpPost]
        public IActionResult Post([FromBody] NewsCategoryDto data)
        {
            var result = _newsCategory.AddAsync(data);
            return Json(result);
        }

        // Put api/NewsCategory
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] NewsCategoryDto data)
        {
            var result = _newsCategory.AddAsync(data, id);
            return Json(result);
        }

        // POST api/NewsCategory
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _newsCategory.DeleteById(id);
            return Json(result);
        }
    }
}
