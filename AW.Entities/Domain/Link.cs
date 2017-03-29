using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AW.Entities.Domain
{
    [Table("Links", Schema = "dbo")]
    public class Link
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }
         
        public string Url { get; set; }
    }
}
