namespace OpenForum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserInfo")]
    public partial class UserInfo
    {
        [Key]
        [StringLength(50)]
        public string username { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string sex { get; set; }

        [StringLength(50)]
        public string birthday { get; set; }

        [StringLength(50)]
        public string page { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [StringLength(50)]
        public string address { get; set; }

        public string education { get; set; }

        public string work { get; set; }

        public int? points { get; set; }

        public string questionID { get; set; }

        public string solveID { get; set; }
    }
}
