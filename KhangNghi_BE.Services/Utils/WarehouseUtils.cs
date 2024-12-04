using KhangNghi_BE.Data.Models;

namespace KhangNghi_BE.Services.Utils
{
    public static class WarehouseUtils
    {
        public static IQueryable<Warehouse> SelectWarehouse(this IQueryable<Warehouse> warehouses)
        {
            return warehouses
                .Select(w => new Warehouse
                {
                    WarehouseId = w.WarehouseId,
                    WarehouseName = w.WarehouseName,
                    Address = new Address
                    {
                        AddressId = w.Address.AddressId,
                        Street = w.Address.Street,
                        Ward = w.Address.Ward,
                        District = w.Address.District,
                        City = w.Address.City,
                    }
                });
        }
    }
}
