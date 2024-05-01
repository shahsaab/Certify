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
    [Route("odata/CertifyApp/Stores")]
    public partial class StoresController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public StoresController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.Store> GetStores()
        {
            var items = this.context.Stores.AsQueryable<Certify.Server.Models.CertifyApp.Store>();
            this.OnStoresRead(ref items);

            return items;
        }

        partial void OnStoresRead(ref IQueryable<Certify.Server.Models.CertifyApp.Store> items);

        partial void OnStoreGet(ref SingleResult<Certify.Server.Models.CertifyApp.Store> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Stores(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.Store> GetStore(int key)
        {
            var items = this.context.Stores.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnStoreGet(ref result);

            return result;
        }
        partial void OnStoreDeleted(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreDeleted(Certify.Server.Models.CertifyApp.Store item);

        [HttpDelete("/odata/CertifyApp/Stores(Id={Id})")]
        public IActionResult DeleteStore(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Stores
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStoreDeleted(item);
                this.context.Stores.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStoreDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStoreUpdated(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreUpdated(Certify.Server.Models.CertifyApp.Store item);

        [HttpPut("/odata/CertifyApp/Stores(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStore(int key, [FromBody]Certify.Server.Models.CertifyApp.Store item)
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
                this.OnStoreUpdated(item);
                this.context.Stores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");
                this.OnAfterStoreUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Stores(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStore(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.Store> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Stores.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStoreUpdated(item);
                this.context.Stores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");
                this.OnAfterStoreUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStoreCreated(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreCreated(Certify.Server.Models.CertifyApp.Store item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.Store item)
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

                this.OnStoreCreated(item);
                this.context.Stores.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Stores.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");

                this.OnAfterStoreCreated(item);

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
