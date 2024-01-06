using Newtonsoft.Json;
using Sharc.Core.Models.Entities;
using System.Net.Http.Json;

namespace Sharc.Client.Services {
    public class EventService {

        private readonly HttpClient _httpClient;

        private static List<Event>? _cachedEvents;

        public EventService() {
            _httpClient = new() ;
            _httpClient.BaseAddress = new Uri(Config.SharcServerUrl + "/api/");
        }

        private async Task<bool> TryCacheCalendarAsync() {
            var path = Path.Combine(FileSystem.Current.AppDataDirectory, "calendar.json");
            var res = await _httpClient.GetFromJsonAsync<List<Event>>("calendar/").ConfigureAwait(false);
            if(res is null) {
                if (!File.Exists(path))
                    return false;
                var cache = await File.ReadAllTextAsync(path).ConfigureAwait(false);
                _cachedEvents = JsonConvert.DeserializeObject<List<Event>>(cache);
                return true;
            }

            _cachedEvents = res;
            File.WriteAllText(path, JsonConvert.SerializeObject(res));
            return true;
        }

        public async Task<List<Event>> GetEventsBetweenAsync(DateTime start, DateTime end) {
            if(_cachedEvents is null) {
                var res = await TryCacheCalendarAsync().ConfigureAwait(false);
                if (!res)
                    throw new System.Exception("Could not load calendar");
            }
            return _cachedEvents!
                .Where(e => IsRecurringInRange(e,start) || IsEventInRange(e,start,end))
                .ToList();
        }

        private bool IsRecurringInRange(Event e, DateTime start) => e.RecurrenceRule is not null && e.RecurrenceRule.Until > start;
        private bool IsEventInRange(Event e, DateTime start, DateTime end) => e.StartTime <= end && e.EndTime >= start;
    }
}
