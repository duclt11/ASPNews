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
            posts = new HashSet<posts>();
        }

        [Key]
        public int poster_id { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Email không được để trống!")]
        [RegularExpression("^[a-z][a-z0-9_\\.]{5,32}@[a-z0-9]{2,}(\\.[a-z0-9]{2,4}){1,2}$")] //regex test email
        
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        
        [StringLength(50)]
        public string fullname { get; set; }

        [Required]
        [StringLength(128)]
        public string password { get; set; }

       

       
        [StringLength(100)]
        public string avatar { get; set; }
        [RegularExpression("0[\\d]{9,9}")]
        public int? phone { get; set; }

       
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? birthaday { get; set; }

        public DateTime created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<posts> posts { get; set; }
    }
}
