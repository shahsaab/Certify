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
    public partial class Menus
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

        protected IEnumerable<Certify.Server.Models.CertifyApp.Menu> menus;

        protected RadzenDataGrid<Certify.Server.Models.CertifyApp.Menu> grid0;
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
                var result = await CertifyAppService.GetMenus(filter: $@"(contains(Title,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                menus = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Menus" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            isEdit = false;
            menu = new Certify.Server.Models.CertifyApp.Menu();
        }

        protected async Task EditRow(Certify.Server.Models.CertifyApp.Menu args)
        {
            isEdit = true;
            menu = args;
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, Certify.Server.Models.CertifyApp.Menu menu)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await CertifyAppService.DeleteMenu(id:menu.Id);

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
                    Detail = $"Unable to delete Menu"
                });
            }
        }
        protected bool errorVisible;
        protected Certify.Server.Models.CertifyApp.Menu menu;

        protected async Task FormSubmit()
        {
            try
            {
                dynamic result = isEdit ? await CertifyAppService.UpdateMenu(id:menu.Id, menu) : await CertifyAppService.CreateMenu(menu);

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