using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProLab.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await ApplyAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await ApplyAudit();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            AsyncHelper.RunSync(() => ApplyAudit());
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AsyncHelper.RunSync(() => ApplyAudit());
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// code to enter values in audit table
        /// </summary>
        private async Task ApplyAudit()
        {

            var auditService = EngineContext.Current.Resolve<IAuditService>();

            if (auditService == null) return;

            var currentUser = await auditService.GetCurrentUser();
            //check any user is logined
            if (currentUser != null && currentUser.UserId > 0)
            {
                var changedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted).ToList();
                foreach (var entry in changedEntries)
                {
                    if (entry.Entity.GetType().GetInterface(nameof(IAuditableEntity)) != null)
                    {
                        var entity = entry.Entity as IAuditableEntity;
                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedBy = currentUser.UserId;
                            entity.CreatedDate = DateTime.Now;
                        }

                        else if (entry.State == EntityState.Modified)
                        {
                            entity.ModifiedBy = currentUser.UserId;
                            entity.ModifiedDate = DateTime.Now;
                        }
                        else if (entry.State == EntityState.Deleted)
                        {
                            if (entity.GetType().GetInterface(nameof(ISoftDeletedEntity)) != null)
                            {
                                entity.ModifiedBy = currentUser.UserId;
                                entity.ModifiedDate = DateTime.Now;
                            }
                        }
                    }
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
