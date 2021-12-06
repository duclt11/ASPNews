namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class admins
    {
        [Key]
        public int admin_id { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string fullname { get; set; }

        [Required]
        [StringLength(10)]
        public string password { get; set; }

        [Required]
        [StringLength(10)]
        public string reset_password { get; set; }

        [Required]
        [StringLength(100)]
        public string avatar { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthaday { get; set; }

        public DateTime created_at { get; set; }
    }
}
