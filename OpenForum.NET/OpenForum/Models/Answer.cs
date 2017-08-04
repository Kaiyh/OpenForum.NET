namespace OpenForum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? qid { get; set; }

        [StringLength(50)]
        public string solver { get; set; }

        public string content { get; set; }

        [StringLength(50)]
        public string solvertime { get; set; }
    }
}
