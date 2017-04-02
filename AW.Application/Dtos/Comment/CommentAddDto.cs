using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Application.Dtos.Comment
{
    public class CommentAddDto
    {
        public int NewsId { get; set; } 

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public string Context { get; set; }
          
        public bool IsPrivate { get; set; }

    }
}
