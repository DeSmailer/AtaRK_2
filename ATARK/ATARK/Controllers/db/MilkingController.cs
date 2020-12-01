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
    public class MilkingController : ControllerBase
    {
        private readonly IRepository repository;

        public MilkingController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Milking>> Get()
        {
            var milking = await this.repository.GetRangeAsync<Milking>(true, x => true);
            return milking.ToArray();
        }

        [HttpGet("{milkingId}")]
        public async Task<Milking> GetById(int milkingId)
        {
            var milking = await this.repository.GetAsync<Milking>(true, x => x.MilkingId == milkingId);
            if (milking == null)
            {
                throw new Exception("Milking not found.");
            }
            return milking;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Milking milking)
        {
            await this.repository.AddAsync<Milking>(milking);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Milking milking)
        {
            var currentMilking = await this.repository.GetAsync<Milking>(true, x => x.MilkingId == milking.MilkingId);
            if (currentMilking == null)
            {
                throw new Exception("Milking not found.");
            }
            currentMilking.FishId = milking.FishId;
            currentMilking.MilkingDate = milking.MilkingDate;
            currentMilking.CaviarWeight = milking.CaviarWeight;
            await this.repository.UpdateAsync<Milking>(currentMilking);
            return this.Ok();

        }

        [HttpDelete("{milkingId}")]
        public async Task<IActionResult> Delete(int milkingId)
        {
            var milking = await this.repository.GetAsync<Milking>(true, x => x.MilkingId == milkingId);
            if (milking == null)
            {
                throw new Exception("Milking not found.");
            }
            await this.repository.DeleteAsync<Milking>(milking);
            return this.Ok();
        }

        [HttpGet("{fishId}")]
        public async Task<IEnumerable<Milking>> GetByFishId(int fishId)
        {
            var milking = await this.repository.GetRangeAsync<Milking>(true, x => x.FishId == fishId);
            if (milking == null)
            {
                throw new Exception("Milking not found.");
            }
            return milking.ToArray();
        }

        [HttpGet("{organizationId}")]
        public async Task<IEnumerable<Milking>> GetByOrganizationId(int organizationId)
        {
            var closedWaterSupplyInstallations = await this.repository.GetRangeAsync<ClosedWaterSupplyInstallation>(true, x => x.OrganizationId == organizationId);
            List<Milking> listMilking = new List<Milking>();
            foreach (ClosedWaterSupplyInstallation item in closedWaterSupplyInstallations)
            {
                var pools = await this.repository.GetRangeAsync<Pool>(true, x => x.ClosedWaterSupplyInstallationId == item.ClosedWaterSupplyInstallationId);
                foreach (Pool pool in pools)
                {
                    var fishsInThePool = await this.repository.GetRangeAsync<Fish>(true, x => x.PoolNowId == pool.PoolId);
                    foreach (Fish fish in fishsInThePool)
                    {
                        var milking = await this.repository.GetRangeAsync<Milking>(true, x => x.FishId == fish.FishId);
                        foreach (Milking milkingAtTheFish in milking)
                        {
                            listMilking.Add(milkingAtTheFish);
                        }
                    }

                }
            }
           
            return listMilking.ToArray();
        }
    }
}
