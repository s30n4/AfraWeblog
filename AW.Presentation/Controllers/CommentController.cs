using Microsoft.AspNetCore.Mvc;
using AW.Application.Services.Contracts;
using AW.Application.Dtos.Comment;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/Comment")]
    public class CommentController : Controller
    {
        private readonly IComment _com;

        public CommentController(IComment com)
        {
            _com = com;
        }

        // Get: api/Author/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _com.GetById(id);
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
        public IActionResult Post([FromBody] CommentAddDto data)
        {
            var result = _com.AddAsync(data);
            return Json(result);
        }

        // Put api/Author
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CommentAddDto data)
        {
            var result = _com.AddAsync(data, id);
            return Json(result);
        }

        // Put api/Author
        [HttpPut("Confirm")]
        public IActionResult Confirm( [FromBody] CommentConfirmDto data)
        {
            var result = _com.Confirm(data);
            return Json(result);
        }

        // POST api/Author
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _com.DeleteById(id);
            return Json(result);
        }
    }
}
