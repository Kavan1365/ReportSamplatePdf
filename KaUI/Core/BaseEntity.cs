using System;
using System.ComponentModel.DataAnnotations;

namespace KaUI.Core
{
    public class BaseEntity : IBaseEntity
    {

        [Key]
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedByUserId { get; set; }

        public Guid Guid { get; set; }

        public bool IsSoftDeleted { get; set; }


    }

}
