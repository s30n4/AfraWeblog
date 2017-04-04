using Microsoft.AspNetCore.Mvc;
using AW.Application.Services.Contracts;
using AW.Application.Dtos.Link;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/Link")]
    public class LinkController : Controller
    {
        private readonly ILink _lin;

        public LinkController(ILink lin)
        {
            _lin = lin;
        }

        // Get: api/Label/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _lin.GetById(id);
            return Json(result);
        }

        //// Get: api/Label/{}
        //[HttpGet]
        //public IActionResult Get(BaseSearchRequest data)
        //{
        //    var result = _system.GetAsync(data);
        //    return Json(result);
        //}

        // POST api/Label
        [HttpPost]
        public IActionResult Post([FromBody] LinkDto data)
        {
            var result = _lin.AddAsync(data);
            return Json(result);
        }

        // Put api/Label
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LinkDto data)
        {
            var result = _lin.AddAsync(data, id);
            return Json(result);
        }

        // POST api/Label
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _lin.DeleteById(id);
            return Json(result);
        }
    }
}
