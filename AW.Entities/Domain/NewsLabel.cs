using System.ComponentModel.DataAnnotations.Schema;

namespace AW.Entities.Domain
{
    public class NewsLabel
    {
        public int Id { get; set; }

        public int NewsId { get; set; }
        [ForeignKey("NewsId")]
        public virtual NewsContent NewsContents { get; set; }
        
        public int LabelId { get; set; }
        [ForeignKey("LabelId")]
        public virtual Label Labels { get; set; }

    }
}
