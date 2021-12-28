namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public post()
        {
            comments = new HashSet<comment>();
        }

        [Key]
        public int post_id { get; set; }

        [Required]
        [Display(Name ="Tiêu đề")]
        [StringLength(4000)]
        public string title { get; set; }

        [Required]
        [Display(Name ="Mô tả")]
        [StringLength(4000)]
        public string description { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name ="Nội dung")]
        public string content { get; set; }
        [Display(Name ="Ảnh đại diện")]
        public string avatar { get; set; }
        [Display(Name ="Ngày đăng")]
        public DateTime created_at { get; set; }

        public int? category_id { get; set; }

        public int? poster_id { get; set; }

        public virtual category category { get; set; }

        public virtual category category1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        public virtual poster poster { get; set; }

        public virtual poster poster1 { get; set; }
    }
}
