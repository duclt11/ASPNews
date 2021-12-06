namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("posts")]
    public partial class posts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public posts()
        {
            comments = new HashSet<comments>();
        }
        [Key]
        [DisplayName("Mã bài viết")]      
        public int post_id { get; set; }

        [Required(ErrorMessage = "Title không được để trống!")]
        [StringLength(4000)]
        [DisplayName("Tiêu đề")]
        public string title { get; set; }

        [Required(ErrorMessage = "Miêu tả không được để trống!")]
        [StringLength(4000)]
        [DisplayName("Miêu tả bài viết")]
        public string description { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống!")]
        [StringLength(4000)]
        [DisplayName("Nội dung bài viết")]
        public string content { get; set; }


        [DisplayName("ảnh bài viết")]
        [Column(TypeName = "text")]
        public string avatar { get; set; }

        public DateTime created_at { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống!")]
        public int? category_id { get; set; }

        [Required(ErrorMessage = "Người viết bài không được để trống")]
        public int? poster_id { get; set; }

        public virtual categories categories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comments> comments { get; set; }

        public virtual poster poster { get; set; }
    }
}
