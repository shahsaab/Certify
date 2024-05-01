using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Certify.Client.Pages
{
    public partial class RoleMenuMappings
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public CertifyAppService CertifyAppService { get; set; }

        protected IEnumerable<Certify.Server.Models.CertifyApp.RoleMenuMapping> roleMenuMappings;

        protected RadzenDataGrid<Certify.Server.Models.CertifyApp.RoleMenuMapping> grid0;
        protected int count;
        protected bool isEdit = true;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetRoleMenuMappings(filter: $"{args.Filter}", expand: "Role,Menu", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                roleMenuMappings = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load RoleMenuMappings" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            roleMenuMapping = new Certify.Server.Models.CertifyApp.RoleMenuMapping();
        }

        protected async Task EditRow(Certify.Server.Models.CertifyApp.RoleMenuMapping args)
        {
            isEdit = true;
            roleMenuMapping = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Certify.Server.Models.CertifyApp.RoleMenuMapping roleMenuMapping)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CertifyAppService.DeleteRoleMenuMapping(id:roleMenuMapping.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete RoleMenuMapping"
                });
            }
        }
        protected bool errorVisible;
        protected Certify.Server.Models.CertifyApp.RoleMenuMapping roleMenuMapping;

        protected IEnumerable<Certify.Server.Models.CertifyApp.Role> rolesForRoleId;

        protected IEnumerable<Certify.Server.Models.CertifyApp.Menu> menusForMenuId;


        protected int rolesForRoleIdCount;
        protected Certify.Server.Models.CertifyApp.Role rolesForRoleIdValue;
        protected async Task rolesForRoleIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetRoles(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                rolesForRoleId = result.Value.AsODataEnumerable();
                rolesForRoleIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Role" });
            }
        }

        protected int menusForMenuIdCount;
        protected Certify.Server.Models.CertifyApp.Menu menusForMenuIdValue;
        protected async Task menusForMenuIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetMenus(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                menusForMenuId = result.Value.AsODataEnumerable();
                menusForMenuIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Menu" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CertifyAppService.UpdateRoleMenuMapping(id:roleMenuMapping.Id, roleMenuMapping) : await CertifyAppService.CreateRoleMenuMapping(roleMenuMapping);

            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {

        }
    }
}