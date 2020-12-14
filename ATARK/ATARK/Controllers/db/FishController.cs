using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATARK.Models.Entity;
using ATARK.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATARK.Controllers.db
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IRepository repository;

        public FishController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Fish>> Get()
        {
            var fish = await this.repository.GetRangeAsync<Fish>(true, x => true);
            return fish.ToArray();
        }

        [HttpGet("{fishId}")]
        public async Task<Fish> GetById(int fishId)
        {
            var fish = await this.repository.GetAsync<Fish>(true, x => x.FishId == fishId);
            if (fish == null)
            {
                throw new Exception("Fish not found.");
            }
            return fish;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Fish fish)
        {
            var pool = await this.repository.GetAsync<Pool>(true, x => x.PoolId == fish.PoolNowId);
            string whoIsInThePool = pool.WhoIsInThePool;//fish herd none
            fish.RelocationPoolId = fish.PoolNowId;
            if (whoIsInThePool == "fish")
            {
                await this.repository.AddAsync<Fish>(fish);
            }
            else if(whoIsInThePool == "none")
            {
                await this.repository.AddAsync<Fish>(fish);
                pool.WhoIsInThePool = "fish";
            }
            else
            {
                throw new Exception("The herd is already in the pool");
            }
            await this.repository.UpdateAsync<Pool>(pool);
            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Fish fish)
        {
            var currentFish = await this.repository.GetAsync<Fish>(true, x => x.FishId == fish.FishId);
            if (currentFish == null)
            {
                throw new Exception("Fish not found.");
            }
            currentFish.KindOfFishId = fish.KindOfFishId;
            currentFish.Sex = fish.Sex;
            currentFish.DateOfBirth = fish.DateOfBirth;
            currentFish.PoolNowId = fish.PoolNowId;
            currentFish.RelocationPoolId = fish.RelocationPoolId;
            currentFish.Weight = fish.Weight;
            currentFish.Adulthood = fish.Adulthood;
            currentFish.State = fish.State;
            await this.repository.UpdateAsync<Fish>(currentFish);
            return this.Ok();
    }
        [HttpPut("{fishId}")]
        public async Task<IActionResult> ConfirmRelocation(int fishId)
        {
            var currentFish = await this.repository.GetAsync<Fish>(true, x => x.FishId == fishId);
            if (currentFish == null)
            {
                throw new Exception("Fish not found.");
            }
            currentFish.PoolNowId = currentFish.RelocationPoolId;
            await this.repository.UpdateAsync<Fish>(currentFish);
            return this.Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRelocationPoolToPoolNow()
        {
            var fishs = await this.repository.GetRangeAsync<Fish>(true, x => true);
            if (fishs == null)
            {
                throw new Exception("Fish not found.");
            }
            foreach(Fish fish in fishs)
            {
                fish.RelocationPoolId = fish.PoolNowId;
                await this.repository.UpdateAsync<Fish>(fish);
            }

            return this.Ok();
        }
        [HttpDelete("{fishId}")]
        public async Task<IActionResult> Delete(int fishId)
        {
            var fish = await this.repository.GetAsync<Fish>(true, x => x.FishId == fishId);
            if (fish == null)
            {
                throw new Exception("Fish not found.");
            }
            await this.repository.DeleteAsync<Fish>(fish);
            return this.Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<Fish>> GetAllPregnantFish()
        {
            var fish = await this.repository.GetRangeAsync<Fish>(true, x => x.State == "Pregnancy");
            return fish.ToArray();
        }

        [HttpGet("{PoolIdId}")]
        public async Task<IEnumerable<Fish>> GetFishByPoolId(int PoolIdId)
        {
            var fishs = await this.repository.GetRangeAsync<Fish>(true, x => x.PoolNowId == PoolIdId);
            return fishs.ToArray();
        }

        [HttpGet("{PoolIdId}")]
        public async Task<IEnumerable<Fish>> GetFishForRelocationByPoolId(int PoolIdId)
        {
            var fishs = await this.repository.GetRangeAsync<Fish>(true, x => (x.RelocationPoolId == PoolIdId&& x.PoolNowId != x.RelocationPoolId));
            return fishs.ToArray();
        }

        [HttpGet("{ClosedWaterSupplyInstallatioId}")]
        public async Task<IEnumerable<Fish>> GetAllFishByCWSI_Id(int ClosedWaterSupplyInstallatioId)
        {
            var pools = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == ClosedWaterSupplyInstallatioId);
            List<Fish> fishs = new List<Fish>();
            foreach (Pool pool in pools)
            {
                foreach (Fish f in this.repository.GetRange<Fish>(true, x => x.PoolNowId == pool.PoolId))
                {
                    fishs.Add(f);
                }
            }
            return fishs.ToArray();
        }

        //[HttpGet("{organizatoinId}")]
        //public async Task<IEnumerable<Fish>> GetByOrganizatoinId(int organizatoinId)
        //{

        //    var closedWaterSupplyInstallations = await this.repository.GetRangeAsync<ClosedWaterSupplyInstallation>(true,
        //           x => x.OrganizationId == organizatoinId);
        //    List<Pool> pools = new List<Pool>();
        //    List<Fish> fishs = new List<Fish>();

        //    foreach (ClosedWaterSupplyInstallation closedWaterSupplyInstallation in closedWaterSupplyInstallations)
        //    {
        //        var pool = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == closedWaterSupplyInstallation.ClosedWaterSupplyInstallationId);
        //        pools.AddRange(pool);
        //    }
        //    foreach (Pool pool in pools)
        //    {
        //        var fish = await this.repository.GetRangeAsync<Fish>(true, x => x.PoolNowId == pool.PoolId);
        //        fishs.AddRange(fish);
        //    }
            
        //    List<Fish> fishs23 = new List<Fish>();
        //    List<int> fishsId = new List<int>();
        //    foreach (Fish f in fishs)
        //    {
        //        fishsId.Add(f.FishId);
        //    }
        //    foreach (int id in fishsId)
        //    {
        //        var fish = await this.repository.GetAsync<Fish>(true, x => x.FishId == id);
        //        fishs23.Add(fish);
        //    }
        //    if (fishs == null)
        //    {
        //        throw new Exception("Pool not found.");
        //    }
        //    return fishs23.ToArray();
        //}
    }
}
