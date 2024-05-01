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
    [Route("odata/CertifyApp/Products")]
    public partial class ProductsController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public ProductsController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.Product> GetProducts()
        {
            var items = this.context.Products.AsQueryable<Certify.Server.Models.CertifyApp.Product>();
            this.OnProductsRead(ref items);

            return items;
        }

        partial void OnProductsRead(ref IQueryable<Certify.Server.Models.CertifyApp.Product> items);

        partial void OnProductGet(ref SingleResult<Certify.Server.Models.CertifyApp.Product> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Products(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.Product> GetProduct(int key)
        {
            var items = this.context.Products.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnProductGet(ref result);

            return result;
        }
        partial void OnProductDeleted(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductDeleted(Certify.Server.Models.CertifyApp.Product item);

        [HttpDelete("/odata/CertifyApp/Products(Id={Id})")]
        public IActionResult DeleteProduct(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Products
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnProductDeleted(item);
                this.context.Products.Remove(item);
                this.context.SaveChanges();
                this.OnAfterProductDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProductUpdated(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductUpdated(Certify.Server.Models.CertifyApp.Product item);

        [HttpPut("/odata/CertifyApp/Products(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutProduct(int key, [FromBody]Certify.Server.Models.CertifyApp.Product item)
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
                this.OnProductUpdated(item);
                this.context.Products.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,Customer,User1");
                this.OnAfterProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Products(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchProduct(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.Product> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Products.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnProductUpdated(item);
                this.context.Products.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,Customer,User1");
                this.OnAfterProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProductCreated(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductCreated(Certify.Server.Models.CertifyApp.Product item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.Product item)
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

                this.OnProductCreated(item);
                this.context.Products.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Products.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "User,Customer,User1");

                this.OnAfterProductCreated(item);

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
