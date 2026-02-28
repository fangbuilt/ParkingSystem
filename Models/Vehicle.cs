using System;

namespace ParkingSystem.Models;

public record Vehicle
(
    string RegistrationNumber,
    string Color,
    VehicleType Type
);
