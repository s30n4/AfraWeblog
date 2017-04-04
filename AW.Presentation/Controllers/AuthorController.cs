using AW.Application.Dtos.Author;
using AW.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/Author")]
    public class AuthorController : Controller
    {
        private readonly IAuthor _aut;

        public AuthorController(IAuthor aut)
        {
            _aut = aut;
        }

        // Get: api/Author/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _aut.GetById(id);
            return Json(result);
        }

        //// Get: api/System/{}
        //[HttpGet]
        //public IActionResult Get(BaseSearchRequest data)
        //{
        //    var result = _system.GetAsync(data);
        //    return Json(result);
        //}

        // POST api/Author
        [HttpPost]
        public IActionResult Post([FromBody] AuthorDto data)
        {
            var result = _aut.AddAsync(data);
            return Json(result);
        }

        // Put api/Author
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] AuthorDto data)
        {
            var result = _aut.AddAsync(data, id);
            return Json(result);
        }

        // POST api/Author
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _aut.DeleteById(id);
            return Json(result);
        }
    }
}
