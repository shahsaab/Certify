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
    public partial class Customers
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

        protected IEnumerable<Certify.Server.Models.CertifyApp.Customer> customers;

        protected RadzenDataGrid<Certify.Server.Models.CertifyApp.Customer> grid0;
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
                var result = await CertifyAppService.GetCustomers(filter: $@"(contains(Name,""{search}"") or contains(Contact,""{search}"") or contains(NIC,""{search}"") or contains(Address,""{search}"") or contains(Email,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "User,User1", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                customers = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Customers" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            customer = new Certify.Server.Models.CertifyApp.Customer();
        }

        protected async Task EditRow(Certify.Server.Models.CertifyApp.Customer args)
        {
            isEdit = true;
            customer = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Certify.Server.Models.CertifyApp.Customer customer)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CertifyAppService.DeleteCustomer(id:customer.Id);

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
                    Detail = $"Unable to delete Customer"
                });
            }
        }
        protected bool errorVisible;
        protected Certify.Server.Models.CertifyApp.Customer customer;

        protected IEnumerable<Certify.Server.Models.CertifyApp.User> usersForCreatedBy;

        protected IEnumerable<Certify.Server.Models.CertifyApp.User> usersForModifiedBy;


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
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load User" });
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
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load User1" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CertifyAppService.UpdateCustomer(id:customer.Id, customer) : await CertifyAppService.CreateCustomer(customer);

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