namespace OpenForum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string content { get; set; }

        [StringLength(50)]
        public string time { get; set; }

        [StringLength(50)]
        public string master { get; set; }

        public string aID { get; set; }

        [StringLength(50)]
        public string kind { get; set; }
    }
}
