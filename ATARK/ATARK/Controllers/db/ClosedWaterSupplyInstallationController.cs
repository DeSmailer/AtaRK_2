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
    public class ClosedWaterSupplyInstallationController : ControllerBase
    {
        private readonly IRepository repository;

        public ClosedWaterSupplyInstallationController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ClosedWaterSupplyInstallation>> Get()
        {
            var closedWaterSupplyInstallation = await this.repository.GetRangeAsync<ClosedWaterSupplyInstallation>(true, x => true);
            return closedWaterSupplyInstallation.ToArray();
        }

        [HttpGet("{closedWaterSupplyInstallationId}")]
        public async Task<ClosedWaterSupplyInstallation> GetById(int closedWaterSupplyInstallationId)
        {
            var closedWaterSupplyInstallation = await this.repository.GetAsync<ClosedWaterSupplyInstallation>(true, x => x.ClosedWaterSupplyInstallationId == closedWaterSupplyInstallationId);
            if (closedWaterSupplyInstallation == null)
            {
                throw new Exception("ClosedWaterSupplyInstallation not found.");
            }
            return closedWaterSupplyInstallation;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClosedWaterSupplyInstallation closedWaterSupplyInstallation)
        {
            StateOfTheSystem stateOfTheSystem = new StateOfTheSystem();
            stateOfTheSystem.ClosedWaterSupplyInstallation = closedWaterSupplyInstallation;
            stateOfTheSystem.Temperature = 0f;
            stateOfTheSystem.OxygenLevel = 0f;
            stateOfTheSystem.DateOfLastCheck = DateTime.Now;

            await this.repository.AddAsync<StateOfTheSystem>(stateOfTheSystem);

            StateOfTheSystem oldStateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.ClosedWaterSupplyInstallation == closedWaterSupplyInstallation);
            int stateOfTheSystemId = oldStateOfTheSystem.StateOfTheSystemId;
            closedWaterSupplyInstallation.StateOfTheSystemId = stateOfTheSystemId;
            closedWaterSupplyInstallation.StateOfTheSystem = stateOfTheSystem;
            await this.repository.AddAsync<ClosedWaterSupplyInstallation>(closedWaterSupplyInstallation);


            Organization organization = await this.repository.GetAsync<Organization>(true, x => x.OrganizationId == closedWaterSupplyInstallation.OrganizationId);
            organization.ClosedWaterSupplyInstallations.Add(closedWaterSupplyInstallation);
            await this.repository.UpdateAsync<Organization>(organization);

            return this.Ok();

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClosedWaterSupplyInstallation closedWaterSupplyInstallation)
        {
            var currentClosedWaterSupplyInstallation = await this.repository.GetAsync<ClosedWaterSupplyInstallation>(true, x => x.OrganizationId == closedWaterSupplyInstallation.ClosedWaterSupplyInstallationId);
            if (currentClosedWaterSupplyInstallation == null)
            {
                throw new Exception("ClosedWaterSupplyInstallation not found.");
            }
            currentClosedWaterSupplyInstallation.OrganizationId = closedWaterSupplyInstallation.OrganizationId;
            currentClosedWaterSupplyInstallation.Location = closedWaterSupplyInstallation.Location;
            await this.repository.UpdateAsync<ClosedWaterSupplyInstallation>(currentClosedWaterSupplyInstallation);
            return this.Ok();
        }

        [HttpDelete("{closedWaterSupplyInstallationId}")]
        public async Task<IActionResult> Delete(int closedWaterSupplyInstallationId)
        {
            var closedWaterSupplyInstallation = await this.repository.GetAsync<ClosedWaterSupplyInstallation>(true, x => x.ClosedWaterSupplyInstallationId == closedWaterSupplyInstallationId);
            if (closedWaterSupplyInstallation == null)
            {
                throw new Exception("ClosedWaterSupplyInstallation not found.");
            }
            int stateOfTheSystemId = closedWaterSupplyInstallation.StateOfTheSystemId;
            var stateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.StateOfTheSystemId == stateOfTheSystemId);
            await this.repository.DeleteAsync<ClosedWaterSupplyInstallation>(closedWaterSupplyInstallation);
            await this.repository.DeleteAsync<StateOfTheSystem>(stateOfTheSystem);


            //var stateOfTheSystem = await this.repository.GetAsync<StateOfTheSystem>(true, x => x.StateOfTheSystemId == closedWaterSupplyInstallation.StateOfTheSystemId);
            //await this.repository.DeleteAsync<StateOfTheSystem>(stateOfTheSystem);
            return this.Ok();
        }
    }
}
