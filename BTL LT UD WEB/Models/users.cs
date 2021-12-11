namespace BTL_LT_UD_WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    
    [Table("users")]
    public partial class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            comments = new HashSet<comments>();
        }
        [Key]
        [DisplayName("Mã người dùng")]
        public int user_id { get; set; }
        //[Index(IsUnique = true)]

        [Required(ErrorMessage = "Email không được để trống!")]
        [RegularExpression("^[a-z][a-z0-9_\\.]{5,32}@[a-z0-9]{2,}(\\.[a-z0-9]{2,4}){1,2}$")] //regex test email
        [StringLength(50)]
        [DisplayName("Email")]
        [Index(IsUnique = true)]
        public string email { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống!")]
        [StringLength(50)]
        [DisplayName("username")]
        public string username { get; set; }


        
        [StringLength(50)]
        [DisplayName("fullname")]
        public string fullname { get; set; }

        [Required(ErrorMessage = "Password không được để trống!")]
        [StringLength(128)]
        [DisplayName("password")]
        public string password { get; set; }


        
        
       
        [Column(TypeName = "text")]
        [DisplayName("Hình ảnh")]
        public string avatar { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        
        public DateTime? birthaday { get; set; }

        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comments> comments { get; set; }
    }
}
