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
    [Route("odata/CertifyApp/Roles")]
    public partial class RolesController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public RolesController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.Role> GetRoles()
        {
            var items = this.context.Roles.AsQueryable<Certify.Server.Models.CertifyApp.Role>();
            this.OnRolesRead(ref items);

            return items;
        }

        partial void OnRolesRead(ref IQueryable<Certify.Server.Models.CertifyApp.Role> items);

        partial void OnRoleGet(ref SingleResult<Certify.Server.Models.CertifyApp.Role> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Roles(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.Role> GetRole(int key)
        {
            var items = this.context.Roles.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnRoleGet(ref result);

            return result;
        }
        partial void OnRoleDeleted(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleDeleted(Certify.Server.Models.CertifyApp.Role item);

        [HttpDelete("/odata/CertifyApp/Roles(Id={Id})")]
        public IActionResult DeleteRole(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Roles
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnRoleDeleted(item);
                this.context.Roles.Remove(item);
                this.context.SaveChanges();
                this.OnAfterRoleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleUpdated(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleUpdated(Certify.Server.Models.CertifyApp.Role item);

        [HttpPut("/odata/CertifyApp/Roles(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutRole(int key, [FromBody]Certify.Server.Models.CertifyApp.Role item)
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
                this.OnRoleUpdated(item);
                this.context.Roles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Roles.Where(i => i.Id == key);
                
                this.OnAfterRoleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Roles(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchRole(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.Role> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Roles.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnRoleUpdated(item);
                this.context.Roles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Roles.Where(i => i.Id == key);
                
                this.OnAfterRoleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleCreated(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleCreated(Certify.Server.Models.CertifyApp.Role item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.Role item)
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

                this.OnRoleCreated(item);
                this.context.Roles.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Roles.Where(i => i.Id == item.Id);

                

                this.OnAfterRoleCreated(item);

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
