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
    [Route("odata/CertifyApp/Menus")]
    public partial class MenusController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public MenusController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.Menu> GetMenus()
        {
            var items = this.context.Menus.AsQueryable<Certify.Server.Models.CertifyApp.Menu>();
            this.OnMenusRead(ref items);

            return items;
        }

        partial void OnMenusRead(ref IQueryable<Certify.Server.Models.CertifyApp.Menu> items);

        partial void OnMenuGet(ref SingleResult<Certify.Server.Models.CertifyApp.Menu> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Menus(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.Menu> GetMenu(int key)
        {
            var items = this.context.Menus.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnMenuGet(ref result);

            return result;
        }
        partial void OnMenuDeleted(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuDeleted(Certify.Server.Models.CertifyApp.Menu item);

        [HttpDelete("/odata/CertifyApp/Menus(Id={Id})")]
        public IActionResult DeleteMenu(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Menus
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnMenuDeleted(item);
                this.context.Menus.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMenuDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMenuUpdated(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuUpdated(Certify.Server.Models.CertifyApp.Menu item);

        [HttpPut("/odata/CertifyApp/Menus(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMenu(int key, [FromBody]Certify.Server.Models.CertifyApp.Menu item)
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
                this.OnMenuUpdated(item);
                this.context.Menus.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Menus.Where(i => i.Id == key);
                
                this.OnAfterMenuUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Menus(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMenu(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.Menu> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Menus.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnMenuUpdated(item);
                this.context.Menus.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Menus.Where(i => i.Id == key);
                
                this.OnAfterMenuUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMenuCreated(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuCreated(Certify.Server.Models.CertifyApp.Menu item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.Menu item)
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

                this.OnMenuCreated(item);
                this.context.Menus.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Menus.Where(i => i.Id == item.Id);

                

                this.OnAfterMenuCreated(item);

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
