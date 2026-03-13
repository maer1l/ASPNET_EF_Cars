using ASPNET_EF_Cars.Models;

namespace ASPNET_EF_Cars.ViewModels
{
    public class CarViewModel
    {
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Car> cars { get; set; }
    }
}
