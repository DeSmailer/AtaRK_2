using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;



namespace ATARK.Models.Entity
{
    public class KindOfFish : BaseTable 
    {
        [Key]
        public int KindOfFishId { get; set; }
        public string Kind { get; set; }
        public List<Herd> Herds { get; set; }
        public List<Fish> Fishs { get; set; }
    }
}
