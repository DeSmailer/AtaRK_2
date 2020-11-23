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
    public class HerdController : ControllerBase
    {
        private readonly IRepository repository;
        
        public HerdController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Herd>> Get()
        {
            var herd = await this.repository.GetRangeAsync<Herd>(true, x => true);
            return herd.ToArray();
        }

        [HttpGet("{herdId}")]
        public async Task<Herd> GetById(int herdId)
        {
            var herd = await this.repository.GetAsync<Herd>(true, x => x.HerdId == herdId);
            if (herd == null)
            {
                throw new Exception("Herd not found.");
            }
            return herd;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Herd herd)
        {
            var pool = await this.repository.GetAsync<Pool>(true, x => x.PoolId == herd.PoolIdNow);
            string whoIsInThePool = pool.WhoIsInThePool;//fish herd none
            if (whoIsInThePool == "herd")
            {
                await this.repository.AddAsync<Herd>(herd);
            }
            else if (whoIsInThePool == "none")
            {
                await this.repository.AddAsync<Herd>(herd);
                pool.WhoIsInThePool = "herd";
            }
            else
            {
                throw new Exception("The fish is already in the pool");
            }
            await this.repository.UpdateAsync<Pool>(pool);
            return this.Ok();

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Herd herd)
        {
            var currentHerd = await this.repository.GetAsync<Herd>(true, x => x.HerdId == herd.HerdId);
            if (currentHerd == null)
            {
                throw new Exception("Herd not found.");
            }
            currentHerd.KindOfFishId = herd.KindOfFishId;
            currentHerd.DateOfBirth = herd.DateOfBirth;
            currentHerd.PoolIdNow = herd.PoolIdNow;
            currentHerd.AverageWeightOfAnIndividual = herd.AverageWeightOfAnIndividual;
            currentHerd.Quantity = herd.Quantity;
            await this.repository.UpdateAsync<Herd>(currentHerd);
            return this.Ok();
    }

        [HttpDelete("{herdId}")]
        public async Task<IActionResult> Delete(int herdId)
        {
            var herd = await this.repository.GetAsync<Herd>(true, x => x.HerdId == herdId);
            if (herd == null)
            {
                throw new Exception("Herd not found.");
            }
            await this.repository.DeleteAsync<Herd>(herd);
            return this.Ok();
        }
        [HttpGet("{PoolIdId}")]
        public async Task<IEnumerable<Herd>> GetAllHerdByPoolId(int PoolIdId)
        {
            var fishs = await this.repository.GetRangeAsync<Herd>(true, x => x.PoolIdNow == PoolIdId);
            return fishs.ToArray();
        }

        [HttpGet("{ClosedWaterSupplyInstallatioId}")]
        public async Task<IEnumerable<Herd>> GetAllHerdByCWSIId(int ClosedWaterSupplyInstallatioId)
        {
            var pools = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == ClosedWaterSupplyInstallatioId);
            List<Herd> fishs = new List<Herd>();
            foreach (Pool pool in pools)
            {
                foreach (Herd h in this.repository.GetRange<Herd>(true, x => x.PoolIdNow == pool.PoolId))
                {
                    fishs.Add(h);
                }
            }
            return fishs.ToArray();
        }
    }
}
