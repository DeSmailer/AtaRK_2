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
        
        public async void Execute()
        {
            //взяли всі басейни в системі
            var oldPools =  await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == CWSIId);
            //список басейнів у які буде перерозподілено рибу 
            poolsForRedistribution = new List<BufferPool>();

            foreach (Pool pool in oldPools)
            {
                List<Fish> fishs = new List<Fish>();

                //заповнюємо список басейнів, у які буде перенаправлено рибу
                poolsForRedistribution.Add(new BufferPool(pool.PoolId, "", pool.Volume * 40, pool.Volume * 40));

                //у кожен басейн записали список риби що у ньому
                foreach (Fish f in this.repository.GetRange<Fish>(true, x => (x.PoolNowId == pool.PoolId)))
                {
                    fishs.Add(f);
                }
                listOfFishInThePool.Add((List<Fish>)fishs.OrderByDescending(x => x.Weight)); 
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

        private async void MoveTo(Fish fish)
        {
            foreach(BufferPool bufferPool in poolsForRedistribution)
            {
                if(bufferPool.sex.Equals("m") && fish.Sex.Equals("m")) //якщо басейн ще зовсім пустий
                {
                    if (fish.Weight <= bufferPool.spaceLeft)
                    {
                        UpdateRelocationPoolId(fish, bufferPool);
                        break;
                    }
                }
                else if(bufferPool.sex.Equals("w") && fish.Sex.Equals("w"))//якщо в ньому вже є жіночі особини
                {
                    if (fish.Weight <= bufferPool.spaceLeft)
                    {
                        UpdateRelocationPoolId(fish, bufferPool);
                        break;
                    }
                }
                else//чоловічі
                {
                    if (fish.Weight <= bufferPool.spaceLeft)
                    {
                        UpdateRelocationPoolId(fish, bufferPool);
                        bufferPool.sex = fish.Sex;
                        break;
                    }
                }
            }
        }
        private async void UpdateRelocationPoolId(Fish fish, BufferPool bufferPool)
        {
            bufferPool.spaceLeft = bufferPool.spaceLeft - fish.Weight;
            var currentFish = await this.repository.GetAsync<Fish>(true, x => x.FishId == fish.FishId);
            currentFish.RelocationPoolId = bufferPool.poolId;
            await this.repository.UpdateAsync<Fish>(currentFish);
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
