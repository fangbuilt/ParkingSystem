using Microsoft.Extensions.DependencyInjection;
using ParkingSystem.Models;
using ParkingSystem.Services;

var services = new ServiceCollection();
services.AddSingleton<IParkingService, ParkingService>();
var serviceProvider = services.BuildServiceProvider();
var parkingService = serviceProvider.GetRequiredService<IParkingService>();

Console.WriteLine("Parking System Ready. Type your command:");

while (true)
{
    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(input)) continue;
    var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLower();

    switch (command)
    {
        case "create_parking_lot":
            if (parts.Length < 2 || !int.TryParse(parts[1], out int capacity))
            {
                Console.WriteLine("Usage: create_parking_lot <number>");
                break;
            }
            parkingService.CreateLot(capacity);
            break;
        case "park":
            if (parts.Length < 4)
            {
                Console.WriteLine("Usage: park <registration> <color> <type>");
                break;
            }
            if (!Enum.TryParse<VehicleType>(parts[3], true, out var vehicleType))
            {
                Console.WriteLine("Invalid vehicle type. Use: Mobil or Motor");
            }
            var slot = parkingService.Park(parts[1], parts[2], vehicleType);
            Console.WriteLine(slot.HasValue ? $"Allocated slot number: {slot}" : "Sorry, parking lot is full");
            break;
        case "leave":
            if (parts.Length < 2 || !int.TryParse(parts[1], out int leaveSlot))
            {
                Console.WriteLine("Usage: leave <slot_number>");
                break;
            }
            var left = parkingService.Leave(leaveSlot);
            Console.WriteLine(left ? $"Slot number {leaveSlot} is free" : $"Slot number {leaveSlot} not found");
            break;
        case "status":
            parkingService.PrintStatus();
            break;
        case "type_of_vehicles":
            if (parts.Length < 2 || !Enum.TryParse<VehicleType>(parts[1], true, out var typeQuery))
            {
                Console.WriteLine("Usage: type_of_vehicles <Mobil|Motor>");
                break;
            }
            Console.WriteLine(parkingService.CountByType(typeQuery));
            break;
        case "registration_numbers_for_vehicles_with_odd_plate":
            var oddPlates = parkingService.GetRegistrationsByOddPlate();
            Console.WriteLine(string.Join(", ", oddPlates));
            break;

        case "registration_numbers_for_vehicles_with_even_plate":
            Console.WriteLine(string.Join(", ", parkingService.GetRegistrationsByEvenPlate()));
            break;

        case "registration_numbers_for_vehicles_with_color":
            if (parts.Length < 2) { Console.WriteLine("Usage: ...with_color <color>"); break; }
            Console.WriteLine(string.Join(", ", parkingService.GetRegistrationsByColor(parts[1])));
            break;

        case "slot_numbers_for_vehicles_with_color":
            if (parts.Length < 2) { Console.WriteLine("Usage: ...with_color <color>"); break; }
            Console.WriteLine(string.Join(", ", parkingService.GetSlotsByColor(parts[1])));
            break;

        case "slot_number_for_registration_number":
            if (parts.Length < 2) { Console.WriteLine("Usage: slot_number_for_registration_number <plate>"); break; }
            var foundSlot = parkingService.GetSlotByRegistration(parts[1]);
            Console.WriteLine(foundSlot.HasValue ? foundSlot.ToString() : "Not found");
            break;
        case "exit":
            Console.WriteLine("Goodbye");
            return;
        default:
            Console.WriteLine($"Unknown command: {command}");
            break;
    }
}
