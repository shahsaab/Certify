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
    [Route("odata/CertifyApp/Users")]
    public partial class UsersController : ODataController
    {
        private Certify.Server.Data.CertifyAppContext context;

        public UsersController(Certify.Server.Data.CertifyAppContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Certify.Server.Models.CertifyApp.User> GetUsers()
        {
            var items = this.context.Users.AsQueryable<Certify.Server.Models.CertifyApp.User>();
            this.OnUsersRead(ref items);

            return items;
        }

        partial void OnUsersRead(ref IQueryable<Certify.Server.Models.CertifyApp.User> items);

        partial void OnUserGet(ref SingleResult<Certify.Server.Models.CertifyApp.User> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/CertifyApp/Users(Id={Id})")]
        public SingleResult<Certify.Server.Models.CertifyApp.User> GetUser(int key)
        {
            var items = this.context.Users.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnUserGet(ref result);

            return result;
        }

        [HttpGet("/odata/CertifyApp/Users(Username={username}, Password={password})")]
        public SingleResult<Certify.Server.Models.CertifyApp.User> Authenticate(string username, string password)
        {
            var items = this.context.Users.Where(i => i.Username == username && i.Password == password);
            var result = SingleResult.Create(items);
            return result;
        }
        
        partial void OnUserDeleted(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserDeleted(Certify.Server.Models.CertifyApp.User item);

        [HttpDelete("/odata/CertifyApp/Users(Id={Id})")]
        public IActionResult DeleteUser(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Users
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnUserDeleted(item);
                this.context.Users.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUserDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUserUpdated(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserUpdated(Certify.Server.Models.CertifyApp.User item);

        [HttpPut("/odata/CertifyApp/Users(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUser(int key, [FromBody]Certify.Server.Models.CertifyApp.User item)
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
                this.OnUserUpdated(item);
                this.context.Users.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Users.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User1,User2,Role,Store");
                this.OnAfterUserUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/CertifyApp/Users(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUser(int key, [FromBody]Delta<Certify.Server.Models.CertifyApp.User> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Users.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnUserUpdated(item);
                this.context.Users.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Users.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "User1,User2,Role,Store");
                this.OnAfterUserUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUserCreated(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserCreated(Certify.Server.Models.CertifyApp.User item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Certify.Server.Models.CertifyApp.User item)
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

                this.OnUserCreated(item);
                this.context.Users.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Users.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "User1,User2,Role,Store");

                this.OnAfterUserCreated(item);

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
