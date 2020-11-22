using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        [HttpGet]   
        public async Task<int> GetId([FromBody] Organization organization)
        {
            var currentOrganization = await this.repository.GetAsync<Organization>(true, x => (x.Mail == organization.Mail && x.Password == GetHashString(organization.Password)));
            return organization.OrganizationId;
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
            organization.Password = GetHashString(organization.Password);
            await this.repository.AddAsync<Organization>(organization);

            return this.Ok();
        }

        string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
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
            currentOrganization.Password = GetHashString(organization.Password);
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
    }
}
