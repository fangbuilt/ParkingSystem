using System;
using ParkingSystem.Models;

namespace ParkingSystem.Services;

public class ParkingService : IParkingService
{
    private List<ParkingSlot> _slots = new();
    public void CreateLot(int capacity)
    {
        _slots = Enumerable.Range(1, capacity)
                            .Select(i => new ParkingSlot(i))
                            .ToList();

        Console.WriteLine($"Created a parking lot with {capacity} slots");
    }
    public int? Park(string registrationNumber, string color, VehicleType type)
    {
        var availableSlot = _slots.FirstOrDefault(s => s.IsAvailable);

        if (availableSlot == null)
            return null;

        availableSlot.CurrentVehicle = new Vehicle(registrationNumber, color, type);
        return availableSlot.SlotNumber;
    }
    public bool Leave(int slotNumber)
    {
        var slot = _slots.FirstOrDefault(s => s.SlotNumber == slotNumber);
        if (slot == null)
            return false;

        slot.CurrentVehicle = null;
        return true;
    }
    public void PrintStatus()
    {
        Console.WriteLine($"{"Slot No.",-10} {"Type",-10} {"Registration No.",-20} Color");

        foreach (var slot in _slots.Where(s => !s.IsAvailable).OrderBy(s => s.SlotNumber))
        {
            var v = slot.CurrentVehicle;

            Console.WriteLine($"{slot.SlotNumber,-10} {v.Type,-10} {v.RegistrationNumber,-20} {v.Color}");
        }
    }
    public int CountByType(VehicleType type)
    {
        return _slots.Count(s => !s.IsAvailable && s.CurrentVehicle!.Type == type);
    }
    public IEnumerable<string> GetRegistrationsByOddPlate()
    {
        return _slots
            .Where(s => !s.IsAvailable)
            .Select(s => s.CurrentVehicle!.RegistrationNumber)
            .Where(reg => IsOddPlate(reg));
    }
    public IEnumerable<string> GetRegistrationsByEvenPlate()
    {
        return _slots
            .Where(s => !s.IsAvailable)
            .Select(s => s.CurrentVehicle!.RegistrationNumber)
            .Where(reg => !IsOddPlate(reg));
    }
    public IEnumerable<string> GetRegistrationsByColor(string color)
    {
        return _slots
            .Where(s => !s.IsAvailable)
            .Where(s => s.CurrentVehicle!.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.CurrentVehicle!.RegistrationNumber);
    }
    public IEnumerable<int> GetSlotsByColor(string color)
    {
        return _slots
            .Where(s => !s.IsAvailable)
            .Where(s => s.CurrentVehicle!.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.SlotNumber);
    }
    public int? GetSlotByRegistration(string registrationNumber)
    {
        return _slots
            .Where(s => !s.IsAvailable)
            .FirstOrDefault(s => s.CurrentVehicle!.RegistrationNumber == registrationNumber)?.SlotNumber;
    }
    private static bool IsOddPlate(string registrationNumber)
    {
        var parts = registrationNumber.Split('-');
        if (parts.Length < 2) return false;

        if (int.TryParse(parts[1], out int number))
            return number % 2 != 0;

        return false;
    }
}
