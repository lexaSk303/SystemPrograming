using EquipmentRegistry.Legacy;

var repository = new EquipmentRepository();
var service = new InventoryService(repository);
var failures = new List<string>();

void Check(string name, bool condition)
{
    Console.WriteLine($"{(condition ? "PASS" : "FAIL")} — {name}");
    if (!condition) failures.Add(name);
}

// 1. Проверка обычного сценария 
var normalEquipment = new Equipment
{
    Id = 1,
    Name = "Test Laptop",
    PurchasePrice = 1000m,
    PurchaseDate = new DateOnly(2022, 10, 27),
    Status = EquipmentStatus.InUse
};
var normalValue = service.CalculateBookValue(normalEquipment, new DateOnly(2026, 10, 27));
Check("балансовая стоимость нового оборудования корректна (4 года = 80% амортизации)",
    normalValue == 200m);

// 2. Проверка граничного сценария 
var oldEquipment = new Equipment
{
    Id = 999,
    Name = "Old test device",
    PurchasePrice = 100m,
    PurchaseDate = new DateOnly(2010, 1, 1),
    Status = EquipmentStatus.InUse
};
var oldValue = service.CalculateBookValue(oldEquipment, new DateOnly(2026, 10, 27));
Check("балансовая стоимость старого оборудования не отрицательна",
    oldValue >= 0m);
Check("балансовая стоимость старого оборудования строго равна нулю при полной амортизации",
    oldValue == 0m);

Console.WriteLine();
Console.WriteLine($"Итого: {failures.Count} проверок не пройдено.");
