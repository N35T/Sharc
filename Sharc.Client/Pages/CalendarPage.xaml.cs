using Sharc.Client.Services;
using Sharc.Core.Models.Entities;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace Sharc.Client.Pages;

public partial class CalendarPage : ContentPage {

	private readonly EventService _eventService;

	public ObservableCollection<Event> MappedEvents { get; private set; } = new ();

	public CalendarPage(EventService eventService) {
		BindingContext = this;
		_eventService = eventService;
		InitializeComponent();
		this.Scheduler.Loaded += LoadEvents;
	}

	private async void LoadEvents(object? sender, EventArgs e) {
		this.Scheduler.ShowBusyIndicator = true;
		try {
			var events = await _eventService.GetAllEventsAsync();
			MappedEvents = new ObservableCollection<Event>(events);
		}catch(Exception) {
			await DisplayAlert("Error", "Could not load events. Are you in your local network?", "Ok");
		}
        this.Scheduler.ShowBusyIndicator = false;
    }
}