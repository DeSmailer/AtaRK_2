using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Herd : BaseTable
    {
        [Key]
        public int HerdId { get; set; }
        public int KindOfFishId { get; set; }
        public KindOfFish KindOfFish { get; set; }//мб привязать
        public DateTime DateOfBirth { get; set; }
        public int PoolIdNow { get; set; }//привязать
        public Pool Pool { get; set; }
        public float AverageWeightOfAnIndividual { get; set; }
        public int Quantity { get; set; }
    }
}
