using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Pool : BaseTable
    {
        [Key]
        public int PoolId { get; set; }
        public int ClosedWaterSupplyInstallationId { get; set; }
        public ClosedWaterSupplyInstallation ClosedWaterSupplyInstallation { get; set; }
        public string WhoIsInThePool { get; set; }
        public float Volume { get; set; }
        public List<Herd> Herds { get; set; }
        public List<Fish> FishsNow { get; set; }
        public List<Fish> RelocationFishs { get; set; }

    }
}
