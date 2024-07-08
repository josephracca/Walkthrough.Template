using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class NonAuditBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
