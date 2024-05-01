using System.Data;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Certify.Server.Data;

namespace Certify.Server
{
    public partial class CertifyAppService
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

        public CertifyAppService(CertifyAppContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportCustomersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCustomersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCustomersRead(ref IQueryable<Certify.Server.Models.CertifyApp.Customer> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.Customer>> GetCustomers(Query query = null)
        {
            var items = Context.Customers.AsQueryable();

            items = items.Include(i => i.User);
            items = items.Include(i => i.User1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCustomersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCustomerGet(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnGetCustomerById(ref IQueryable<Certify.Server.Models.CertifyApp.Customer> items);


        public async Task<Certify.Server.Models.CertifyApp.Customer> GetCustomerById(int id)
        {
            var items = Context.Customers
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.User);
            items = items.Include(i => i.User1);
 
            OnGetCustomerById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCustomerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCustomerCreated(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerCreated(Certify.Server.Models.CertifyApp.Customer item);

        public async Task<Certify.Server.Models.CertifyApp.Customer> CreateCustomer(Certify.Server.Models.CertifyApp.Customer customer)
        {
            OnCustomerCreated(customer);

            var existingItem = Context.Customers
                              .Where(i => i.Id == customer.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Customers.Add(customer);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(customer).State = EntityState.Detached;
                throw;
            }

            OnAfterCustomerCreated(customer);

            return customer;
        }

        public async Task<Certify.Server.Models.CertifyApp.Customer> CancelCustomerChanges(Certify.Server.Models.CertifyApp.Customer item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCustomerUpdated(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerUpdated(Certify.Server.Models.CertifyApp.Customer item);

        public async Task<Certify.Server.Models.CertifyApp.Customer> UpdateCustomer(int id, Certify.Server.Models.CertifyApp.Customer customer)
        {
            OnCustomerUpdated(customer);

            var itemToUpdate = Context.Customers
                              .Where(i => i.Id == customer.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(customer);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCustomerUpdated(customer);

            return customer;
        }

        partial void OnCustomerDeleted(Certify.Server.Models.CertifyApp.Customer item);
        partial void OnAfterCustomerDeleted(Certify.Server.Models.CertifyApp.Customer item);

        public async Task<Certify.Server.Models.CertifyApp.Customer> DeleteCustomer(int id)
        {
            var itemToDelete = Context.Customers
                              .Where(i => i.Id == id)
                              .Include(i => i.Products)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCustomerDeleted(itemToDelete);


            Context.Customers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCustomerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMenusToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/menus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/menus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMenusToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/menus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/menus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMenusRead(ref IQueryable<Certify.Server.Models.CertifyApp.Menu> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.Menu>> GetMenus(Query query = null)
        {
            var items = Context.Menus.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMenusRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMenuGet(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnGetMenuById(ref IQueryable<Certify.Server.Models.CertifyApp.Menu> items);


        public async Task<Certify.Server.Models.CertifyApp.Menu> GetMenuById(int id)
        {
            var items = Context.Menus
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetMenuById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMenuGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMenuCreated(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuCreated(Certify.Server.Models.CertifyApp.Menu item);

        public async Task<Certify.Server.Models.CertifyApp.Menu> CreateMenu(Certify.Server.Models.CertifyApp.Menu menu)
        {
            OnMenuCreated(menu);

            var existingItem = Context.Menus
                              .Where(i => i.Id == menu.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Menus.Add(menu);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(menu).State = EntityState.Detached;
                throw;
            }

            OnAfterMenuCreated(menu);

            return menu;
        }

        public async Task<Certify.Server.Models.CertifyApp.Menu> CancelMenuChanges(Certify.Server.Models.CertifyApp.Menu item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMenuUpdated(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuUpdated(Certify.Server.Models.CertifyApp.Menu item);

        public async Task<Certify.Server.Models.CertifyApp.Menu> UpdateMenu(int id, Certify.Server.Models.CertifyApp.Menu menu)
        {
            OnMenuUpdated(menu);

            var itemToUpdate = Context.Menus
                              .Where(i => i.Id == menu.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(menu);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMenuUpdated(menu);

            return menu;
        }

        partial void OnMenuDeleted(Certify.Server.Models.CertifyApp.Menu item);
        partial void OnAfterMenuDeleted(Certify.Server.Models.CertifyApp.Menu item);

        public async Task<Certify.Server.Models.CertifyApp.Menu> DeleteMenu(int id)
        {
            var itemToDelete = Context.Menus
                              .Where(i => i.Id == id)
                              .Include(i => i.RoleMenuMappings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMenuDeleted(itemToDelete);


            Context.Menus.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMenuDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProductsRead(ref IQueryable<Certify.Server.Models.CertifyApp.Product> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.Product>> GetProducts(Query query = null)
        {
            var items = Context.Products.AsQueryable();

            items = items.Include(i => i.User);
            items = items.Include(i => i.Customer);
            items = items.Include(i => i.User1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProductGet(Certify.Server.Models.CertifyApp.Product item);
        partial void OnGetProductById(ref IQueryable<Certify.Server.Models.CertifyApp.Product> items);


        public async Task<Certify.Server.Models.CertifyApp.Product> GetProductById(int id)
        {
            var items = Context.Products
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.User);
            items = items.Include(i => i.Customer);
            items = items.Include(i => i.User1);
 
            OnGetProductById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProductCreated(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductCreated(Certify.Server.Models.CertifyApp.Product item);

        public async Task<Certify.Server.Models.CertifyApp.Product> CreateProduct(Certify.Server.Models.CertifyApp.Product product)
        {
            OnProductCreated(product);

            var existingItem = Context.Products
                              .Where(i => i.Id == product.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Products.Add(product);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(product).State = EntityState.Detached;
                throw;
            }

            OnAfterProductCreated(product);

            return product;
        }

        public async Task<Certify.Server.Models.CertifyApp.Product> CancelProductChanges(Certify.Server.Models.CertifyApp.Product item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProductUpdated(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductUpdated(Certify.Server.Models.CertifyApp.Product item);

        public async Task<Certify.Server.Models.CertifyApp.Product> UpdateProduct(int id, Certify.Server.Models.CertifyApp.Product product)
        {
            OnProductUpdated(product);

            var itemToUpdate = Context.Products
                              .Where(i => i.Id == product.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(product);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProductUpdated(product);

            return product;
        }

        partial void OnProductDeleted(Certify.Server.Models.CertifyApp.Product item);
        partial void OnAfterProductDeleted(Certify.Server.Models.CertifyApp.Product item);

        public async Task<Certify.Server.Models.CertifyApp.Product> DeleteProduct(int id)
        {
            var itemToDelete = Context.Products
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProductDeleted(itemToDelete);


            Context.Products.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProductDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRolesRead(ref IQueryable<Certify.Server.Models.CertifyApp.Role> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.Role>> GetRoles(Query query = null)
        {
            var items = Context.Roles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnRolesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRoleGet(Certify.Server.Models.CertifyApp.Role item);
        partial void OnGetRoleById(ref IQueryable<Certify.Server.Models.CertifyApp.Role> items);


        public async Task<Certify.Server.Models.CertifyApp.Role> GetRoleById(int id)
        {
            var items = Context.Roles
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetRoleById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnRoleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnRoleCreated(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleCreated(Certify.Server.Models.CertifyApp.Role item);

        public async Task<Certify.Server.Models.CertifyApp.Role> CreateRole(Certify.Server.Models.CertifyApp.Role role)
        {
            OnRoleCreated(role);

            var existingItem = Context.Roles
                              .Where(i => i.Id == role.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Roles.Add(role);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(role).State = EntityState.Detached;
                throw;
            }

            OnAfterRoleCreated(role);

            return role;
        }

        public async Task<Certify.Server.Models.CertifyApp.Role> CancelRoleChanges(Certify.Server.Models.CertifyApp.Role item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnRoleUpdated(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleUpdated(Certify.Server.Models.CertifyApp.Role item);

        public async Task<Certify.Server.Models.CertifyApp.Role> UpdateRole(int id, Certify.Server.Models.CertifyApp.Role role)
        {
            OnRoleUpdated(role);

            var itemToUpdate = Context.Roles
                              .Where(i => i.Id == role.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(role);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterRoleUpdated(role);

            return role;
        }

        partial void OnRoleDeleted(Certify.Server.Models.CertifyApp.Role item);
        partial void OnAfterRoleDeleted(Certify.Server.Models.CertifyApp.Role item);

        public async Task<Certify.Server.Models.CertifyApp.Role> DeleteRole(int id)
        {
            var itemToDelete = Context.Roles
                              .Where(i => i.Id == id)
                              .Include(i => i.RoleMenuMappings)
                              .Include(i => i.Users)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRoleDeleted(itemToDelete);


            Context.Roles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRoleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportRoleMenuMappingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/rolemenumappings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/rolemenumappings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRoleMenuMappingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/rolemenumappings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/rolemenumappings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRoleMenuMappingsRead(ref IQueryable<Certify.Server.Models.CertifyApp.RoleMenuMapping> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.RoleMenuMapping>> GetRoleMenuMappings(Query query = null)
        {
            var items = Context.RoleMenuMappings.AsQueryable();

            items = items.Include(i => i.Menu);
            items = items.Include(i => i.Role);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnRoleMenuMappingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRoleMenuMappingGet(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnGetRoleMenuMappingById(ref IQueryable<Certify.Server.Models.CertifyApp.RoleMenuMapping> items);


        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> GetRoleMenuMappingById(int id)
        {
            var items = Context.RoleMenuMappings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Menu);
            items = items.Include(i => i.Role);
 
            OnGetRoleMenuMappingById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnRoleMenuMappingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnRoleMenuMappingCreated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingCreated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> CreateRoleMenuMapping(Certify.Server.Models.CertifyApp.RoleMenuMapping rolemenumapping)
        {
            OnRoleMenuMappingCreated(rolemenumapping);

            var existingItem = Context.RoleMenuMappings
                              .Where(i => i.Id == rolemenumapping.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.RoleMenuMappings.Add(rolemenumapping);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(rolemenumapping).State = EntityState.Detached;
                throw;
            }

            OnAfterRoleMenuMappingCreated(rolemenumapping);

            return rolemenumapping;
        }

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> CancelRoleMenuMappingChanges(Certify.Server.Models.CertifyApp.RoleMenuMapping item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnRoleMenuMappingUpdated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingUpdated(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> UpdateRoleMenuMapping(int id, Certify.Server.Models.CertifyApp.RoleMenuMapping rolemenumapping)
        {
            OnRoleMenuMappingUpdated(rolemenumapping);

            var itemToUpdate = Context.RoleMenuMappings
                              .Where(i => i.Id == rolemenumapping.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(rolemenumapping);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterRoleMenuMappingUpdated(rolemenumapping);

            return rolemenumapping;
        }

        partial void OnRoleMenuMappingDeleted(Certify.Server.Models.CertifyApp.RoleMenuMapping item);
        partial void OnAfterRoleMenuMappingDeleted(Certify.Server.Models.CertifyApp.RoleMenuMapping item);

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> DeleteRoleMenuMapping(int id)
        {
            var itemToDelete = Context.RoleMenuMappings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRoleMenuMappingDeleted(itemToDelete);


            Context.RoleMenuMappings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRoleMenuMappingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStoresRead(ref IQueryable<Certify.Server.Models.CertifyApp.Store> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.Store>> GetStores(Query query = null)
        {
            var items = Context.Stores.AsQueryable();

            items = items.Include(i => i.User);
            items = items.Include(i => i.User1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStoresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStoreGet(Certify.Server.Models.CertifyApp.Store item);
        partial void OnGetStoreById(ref IQueryable<Certify.Server.Models.CertifyApp.Store> items);


        public async Task<Certify.Server.Models.CertifyApp.Store> GetStoreById(int id)
        {
            var items = Context.Stores
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.User);
            items = items.Include(i => i.User1);
 
            OnGetStoreById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStoreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStoreCreated(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreCreated(Certify.Server.Models.CertifyApp.Store item);

        public async Task<Certify.Server.Models.CertifyApp.Store> CreateStore(Certify.Server.Models.CertifyApp.Store store)
        {
            OnStoreCreated(store);

            var existingItem = Context.Stores
                              .Where(i => i.Id == store.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Stores.Add(store);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(store).State = EntityState.Detached;
                throw;
            }

            OnAfterStoreCreated(store);

            return store;
        }

        public async Task<Certify.Server.Models.CertifyApp.Store> CancelStoreChanges(Certify.Server.Models.CertifyApp.Store item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStoreUpdated(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreUpdated(Certify.Server.Models.CertifyApp.Store item);

        public async Task<Certify.Server.Models.CertifyApp.Store> UpdateStore(int id, Certify.Server.Models.CertifyApp.Store store)
        {
            OnStoreUpdated(store);

            var itemToUpdate = Context.Stores
                              .Where(i => i.Id == store.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(store);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStoreUpdated(store);

            return store;
        }

        partial void OnStoreDeleted(Certify.Server.Models.CertifyApp.Store item);
        partial void OnAfterStoreDeleted(Certify.Server.Models.CertifyApp.Store item);

        public async Task<Certify.Server.Models.CertifyApp.Store> DeleteStore(int id)
        {
            var itemToDelete = Context.Stores
                              .Where(i => i.Id == id)
                              .Include(i => i.Users)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStoreDeleted(itemToDelete);


            Context.Stores.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStoreDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUsersRead(ref IQueryable<Certify.Server.Models.CertifyApp.User> items);

        public async Task<IQueryable<Certify.Server.Models.CertifyApp.User>> GetUsers(Query query = null)
        {
            var items = Context.Users.AsQueryable();

            items = items.Include(i => i.User1);
            items = items.Include(i => i.User2);
            items = items.Include(i => i.Role);
            items = items.Include(i => i.Store);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserGet(Certify.Server.Models.CertifyApp.User item);
        partial void OnGetUserById(ref IQueryable<Certify.Server.Models.CertifyApp.User> items);


        public async Task<Certify.Server.Models.CertifyApp.User> GetUserById(int id)
        {
            var items = Context.Users
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.User1);
            items = items.Include(i => i.User2);
            items = items.Include(i => i.Role);
            items = items.Include(i => i.Store);
 
            OnGetUserById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnUserGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnUserCreated(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserCreated(Certify.Server.Models.CertifyApp.User item);

        public async Task<Certify.Server.Models.CertifyApp.User> CreateUser(Certify.Server.Models.CertifyApp.User user)
        {
            OnUserCreated(user);

            var existingItem = Context.Users
                              .Where(i => i.Id == user.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Users.Add(user);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(user).State = EntityState.Detached;
                throw;
            }

            OnAfterUserCreated(user);

            return user;
        }

        public async Task<Certify.Server.Models.CertifyApp.User> CancelUserChanges(Certify.Server.Models.CertifyApp.User item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUserUpdated(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserUpdated(Certify.Server.Models.CertifyApp.User item);

        public async Task<Certify.Server.Models.CertifyApp.User> UpdateUser(int id, Certify.Server.Models.CertifyApp.User user)
        {
            OnUserUpdated(user);

            var itemToUpdate = Context.Users
                              .Where(i => i.Id == user.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(user);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterUserUpdated(user);

            return user;
        }

        partial void OnUserDeleted(Certify.Server.Models.CertifyApp.User item);
        partial void OnAfterUserDeleted(Certify.Server.Models.CertifyApp.User item);

        public async Task<Certify.Server.Models.CertifyApp.User> DeleteUser(int id)
        {
            var itemToDelete = Context.Users
                              .Where(i => i.Id == id)
                              .Include(i => i.Customers)
                              .Include(i => i.Customers1)
                              .Include(i => i.Products)
                              .Include(i => i.Products1)
                              .Include(i => i.Stores)
                              .Include(i => i.Stores1)
                              .Include(i => i.Users1)
                              .Include(i => i.Users2)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnUserDeleted(itemToDelete);


            Context.Users.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUserDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}