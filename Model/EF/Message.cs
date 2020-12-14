namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Message")]
    public partial class Message
    {
        public Guid ID { get; set; }

        public Guid? FromUser { get; set; }

        public Guid? ToUser { get; set; }

        [Column("Message")]
        public string Message1 { get; set; }

        public DateTime? Date { get; set; }
    }
}
