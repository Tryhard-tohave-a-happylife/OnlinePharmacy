namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public Guid ID { get; set; }

        public Guid CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public int? CostBy { get; set; }

        public bool? ChangeStatus { get; set; }
    }
}
