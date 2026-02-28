using System;

namespace ParkingSystem.Models;

public class ParkingSlot
{
    public int SlotNumber { get; init; }
    public Vehicle? CurrentVehicle { get; set; }
    public bool IsAvailable => CurrentVehicle == null;
    public ParkingSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
        CurrentVehicle = null; // Slot baru selalu kosong
    }
}
