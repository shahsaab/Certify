using System.Data;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Certify.Server.Data;

namespace Certify.Server
{
    public partial class CustomService
    {
        CertifyAppContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly CertifyAppContext context;
        private readonly NavigationManager navigationManager;

        public CustomService(CertifyAppContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public async Task<Certify.Server.Models.CertifyApp.User> Authenticate(string username, string password)
        {
            var items = Context.Users
                              .AsNoTracking()
                              .Where(i => i.Username == username && i.Password == password);

            items = items.Include(i => i.Role);
            items = items.Include(i => i.Store);

            var itemToReturn = items.FirstOrDefault();

            return await Task.FromResult(itemToReturn);
        }
    }
}