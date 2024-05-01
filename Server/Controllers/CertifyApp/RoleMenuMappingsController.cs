using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Certify.Server.Controllers.CertifyApp
{
    [Route("odata/CertifyApp/RoleMenuMappings")]
    public partial class RoleMenuMappingsController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public RoleMenuMappingsController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.RoleMenuMapping> GetRoleMenuMappings()
        {
            var items = this.context.RoleMenuMappings.AsQueryable<Certify.Server.Models.CertifyApp.RoleMenuMapping>();
            this.OnRoleMenuMappingsRead(ref items);

            return items;
        }

        partial void OnRoleMenuMappingsRead(ref IQueryable<Certify.Server.Models.CertifyApp.RoleMenuMapping> items);

        partial void OnRoleMenuMappingGet(ref SingleResult<Certify.Server.Models.CertifyApp.RoleMenuMapping> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/RoleMenuMappings(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.RoleMenuMapping> GetRoleMenuMapping(int key)
        {
            var items = this.context.RoleMenuMappings.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnRoleMenuMappingGet(ref result);

            return result;
        }
        partial void OnRoleMenuMappingDeleted(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingDeleted(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        [HttpDelete("/odata/CertifyApp/RoleMenuMappings(Id={Id})")]
        public IActionResult DeleteRoleMenuMapping(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.RoleMenuMappings
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnRoleMenuMappingDeleted(item);
                this.context.RoleMenuMappings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterRoleMenuMappingDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleMenuMappingUpdated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingUpdated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        [HttpPut("/odata/CertifyApp/RoleMenuMappings(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutRoleMenuMapping(int key, [FromBody]Certify.Server.Models.CertifyApp.RoleMenuMapping item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != key))
                {
                    return BadRequest();
                }
                this.OnRoleMenuMappingUpdated(item);
                this.context.RoleMenuMappings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleMenuMappings.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Menu,Role");
                this.OnAfterRoleMenuMappingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/RoleMenuMappings(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchRoleMenuMapping(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.RoleMenuMapping> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.RoleMenuMappings.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnRoleMenuMappingUpdated(item);
                this.context.RoleMenuMappings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleMenuMappings.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Menu,Role");
                this.OnAfterRoleMenuMappingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleMenuMappingCreated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingCreated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.RoleMenuMapping item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnRoleMenuMappingCreated(item);
                this.context.RoleMenuMappings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleMenuMappings.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "Menu,Role");

                this.OnAfterRoleMenuMappingCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
