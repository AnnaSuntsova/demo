namespace EFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerCards")]
    public partial class CustomerCards
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(19)]
        public string CardNumber { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [StringLength(50)]
        public string CardHolder { get; set; }       
    }
}

