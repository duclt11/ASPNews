namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class comment
    {
        [Key]
        [Column(Order = 0)]
        public int comment_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string content { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1000)]
        public string status { get; set; }

        public int? user_id { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime datecomment { get; set; }

        public int? post_id { get; set; }

        public virtual post post { get; set; }

        public virtual user user { get; set; }
    }
}
