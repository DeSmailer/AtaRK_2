using ATARK.Models.Entity;
using ATARK.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATARK.Models
{
    public class BusinessLogic
    {
        private int ClosedWaterSupplyInstallatioId;
        private List<List<Fish>> listOfFishInThePool;
        private readonly IRepository repository;
        private List<BufferPool> poolsForRedistribution;

        public BusinessLogic(IRepository repository, int ClosedWaterSupplyInstallatioId)
        {
            this.repository = repository;
            this.ClosedWaterSupplyInstallatioId = ClosedWaterSupplyInstallatioId;
        }

        public void Execute()
        {
            //взяли всі басейни в системі
            var oldPools = this.repository.GetRange<Pool>(true, x => (x.ClosedWaterSupplyInstallationId == ClosedWaterSupplyInstallatioId &&
                   (x.WhoIsInThePool == "fish" || x.WhoIsInThePool == "none")));
            //список басейнів у які буде перерозподілено рибу 
            poolsForRedistribution = new List<BufferPool>();
            listOfFishInThePool = new List<List<Fish>>();

            foreach (Pool pool in oldPools)
            {
                List<Fish> fishs = new List<Fish>();

                //заповнюємо список басейнів, у які буде перенаправлено рибу
                poolsForRedistribution.Add(new BufferPool(pool.PoolId, "", pool.Volume * 40, pool.Volume * 40));
                var fishsInThePool = this.repository.GetRange<Fish>(true, x => (x.PoolNowId == pool.PoolId));
                //у кожен басейн записали список риби що у ньому
                foreach (Fish item in fishsInThePool)
                {
                    fishs.Add(item);
                }
                if(fishs.Count >= 0)
                {
                    fishs.OrderByDescending(f => f.Weight);
                    listOfFishInThePool.Add((List<Fish>)fishs);
                }
            }
            //перерозподіл
            foreach (List<Fish> listFish in listOfFishInThePool)
            {
                foreach (Fish fish in listFish)
                {
                    MoveTo(fish);
                }
            }
        }

        private void MoveTo(Fish fish)
        {
            bool isAdded = false;
            foreach (BufferPool bufferPool in poolsForRedistribution)
            {
                isAdded = false;
                if (bufferPool.sex.Equals(fish.Sex))
                {
                    if (fish.Weight <= bufferPool.spaceLeft)
                    {
                        UpdateRelocationPoolId(fish, bufferPool);
                        isAdded = true;
                    }
                }
                else if((bufferPool.sex.Equals("w") && fish.Sex.Equals("m"))|| (bufferPool.sex.Equals("m") && fish.Sex.Equals("w")))
                {
                    continue;
                }
                else
                {
                    if (fish.Weight <= bufferPool.spaceLeft)
                    {
                        UpdateRelocationPoolId(fish, bufferPool);
                        bufferPool.sex = fish.Sex;
                        isAdded = true;
                    }
                }
                if (isAdded)
                {
                    break;
                }
            }
            if (!isAdded)
            {
                var currentFish = this.repository.Get<Fish>(true, x => x.FishId == fish.FishId);
                currentFish.RelocationPoolId = currentFish.PoolNowId;
                this.repository.Update<Fish>(currentFish);
            }
        }

        private void UpdateRelocationPoolId(Fish fish, BufferPool bufferPool)
        {
            bufferPool.spaceLeft = bufferPool.spaceLeft - fish.Weight;
            var currentFish = this.repository.Get<Fish>(true, x => x.FishId == fish.FishId);
            currentFish.RelocationPoolId = bufferPool.poolId;
            this.repository.Update<Fish>(currentFish);
        }

        private class BufferPool
        {
            public int poolId;
            public string sex;
            public float wholePlace;
            public float spaceLeft;
            public BufferPool(int poolId, string sex, float wholePlace, float spaceLeft)
            {
                this.poolId = poolId;
                this.sex = sex;
                this.wholePlace = wholePlace;
                this.spaceLeft = spaceLeft;
            }
        }

    }
}
