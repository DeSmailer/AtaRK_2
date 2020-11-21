using ATARK.Models.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.Entity
{
    public class Fish : BaseTable
    {
        [Key]
        public int FishId { get; set; }
        public int KindOfFishId { get; set; }
        public KindOfFish KindOfFish { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PoolNowId { get; set; }
        [ForeignKey("PoolNowId")]
        public Pool PoolNow { get; set; }
        public int RelocationPoolId { get; set; }
        [ForeignKey("RelocationPoolId")]
        public Pool RelocationPool { get; set; }
        public float Weight { get; set; }
        public string Adulthood { get; set; }
        public string State { get; set; }
        public List<Milking> Milkings { get; set; }
        public List<Pregnancy> Pregnancys { get; set; }
    }
}
