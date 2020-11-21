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
            await this.repository.AddAsync<Herd>(herd);

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
    }
}
