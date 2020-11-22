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
        private int CWSIId;
        private List<List<Fish>> listOfFishInThePool;
        private readonly IRepository repository;
        private List<BufferPool> poolsForRedistribution;

        public BusinessLogic(IRepository repository, int CWSIId)
        {
            this.repository = repository;
            this.CWSIId = CWSIId;
        }



        //1 взять все басейны из этой системы
        //2 взять для каждого басейна список рыб которые в нем
        //3 взять допустимый вес риби в басейне
        //4 создаем новый ссписок басейнов В которые переселяем(все те же басейны но пустые)
        //5 берем в первом списке рыб басейна самую большую
        //6 пытаемся засунуть в первый басейн(проверить влазит по весу и по полу)
        //7 если он был пуст, новый пол присвоить басейну
        //8 если засунули, то ОТТУДА ОТКУДА ВЗЯЛИ, удалим эту рыбу
        //9 помнять басейн для релокации в рыбе, из выбраного басейна(сколько может еще влесть) отнять вес той рыбы что засунули

        public void Execute()
        {
            //взяли всі басейни в системі
            var oldPools = this.repository.GetRange<Pool>(true, x => (x.ClosedWaterSupplyInstallationId == CWSIId &&
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
                foreach (Fish f in fishsInThePool)
                {
                    fishs.Add(f);
                }
                if(fishs.Count >= 0)
                {
                    fishs.OrderByDescending(f => f.Weight);
                    listOfFishInThePool.Add((List<Fish>)fishs); //.OrderByDescending(x => x.Weight)
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
