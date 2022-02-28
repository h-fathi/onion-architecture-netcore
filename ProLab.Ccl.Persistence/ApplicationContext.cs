using Microsoft.EntityFrameworkCore;
using ProLab.Ccl.Persistence.Models;
using ProLab.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.Persistence
{
    public class ApplicationContext : BaseDbContext
    {
        public ApplicationContext(DbContextOptions<BaseDbContext> options) : base(options) { }


        //Db sets
        public DbSet<ScoreBoardModel> ScoreBoard { get; set; }

        public DbSet<ScoreBoardDetailModel> ScoreBoardDetail { get; set; }













        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set default schema
            modelBuilder.HasDefaultSchema("ccl");
            base.OnModelCreating(modelBuilder);

        }
    }
}
