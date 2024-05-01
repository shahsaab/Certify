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
    public partial class Users
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

        protected IEnumerable<Certify.Server.Models.CertifyApp.User> users;

        protected RadzenDataGrid<Certify.Server.Models.CertifyApp.User> grid0;
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
                var result = await CertifyAppService.GetUsers(filter: $@"(contains(Username,""{search}"") or contains(Password,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Store,Role,User1,User2", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                users = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Users" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            user = new Certify.Server.Models.CertifyApp.User();
        }

        protected async Task EditRow(Certify.Server.Models.CertifyApp.User args)
        {
            isEdit = true;
            user = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Certify.Server.Models.CertifyApp.User user)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CertifyAppService.DeleteUser(id:user.Id);

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
                    Detail = $"Unable to delete User"
                });
            }
        }
        protected bool errorVisible;
        protected Certify.Server.Models.CertifyApp.User user;

        protected IEnumerable<Certify.Server.Models.CertifyApp.Store> storesForStoreId;

        protected IEnumerable<Certify.Server.Models.CertifyApp.Role> rolesForRoleId;

        protected IEnumerable<Certify.Server.Models.CertifyApp.User> usersForCreatedBy;

        protected IEnumerable<Certify.Server.Models.CertifyApp.User> usersForModifiedBy;


        protected int storesForStoreIdCount;
        protected Certify.Server.Models.CertifyApp.Store storesForStoreIdValue;
        protected async Task storesForStoreIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetStores(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Title, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                storesForStoreId = result.Value.AsODataEnumerable();
                storesForStoreIdCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Store" });
            }
        }

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

        protected int usersForCreatedByCount;
        protected Certify.Server.Models.CertifyApp.User usersForCreatedByValue;
        protected async Task usersForCreatedByLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetUsers(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Username, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                usersForCreatedBy = result.Value.AsODataEnumerable();
                usersForCreatedByCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load User1" });
            }
        }

        protected int usersForModifiedByCount;
        protected Certify.Server.Models.CertifyApp.User usersForModifiedByValue;
        protected async Task usersForModifiedByLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await CertifyAppService.GetUsers(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Username, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                usersForModifiedBy = result.Value.AsODataEnumerable();
                usersForModifiedByCount = result.Count;

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load User2" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CertifyAppService.UpdateUser(id:user.Id, user) : await CertifyAppService.CreateUser(user);

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