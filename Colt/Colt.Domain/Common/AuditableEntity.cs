﻿namespace Colt.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
