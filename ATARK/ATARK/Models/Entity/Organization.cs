using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Organization : BaseTable
    {
        [Key]
        public int OrganizationId { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime FoundationDate { get; set; }
        public string PhoneNumber { get; set; }

        public List<ClosedWaterSupplyInstallation> ClosedWaterSupplyInstallations { get; set; }
    }
}
