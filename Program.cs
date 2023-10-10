using TicketBookingService.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register your services here
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITravelerService, TravelerService>();
builder.Services.AddScoped<ITravelagentService, TravelagentService>();
builder.Services.AddScoped<IBackofficeService, BackofficeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
