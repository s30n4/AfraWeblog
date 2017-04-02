using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AW.Application.Services.Contracts;
using AW.Application.Dtos.Label;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AW.Presentation.Controllers
{
    [Route("api/Label")]
    public class LabelController : Controller
    {
        private readonly ILabel _lab;

        public LabelController(ILabel lab)
        {
            _lab = lab;
        }

        // Get: api/Label/{}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _lab.GetById(id);
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
        public IActionResult Post([FromBody] LabelDto data)
        {
            var result = _lab.AddAsync(data);
            return Json(result);
        }

        // Put api/Label
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LabelDto data)
        {
            var result = _lab.AddAsync(data, id);
            return Json(result);
        }

        // POST api/Label
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _lab.DeleteById(id);
            return Json(result);
        }
    }
}
