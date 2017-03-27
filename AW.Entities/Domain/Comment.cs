using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Entities.Domain
{
    public class Comment
    {

        public int Id { get; set; }

        public int NewsId { get; set; }
        [ForeignKey("NewsId")]
        public virtual NewsContent NewsContents { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public string Context { get; set; }

        public DateTime SubmitDate { get; set; }

        public bool IsPrivate { get; set; }

        public bool IsConfirm { get; set; }
    }
}
