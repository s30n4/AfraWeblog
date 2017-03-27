using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Dtos.NewsContent
{
    public class ContentOutputDto
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int? AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string SubmitData { get; set; }
    }
}
