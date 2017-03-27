using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Entities.Domain
{
    [Table("NewsContents", Schema = "dbo")]
    public class NewsContent
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Subject { get; set; }

        public string Body { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual NewsCategory NewsCategories { get; set; }

        public DateTime SubmitDate { get; set; }

        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Authors { get; set; }

        public virtual ICollection<NewsLabel> NewsLabels { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public NewsContent()
        {
            NewsLabels = new HashSet<NewsLabel>();
            Comments = new HashSet<Comment>();
        }
    }
}
