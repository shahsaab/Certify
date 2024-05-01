using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Certify.Client
{
    public partial class CertifyAppService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public CertifyAppService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/CertifyApp/");
        }


        public async System.Threading.Tasks.Task ExportCustomersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/customers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCustomersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/customers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCustomers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Customer>> GetCustomers(Query query)
        {
            return await GetCustomers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Customer>> GetCustomers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Customers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCustomers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Customer>>(response);
        }

        partial void OnCreateCustomer(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Customer> CreateCustomer(Certify.Server.Models.CertifyApp.Customer customer = default(Certify.Server.Models.CertifyApp.Customer))
        {
            var uri = new Uri(baseUri, $"Customers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(customer), Encoding.UTF8, "application/json");

            OnCreateCustomer(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Customer>(response);
        }

        partial void OnDeleteCustomer(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCustomer(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Customers({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCustomer(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCustomerById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Customer> GetCustomerById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Customers({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCustomerById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Customer>(response);
        }

        partial void OnUpdateCustomer(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCustomer(int id = default(int), Certify.Server.Models.CertifyApp.Customer customer = default(Certify.Server.Models.CertifyApp.Customer))
        {
            var uri = new Uri(baseUri, $"Customers({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(customer), Encoding.UTF8, "application/json");

            OnUpdateCustomer(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMenusToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/menus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/menus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMenusToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/menus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/menus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMenus(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Menu>> GetMenus(Query query)
        {
            return await GetMenus(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Menu>> GetMenus(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Menus");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMenus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Menu>>(response);
        }

        partial void OnCreateMenu(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Menu> CreateMenu(Certify.Server.Models.CertifyApp.Menu menu = default(Certify.Server.Models.CertifyApp.Menu))
        {
            var uri = new Uri(baseUri, $"Menus");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(menu), Encoding.UTF8, "application/json");

            OnCreateMenu(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Menu>(response);
        }

        partial void OnDeleteMenu(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMenu(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Menus({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMenu(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMenuById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Menu> GetMenuById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Menus({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMenuById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Menu>(response);
        }

        partial void OnUpdateMenu(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMenu(int id = default(int), Certify.Server.Models.CertifyApp.Menu menu = default(Certify.Server.Models.CertifyApp.Menu))
        {
            var uri = new Uri(baseUri, $"Menus({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(menu), Encoding.UTF8, "application/json");

            OnUpdateMenu(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/products/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/products/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetProducts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Product>> GetProducts(Query query)
        {
            return await GetProducts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Product>> GetProducts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Products");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProducts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Product>>(response);
        }

        partial void OnCreateProduct(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Product> CreateProduct(Certify.Server.Models.CertifyApp.Product product = default(Certify.Server.Models.CertifyApp.Product))
        {
            var uri = new Uri(baseUri, $"Products");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            OnCreateProduct(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Product>(response);
        }

        partial void OnDeleteProduct(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteProduct(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetProductById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Product> GetProductById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetProductById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Product>(response);
        }

        partial void OnUpdateProduct(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateProduct(int id = default(int), Certify.Server.Models.CertifyApp.Product product = default(Certify.Server.Models.CertifyApp.Product))
        {
            var uri = new Uri(baseUri, $"Products({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            OnUpdateProduct(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportRolesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/roles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportRolesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/roles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetRoles(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Role>> GetRoles(Query query)
        {
            return await GetRoles(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Role>> GetRoles(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Roles");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoles(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Role>>(response);
        }

        partial void OnCreateRole(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Role> CreateRole(Certify.Server.Models.CertifyApp.Role role = default(Certify.Server.Models.CertifyApp.Role))
        {
            var uri = new Uri(baseUri, $"Roles");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            OnCreateRole(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Role>(response);
        }

        partial void OnDeleteRole(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteRole(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Roles({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetRoleById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Role> GetRoleById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Roles({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoleById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Role>(response);
        }

        partial void OnUpdateRole(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateRole(int id = default(int), Certify.Server.Models.CertifyApp.Role role = default(Certify.Server.Models.CertifyApp.Role))
        {
            var uri = new Uri(baseUri, $"Roles({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            OnUpdateRole(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportRoleMenuMappingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/rolemenumappings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/rolemenumappings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportRoleMenuMappingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/rolemenumappings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/rolemenumappings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetRoleMenuMappings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.RoleMenuMapping>> GetRoleMenuMappings(Query query)
        {
            return await GetRoleMenuMappings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.RoleMenuMapping>> GetRoleMenuMappings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"RoleMenuMappings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoleMenuMappings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.RoleMenuMapping>>(response);
        }

        partial void OnCreateRoleMenuMapping(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> CreateRoleMenuMapping(Certify.Server.Models.CertifyApp.RoleMenuMapping roleMenuMapping = default(Certify.Server.Models.CertifyApp.RoleMenuMapping))
        {
            var uri = new Uri(baseUri, $"RoleMenuMappings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(roleMenuMapping), Encoding.UTF8, "application/json");

            OnCreateRoleMenuMapping(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.RoleMenuMapping>(response);
        }

        partial void OnDeleteRoleMenuMapping(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteRoleMenuMapping(int id = default(int))
        {
            var uri = new Uri(baseUri, $"RoleMenuMappings({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteRoleMenuMapping(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetRoleMenuMappingById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.RoleMenuMapping> GetRoleMenuMappingById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"RoleMenuMappings({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetRoleMenuMappingById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.RoleMenuMapping>(response);
        }

        partial void OnUpdateRoleMenuMapping(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateRoleMenuMapping(int id = default(int), Certify.Server.Models.CertifyApp.RoleMenuMapping roleMenuMapping = default(Certify.Server.Models.CertifyApp.RoleMenuMapping))
        {
            var uri = new Uri(baseUri, $"RoleMenuMappings({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(roleMenuMapping), Encoding.UTF8, "application/json");

            OnUpdateRoleMenuMapping(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/stores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/stores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStores(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Store>> GetStores(Query query)
        {
            return await GetStores(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Store>> GetStores(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Stores");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStores(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.Store>>(response);
        }

        partial void OnCreateStore(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Store> CreateStore(Certify.Server.Models.CertifyApp.Store store = default(Certify.Server.Models.CertifyApp.Store))
        {
            var uri = new Uri(baseUri, $"Stores");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(store), Encoding.UTF8, "application/json");

            OnCreateStore(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Store>(response);
        }

        partial void OnDeleteStore(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStore(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStoreById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.Store> GetStoreById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStoreById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.Store>(response);
        }

        partial void OnUpdateStore(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStore(int id = default(int), Certify.Server.Models.CertifyApp.Store store = default(Certify.Server.Models.CertifyApp.Store))
        {
            var uri = new Uri(baseUri, $"Stores({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(store), Encoding.UTF8, "application/json");

            OnUpdateStore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/certifyapp/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/certifyapp/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetUsers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.User>> GetUsers(Query query)
        {
            return await GetUsers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.User>> GetUsers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Users");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUsers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<Certify.Server.Models.CertifyApp.User>>(response);
        }

        partial void OnCreateUser(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.User> CreateUser(Certify.Server.Models.CertifyApp.User user = default(Certify.Server.Models.CertifyApp.User))
        {
            var uri = new Uri(baseUri, $"Users");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            OnCreateUser(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.User>(response);
        }

        partial void OnDeleteUser(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteUser(int id = default(int))
        {
            var uri = new Uri(baseUri, $"Users({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteUser(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetUserById(HttpRequestMessage requestMessage);

        public async Task<Certify.Server.Models.CertifyApp.User> GetUserById(string expand = default(string), int id = default(int))
        {
            var uri = new Uri(baseUri, $"Users({id})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetUserById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Certify.Server.Models.CertifyApp.User>(response);
        }

        partial void OnUpdateUser(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateUser(int id = default(int), Certify.Server.Models.CertifyApp.User user = default(Certify.Server.Models.CertifyApp.User))
        {
            var uri = new Uri(baseUri, $"Users({id})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            OnUpdateUser(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}