namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("poster")]
    public partial class poster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public poster()
        {
            posts = new HashSet<post>();
            posts1 = new HashSet<post>();
        }

        [Key]
        public int poster_id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Email")]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Tên hiển thị")]
        public string username { get; set; }

        [StringLength(50)]
        [Display(Name ="Họ tên")]
        public string fullname { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name ="Mật khẩu")]
        public string password { get; set; }
        [Display(Name ="Ảnh đại diện")]
        public string avatar { get; set; }
        [Display(Name ="Điện thoại")]
        public string phone { get; set; }

        [Display(Name ="Ngày tham gia")]
        public DateTime created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<post> posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<post> posts1 { get; set; }
    }
}
