using BrfSvalanApi;
using Iot.Device.CharacterLcd;
using Iot.Device.Mcp23xxx;
using System.Device.Gpio;
using System.Device.I2c;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

using I2cDevice i2cDevice = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
using Mcp23008 serialDriver = new Mcp23008(i2cDevice);
using var lcd = new Lcd1602(dataPins: new int[] { 0, 1, 2, 3 },
                        registerSelectPin: 4,
                        readWritePin: 5,
                        enablePin: 6,
                        controller: new GpioController(PinNumberingScheme.Logical, serialDriver));
lcd.Clear();
lcd.BacklightOn = true;
lcd.SetCursorPosition(0, 0);
lcd.Write($"Hi from wellsb.com");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}