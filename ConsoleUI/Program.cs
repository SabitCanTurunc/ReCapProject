// See https://aka.ms/new-console-template for more information
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;

CarManager carManager = new CarManager(new EfCarDal());
BrandManager brandManager = new BrandManager(new EfBrandDal());
ColorManager colorManager = new ColorManager(new EfColorDal());


RentalTest();
static void UserTest() {
    UserManager userManager = new UserManager(new EfUserDal());
    User user1 = new User { FirstName = "can", LastName = "turunc", Email = "deneme1@deneme", Password = "124456" };
    User user2 = new User { FirstName = "ömer", LastName = "aydin", Email = "deneme2@deneme", Password = "124456" };
    User user3 = new User { FirstName = "berat", LastName = "durbardagı", Email = "deneme3@deneme", Password = "124456" };



    foreach (User user in userManager.GetAll().Data) {
        Console.WriteLine(user.FirstName);
    }

}

static void RentalTest()
{
    RentalManager rentalManager = new RentalManager(new EfRentalDal());
    Rental rental1 = new Rental {CarId=3,CustomerId=2, RentDate = new DateOnly(2024, 8, 15) };
    Rental return1 = new Rental {Id=6, CarId = 3, CustomerId = 2, RentDate = new DateOnly(2024, 8, 15),ReturnDate = new DateOnly(2024, 8, 16) };


    Console.WriteLine(rentalManager.GetById(return1.Id).Data.CustomerId);

    //foreach (Rental rental in rentalManager.GetAll().Data)
    //{
    //    Console.WriteLine(rental.CarId);
    //}


}

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


