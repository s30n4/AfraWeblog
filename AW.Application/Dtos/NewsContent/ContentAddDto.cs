using System.ComponentModel.DataAnnotations;

namespace AW.Application.Dtos.NewsContent
{
    public class ContentAddDto
    {        

        [MaxLength(200)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public int? CategoryId { get; set; } 
        
        public int? AuthorId { get; set; } 
    }
}
