using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Certify.Server.Data;

namespace Certify.Server.Controllers
{
    public partial class ExportCertifyAppController : ExportController
    {
        private readonly CertifyAppContext context;
        private readonly CertifyAppService service;

        public ExportCertifyAppController(CertifyAppContext context, CertifyAppService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/CertifyApp/customers/csv")]
        [HttpGet("/export/CertifyApp/customers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCustomersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCustomers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/customers/excel")]
        [HttpGet("/export/CertifyApp/customers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCustomersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCustomers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/menus/csv")]
        [HttpGet("/export/CertifyApp/menus/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMenusToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMenus(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/menus/excel")]
        [HttpGet("/export/CertifyApp/menus/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMenusToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMenus(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/products/csv")]
        [HttpGet("/export/CertifyApp/products/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/products/excel")]
        [HttpGet("/export/CertifyApp/products/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/roles/csv")]
        [HttpGet("/export/CertifyApp/roles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRolesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetRoles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/roles/excel")]
        [HttpGet("/export/CertifyApp/roles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRolesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetRoles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/rolemenumappings/csv")]
        [HttpGet("/export/CertifyApp/rolemenumappings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRoleMenuMappingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetRoleMenuMappings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/rolemenumappings/excel")]
        [HttpGet("/export/CertifyApp/rolemenumappings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRoleMenuMappingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetRoleMenuMappings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/stores/csv")]
        [HttpGet("/export/CertifyApp/stores/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStores(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/stores/excel")]
        [HttpGet("/export/CertifyApp/stores/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStoresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStores(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/users/csv")]
        [HttpGet("/export/CertifyApp/users/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUsersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUsers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/CertifyApp/users/excel")]
        [HttpGet("/export/CertifyApp/users/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUsersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUsers(), Request.Query, false), fileName);
        }
    }
}
