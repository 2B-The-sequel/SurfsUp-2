using Microsoft.AspNetCore.Components;

namespace SurfsUpBlazor.Client.Pages
{
    public class DisplayWeatherBase : ComponentBase
    {
        [Parameter]
        public int DaysShown { get; set; } = 1;




    }
}
