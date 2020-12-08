using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATARK.Models.Entity;
using ATARK.Models.Interfaces;
using ATARK.Models.TestClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATARK.Controllers.db
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PoolController : ControllerBase
    {
        private readonly IRepository repository;

        public PoolController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Pool>> Get()
        {
            var pool = await this.repository.GetRangeAsync<Pool>(true, x => true);
            return pool.ToArray();
        }

        [HttpGet("{poolId}")]
        public async Task<Pool> GetById(int poolId)
        {
            var pool = await this.repository.GetAsync<Pool>(true, x => x.PoolId == poolId);
            if (pool == null)
            {
                throw new Exception("Pool not found.");
            }
            return pool;
        }
        [HttpGet("{organizatoinId}")]
        public async Task<IEnumerable<Pool>> GetByOrganizatoinId(int organizatoinId)
        {
            var closedWaterSupplyInstallations = await this.repository.GetRangeAsync<ClosedWaterSupplyInstallation>(true,
                    x => x.OrganizationId == organizatoinId);
            List<Pool> pools = new List<Pool>();
            foreach (ClosedWaterSupplyInstallation closedWaterSupplyInstallation in closedWaterSupplyInstallations)
            {
                var pool = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == closedWaterSupplyInstallation.ClosedWaterSupplyInstallationId);
                pools.AddRange(pool);
            }
            if (pools == null)
            {
                throw new Exception("Pool not found.");
            }

            return pools.ToArray();
        }

        [HttpGet("{closedWaterSupplyInstallationId}")]
        public async Task<IEnumerable<Pool>> GetByCWIIdId(int closedWaterSupplyInstallationId)
        {
            var pools = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == closedWaterSupplyInstallationId);
            if (pools == null)
            {
                throw new Exception("Pool not found.");
            }

            return pools.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Pool pool)
        {
            pool.WhoIsInThePool = "none";
            await this.repository.AddAsync<Pool>(pool);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Pool pool)
        {
            var currentPool = await this.repository.GetAsync<Pool>(true, x => x.PoolId == pool.PoolId);
            if (currentPool == null)
            {
                throw new Exception("Pool not found.");
            }
            currentPool.ClosedWaterSupplyInstallationId = pool.ClosedWaterSupplyInstallationId;
            currentPool.WhoIsInThePool = pool.WhoIsInThePool;
            currentPool.Volume = pool.Volume;
            await this.repository.UpdateAsync<Pool>(currentPool);
            return this.Ok();

    }

        [HttpDelete("{poolId}")]
        public async Task<IActionResult> Delete(int poolId)
        {
            var pool = await this.repository.GetAsync<Pool>(true, x => x.PoolId == poolId);
            if (pool == null)
            {
                throw new Exception("Pool not found.");
            }
            await this.repository.DeleteAsync<Pool>(pool);
            return this.Ok();
        }

        [HttpGet("{ClosedWaterSupplyInstallatioId}")]
        public async Task<IEnumerable<ExpectedWeightOfFishInThePool>> GetWeightOfFishInThePool(int ClosedWaterSupplyInstallatioId)
        {
            var pools = await this.repository.GetRangeAsync<Pool>(true, x => (x.ClosedWaterSupplyInstallationId == ClosedWaterSupplyInstallatioId && x.WhoIsInThePool != "herd"));
            List<ExpectedWeightOfFishInThePool> listExpectedWeightOfFishInThePools = new List<ExpectedWeightOfFishInThePool>();
            foreach (Pool pool in pools)
            {
                ExpectedWeightOfFishInThePool expectedWeightOfFishInThePool = new ExpectedWeightOfFishInThePool(pool.PoolId, 40 * pool.Volume, 0);
                foreach (Fish item in this.repository.GetRange<Fish>(true, x => x.RelocationPoolId == pool.PoolId))
                {
                    expectedWeightOfFishInThePool.currentWeight += item.Weight;
                }
                listExpectedWeightOfFishInThePools.Add(expectedWeightOfFishInThePool);
            }
            return listExpectedWeightOfFishInThePools.ToArray();
        }
      
    }

}
