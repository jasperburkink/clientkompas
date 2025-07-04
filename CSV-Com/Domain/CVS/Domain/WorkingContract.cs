﻿using Domain.Common;
using Domain.CVS.Enums;

namespace Domain.CVS.Domain
{
    public class WorkingContract : BaseAuditableEntity
    {
        public int Id { get; set; }

        public Client Client { get; set; }

        public int ClientId { get; set; }

        public Organization Organization { get; set; }

        public int OrganizationId { get; set; }

        public string Function { get; set; }

        public ContractType ContractType { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }
    }
}
