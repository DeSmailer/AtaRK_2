using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATARK.Models.Entity;
using ATARK.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATARK.Controllers.db
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IRepository repository;

        public OrganizationController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Organization>> Get()
        {
            var organization = await this.repository.GetRangeAsync<Organization>(true, x => true);
            return organization.ToArray();
        }

        [HttpGet("{organizationId}")]
        public async Task<Organization> GetById(int organizationId)
        {
            var organization = await this.repository.GetAsync<Organization>(true, x => x.OrganizationId == organizationId);
            if (organization == null)
            {
                throw new Exception("Organization not found.");
            }
            return organization;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Organization organization)
        {
            await this.repository.AddAsync<Organization>(organization);

            return this.Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Organization organization)
        {
            var currentOrganization = await this.repository.GetAsync<Organization>(true, x => x.OrganizationId == organization.OrganizationId);
            if (currentOrganization == null)
            {
                throw new Exception("Organization not found.");
            }
            currentOrganization.Mail = organization.Mail;
            currentOrganization.Password = organization.Password;
            currentOrganization.Name = organization.Name;
            currentOrganization.FoundationDate = organization.FoundationDate;
            currentOrganization.PhoneNumber = organization.PhoneNumber;
            await this.repository.UpdateAsync<Organization>(currentOrganization);
            return this.Ok();
        }

        [HttpDelete("{organizationId}")]
        public async Task<IActionResult> Delete(int organizationId)
        {
            var organization = await this.repository.GetAsync<Organization>(true, x => x.OrganizationId == organizationId);
            if (organization == null)
            {
                throw new Exception("Organization not found.");
            }
            await this.repository.DeleteAsync<Organization>(organization);
            return this.Ok();
        }
        //{
        //  "OrganizationId" : "1",
        //  "mail": "organization1@gmail.com",
        //  "password": "qwe123",
        //  "name": "organization1",
        //  "foundationDate": "11/11/2011",
        //  "phoneNumber": "+380992196335"
        //}
    }
}
