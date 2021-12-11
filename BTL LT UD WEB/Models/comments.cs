namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class comments
    {
        [Key]
        public int comment_id { get; set; }

        [Required]
        
        public string content { get; set; }

        [Required]
        [StringLength(1000)]
        public string status { get; set; }

        public int? user_id { get; set; }

        public DateTime datecomment { get; set; }

        public int? post_id { get; set; }

        public virtual posts posts { get; set; }

        public virtual users users { get; set; }
    }
}
