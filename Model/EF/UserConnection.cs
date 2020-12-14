namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserConnection")]
    public partial class UserConnection
    {
        public Guid ID { get; set; }

        public Guid? UserID { get; set; }

        public Guid? ConnectionID { get; set; }
    }
}
