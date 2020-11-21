using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class ClosedWaterSupplyInstallation : BaseTable
    {
        [Key]
        public int ClosedWaterSupplyInstallationId { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int StateOfTheSystemId { get; set; }
        [ForeignKey("StateOfTheSystemId")]
        public StateOfTheSystem StateOfTheSystem { get; set; }
        public string Location { get; set; }
        public List<Pool> Pools { get; set; }

    }
}
