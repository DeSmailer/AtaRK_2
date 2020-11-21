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
    [Route("api/[controller]")]
    [ApiController]
    public class StateOfTheSystemController : ControllerBase
    {
        private readonly IRepository repository;

        public StateOfTheSystemController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<StateOfTheSystem>> Get()
        {
            var stateOfTheSystem = await this.repository.GetRangeAsync<StateOfTheSystem>(true, x => true);
            return stateOfTheSystem.ToArray();
        }

        [HttpGet("{stateOfTheSystemId}")]
        public async Task<StateOfTheSystem> GetById(int stateOfTheSystemId)
        {
            var stateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.StateOfTheSystemId == stateOfTheSystemId);
            if (stateOfTheSystem == null)
            {
                throw new Exception("StateOfTheSystem not found.");
            }
            return stateOfTheSystem;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StateOfTheSystem stateOfTheSystem)
        {
            await this.repository.AddAsync<StateOfTheSystem>(stateOfTheSystem);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StateOfTheSystem stateOfTheSystem)
        {
            var currentStateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.StateOfTheSystemId == stateOfTheSystem.StateOfTheSystemId);
            if (currentStateOfTheSystem == null)
            {
                throw new Exception("StateOfTheSystem not found.");
            }
            currentStateOfTheSystem.Temperature = stateOfTheSystem.Temperature;
            currentStateOfTheSystem.OxygenLevel = stateOfTheSystem.OxygenLevel;
            currentStateOfTheSystem.DateOfLastCheck = stateOfTheSystem.DateOfLastCheck;
            await this.repository.UpdateAsync<StateOfTheSystem>(currentStateOfTheSystem);
            return this.Ok();

    }

        [HttpDelete("{stateOfTheSystemId}")]
        public async Task<IActionResult> Delete(int stateOfTheSystemId)
        {
            var stateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.StateOfTheSystemId == stateOfTheSystemId);
            if (stateOfTheSystem == null)
            {
                throw new Exception("StateOfTheSystem not found.");
            }
            await this.repository.DeleteAsync<StateOfTheSystem>(stateOfTheSystem);
            return this.Ok();
        }
    }
}

