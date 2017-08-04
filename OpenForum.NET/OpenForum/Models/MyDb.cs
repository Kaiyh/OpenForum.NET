namespace OpenForum.Models {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDb : DbContext {
        public MyDb()
            : base("name=MyDb2") {
        }

        public virtual DbSet<Answer> Answers {
            get; set;
        }
        public virtual DbSet<Question> Questions {
            get; set;
        }
        public virtual DbSet<UserInfo> UserInfoes {
            get; set;
        }
        public virtual DbSet<UserLogin> UserLogins {
            get; set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<UserLogin>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<UserLogin>()
                .Property(e => e.email)
                .IsUnicode(false);
        }
    }
}
