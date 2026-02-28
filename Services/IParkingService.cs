using System;
using ParkingSystem.Models;

namespace ParkingSystem.Services;

public interface IParkingService
{
    void CreateLot(int capacity);
    int? Park(string registrationNumber, string color, VehicleType type);
    bool Leave(int slotNumber);
    void PrintStatus();
    int CountByType(VehicleType type);
    IEnumerable<string> GetRegistrationsByOddPlate();
    IEnumerable<string> GetRegistrationsByEvenPlate();
    IEnumerable<string> GetRegistrationsByColor(string color);
    IEnumerable<int> GetSlotsByColor(string color);
    int? GetSlotByRegistration(string registrationNumber);

}
