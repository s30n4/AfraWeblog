using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AW.Entities.Domain
{
    [Table("Labels", Schema = "dbo")]
    public class Label
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<NewsLabel> NewsLabels { get; set; }

        public Label()
        {
            NewsLabels = new HashSet<NewsLabel>();
        }
    }
}
