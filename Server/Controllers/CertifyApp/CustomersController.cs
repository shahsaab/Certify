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
    [Route("odata/CertifyApp/Customers")]
    public partial class CustomersController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public CustomersController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.Customer> GetCustomers()
        {
            var items = this.context.Customers.AsQueryable<Certify.Server.Models.CertifyApp.Customer>();
            this.OnCustomersRead(ref items);

            return items;
        }

        partial void OnCustomersRead(ref IQueryable<Certify.Server.Models.CertifyApp.Customer> items);

        partial void OnCustomerGet(ref SingleResult<Certify.Server.Models.CertifyApp.Customer> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Customers(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.Customer> GetCustomer(int key)
        {
            var items = this.context.Customers.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnCustomerGet(ref result);

            return result;
        }
        partial void OnCustomerDeleted(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerDeleted(Certify.Server.Models.CertifyApp.Customer item);

        [HttpDelete("/odata/CertifyApp/Customers(Id={Id})")]
        public IActionResult DeleteCustomer(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Customers
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCustomerDeleted(item);
                this.context.Customers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCustomerDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCustomerUpdated(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerUpdated(Certify.Server.Models.CertifyApp.Customer item);

        [HttpPut("/odata/CertifyApp/Customers(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCustomer(int key, [FromBody]Certify.Server.Models.CertifyApp.Customer item)
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
                this.OnCustomerUpdated(item);
                this.context.Customers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Customers.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");
                this.OnAfterCustomerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Customers(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCustomer(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.Customer> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Customers.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCustomerUpdated(item);
                this.context.Customers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Customers.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");
                this.OnAfterCustomerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCustomerCreated(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerCreated(Certify.Server.Models.CertifyApp.Customer item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.Customer item)
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

                this.OnCustomerCreated(item);
                this.context.Customers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Customers.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "User,User1");

                this.OnAfterCustomerCreated(item);

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
