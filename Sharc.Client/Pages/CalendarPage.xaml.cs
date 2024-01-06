using Sharc.Client.Services;
using Sharc.Core.Models.Entities;
using Syncfusion.Maui.Scheduler;

namespace Sharc.Client.Pages;

public partial class CalendarPage : ContentPage {

	private readonly EventService _eventService;

	public List<Event> MappedEvents { get; private set; } = new List<Event>();

	public CalendarPage(EventService eventService) {
		_eventService = eventService;
		InitializeComponent();
	}

	public async void QueryAppointmentHandler(object sender, SchedulerQueryAppointmentsEventArgs e) {
        this.Scheduler.ShowBusyIndicator = true;
		try {
			MappedEvents = await _eventService.GetEventsBetweenAsync(e.VisibleDates.First(), e.VisibleDates.Last());
		}catch(Exception) {
			await DisplayAlert("Error", "Could not load calendar source! Are you in your local network?", "Ok");
		}
        this.Scheduler.ShowBusyIndicator = false;
    }
}