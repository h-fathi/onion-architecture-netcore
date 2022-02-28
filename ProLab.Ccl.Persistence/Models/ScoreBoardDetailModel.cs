using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Data;
using System;
namespace ProLab.Ccl.Persistence.Models
{

    public class ScoreBoardDetailModel : BaseEntity, ISoftDeletedEntity, IDataIntegrityEntity, IAuditableEntity
    {

        public int IdentityId { get; set; }
        public int Value { get; set; }
        public short ScoreBoardDetailTypeId { get; set; }
        public DateTime EntryDate { get; set; }

        public string DataIntegrity { get; set; }


        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted  { get; set; }

        public void ApplyDataIntegrity()
        {
            DataIntegrity = (Id.GetHashCode() + IdentityId.GetHashCode() + Value.GetHashCode() + ScoreBoardDetailTypeId.GetHashCode() + EntryDate.GetHashCode()).ToString();
        }
    }
}
