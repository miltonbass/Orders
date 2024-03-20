﻿using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountryForm
    {
        private EditContext ediContext = null!;

        protected override void OnInitialized()
        {
            ediContext = new(Country);
        }

        [EditorRequired, Parametrer] public Country Country { get; set; } = null!;
        [EditorRequired, Parametrer] public EventCallback OnValidSubmit { get; set; }
        [EditorRequired, Parametrer] public EventCallback ReturnAccion { get; set; }
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        public bool FormPostedSuccessfully { get; set; }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = ediContext.IsModified();
            if (!formWasEdited || FormPostedSuccessfully)
            {
                return;
            }

            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "¿Deseas abandonar la página y perder los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });
            var confirm = !string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}