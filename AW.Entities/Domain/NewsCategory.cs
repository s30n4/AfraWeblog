using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AW.Entities.Domain
{
    [Table("NewsCategories", Schema ="dbo")]
    public class NewsCategory
    {
        public int Id { get; set; }
         
        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<NewsContent> NewsContents { get; set; }

        public NewsCategory()
        {
            NewsContents = new HashSet<NewsContent>();
        }
    }
}
