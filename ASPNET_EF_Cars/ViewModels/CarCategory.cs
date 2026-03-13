using ASPNET_EF_Cars.Models;

namespace ASPNET_EF_Cars.ViewModels
{
    public class CarCategory
    {
        public Car car { get; set; } // навигационное св-во
        public IEnumerable<Category> categories { get; set; }
    }
}
