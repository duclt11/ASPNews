namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public category()
        {
            posts = new HashSet<post>();
            posts1 = new HashSet<post>();
        }

        [Key]
        public int category_id { get; set; }

        [Required]
        [StringLength(190)]
        [Display(Name ="Tiêu đề")]
        public string category_name { get; set; }
        [Display(Name ="Ngày tạo")]
        public DateTime created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<post> posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<post> posts1 { get; set; }
    }
}
