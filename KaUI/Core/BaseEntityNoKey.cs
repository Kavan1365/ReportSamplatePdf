using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaUI.Core
{
    public class BaseEntityNoKey
    {
        public byte[] RowVersion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("getdate()")]
        public DateTime? Created { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedByUserId { get; set; }

        public Guid Guid { get; set; }
    }
}
