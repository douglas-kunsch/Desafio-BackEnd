
using System;

namespace MotoRent.Domain.Events;

public sealed record MotorcycleRegisteredEvent(Guid MotorcycleId, int Year, string Model, string LicensePlate, string Identifier);
