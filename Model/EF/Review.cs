namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductCode { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid UserID { get; set; }

        [StringLength(50)]
        public string NameUser { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Text { get; set; }

        [Key]
        [Column(Order = 3)]
        public double ReviewPoint { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Status { get; set; }
    }
}
