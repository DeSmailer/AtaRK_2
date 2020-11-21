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
            await this.repository.AddAsync<Fish>(fish);

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
    }
}
