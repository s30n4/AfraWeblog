using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AW.Entities.Domain
{
    [Table("Authors", Schema = "dbo")]
    public class Author
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }

        public virtual ICollection<NewsContent> NewsContents { get; set; }

        public Author()
        {
            NewsContents = new HashSet<NewsContent>();
        }
    }
}
