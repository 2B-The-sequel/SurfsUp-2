using Microsoft.AspNetCore.Components;
using SurfsUpBlazor.Client.Pages.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SurfsUpBlazor.Client.Pages
{
    public class WeatherDate
    {
        [Required]
        [WeatherStartDate]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Startdato")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required]
        [WeatherEndDate]
        [DataType(DataType.Date)]
        [DisplayName("Slutdato")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(5);

        public void UpdateStartDate(WeatherDate date)
        { StartDate = date.StartDate; }

        public void UpdateEndDate(WeatherDate date)
        { EndDate = date.EndDate; }

    }
}
