using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Pregnancy : BaseTable
    {
        [Key]
        public int PregnancyId { get; set; }
        public int FishId { get; set; }
        public Fish Fish { get; set; }
        public DateTime StartDateOfPregnancy { get; set; }
    }
}
