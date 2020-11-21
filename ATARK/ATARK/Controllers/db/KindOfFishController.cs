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
    public class KindOfFishController : ControllerBase
    {
        private readonly IRepository repository;

        public KindOfFishController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<KindOfFish>> Get()
        {
            var kindOfFish = await this.repository.GetRangeAsync<KindOfFish>(true, x => true);
            return kindOfFish.ToArray();
        }

        [HttpGet("{kindOfFishId}")]
        public async Task<KindOfFish> GetById(int kindOfFishId)
        {
            var kindOfFish = await this.repository.GetAsync<KindOfFish>(true, x => x.KindOfFishId == kindOfFishId);
            if (kindOfFish == null)
            {
                throw new Exception("KindOfFish not found.");
            }
            return kindOfFish;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] KindOfFish kindOfFish)
        {
            await this.repository.AddAsync<KindOfFish>(kindOfFish);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] KindOfFish kindOfFish)
        {
            var currentKindOfFish = await this.repository.GetAsync<KindOfFish>(true, x => x.KindOfFishId == kindOfFish.KindOfFishId);
            if (currentKindOfFish == null)
            {
                throw new Exception("KindOfFish not found.");
            }
            currentKindOfFish.Kind = kindOfFish.Kind;
            await this.repository.UpdateAsync<KindOfFish>(currentKindOfFish);
            return this.Ok();

        }

        [HttpDelete("{kindOfFishId}")]
        public async Task<IActionResult> Delete(int kindOfFishId)
        {
            var kindOfFish = await this.repository.GetAsync<KindOfFish>(true, x => x.KindOfFishId == kindOfFishId);
            if (kindOfFish == null)
            {
                throw new Exception("KindOfFish not found.");
            }
            await this.repository.DeleteAsync<KindOfFish>(kindOfFish);
            return this.Ok();
        }
    }
}
