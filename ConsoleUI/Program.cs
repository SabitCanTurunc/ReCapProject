// See https://aka.ms/new-console-template for more information
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;

CarManager carManager = new CarManager(new EfCarDal());
BrandManager brandManager = new BrandManager(new EfBrandDal());
ColorManager colorManager = new ColorManager(new EfColorDal());



CarTest();
static void CarTest()
{
    CarManager carManager = new CarManager(new EfCarDal());
    var result = carManager.GetCarDetails();

    if (result.IsSuccess == true)
    {
        foreach (var cars in result.Data)
        {
            Console.WriteLine(cars.BrandName + "/" + cars.Description);
        }

    }
    else
    {
        Console.WriteLine(result.Message);
    }


}





Brand brand = new Brand { Id = 5, Name = "Hyundai" };




Color color = new Color { Name = "Green" };


