using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Milking : BaseTable
    {
        [Key]
        public int MilkingId { get; set; }
        public int FishId { get; set; }
        public Fish Fish { get; set; }
        public DateTime MilkingDate { get; set; }
        public int CaviarWeight { get; set; }
    }
}
