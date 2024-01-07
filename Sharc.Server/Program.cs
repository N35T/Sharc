using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sharc.Core.Models.Entities;
using Sharc.Server.Core.Repositories;
using Sharc.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("api/calendar", async (IEventRepository eventRepo) => {
    // TODO: auth and current user
    var res = await GetTestEvents();//await eventRepo.GetUserEventsAsync(default);
    return res;
});


app.Run();

static Task<List<Event>> GetTestEvents() {
    var rng = new Random();
    var events = new List<Event>();
    for(int i = 0; i < 125; ++i) {
        events.Add(new Event {
            Summary = GetRandomSummary(rng),
            Created = DateTime.Now,
            StartTime = GetRandomDateTimeBetween(rng, DateTime.Now.Date.Subtract(TimeSpan.FromDays(30 * 3)), DateTime.Now.Date.Add(TimeSpan.FromDays(30 * 3))),
            Duration = TimeSpan.FromHours(1),
            Id = Guid.NewGuid(),
            IsAllDay = rng.NextDouble() < 0.1,
        });
    }
    return Task.FromResult(events);
}

static string GetRandomSummary(Random rng) {
    string[] summaries = ["Geburtstag", "Klausur", "Vorlesung", "Date", "Event", "Kino", "Foo", "Dana stinkt"];
    return summaries[rng.Next(summaries.Length)];
}

static DateTime GetRandomDateTimeBetween(Random rng,DateTime start, DateTime end) {
    var rand = rng.NextDouble();
    var date = start + (end - start) * rand;
    if (date.Hour > 17) date = date.Subtract(TimeSpan.FromHours(6));
    if (date.Hour < 8) date = date.Add(TimeSpan.FromHours(8));
    return date;
}