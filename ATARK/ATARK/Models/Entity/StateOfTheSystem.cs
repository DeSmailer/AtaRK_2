using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class StateOfTheSystem : BaseTable
    {
        [Key]
        public int StateOfTheSystemId { get; set; }
        public ClosedWaterSupplyInstallation ClosedWaterSupplyInstallation {get; set; }
        public float Temperature { get; set; }
        public float OxygenLevel { get; set; }
        public DateTime DateOfLastCheck { get; set; }
    }
}
