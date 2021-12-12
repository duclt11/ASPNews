namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        [StringLength(4000)]
        public string title { get; set; }

        [Required]
        [StringLength(4000)]
        public string description { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        [StringLength(100)]
        public string avatar { get; set; }

        public DateTime created_at { get; set; }

        public int? category_id { get; set; }

        public int? poster_id { get; set; }

        public virtual category category { get; set; }

        public virtual poster poster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }
    }
}
