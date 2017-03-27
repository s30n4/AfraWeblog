using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
