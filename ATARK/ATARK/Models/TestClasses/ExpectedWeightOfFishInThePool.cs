using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models.TestClasses
{
    public class ExpectedWeightOfFishInThePool
    {
        public int poolId;
        public float maxWeight;
        public float currentWeight;
        public ExpectedWeightOfFishInThePool(int poolId, float maxWeight, float currentWeight)
        {
            this.poolId = poolId;
            this.maxWeight = maxWeight;
            this.currentWeight = currentWeight;
        }

    }
}
