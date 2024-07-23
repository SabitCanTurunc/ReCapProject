// See https://aka.ms/new-console-template for more information
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;

CarManager carManager = new CarManager(new EfCarDal());
BrandManager brandManager = new BrandManager(new EfBrandDal());
ColorManager colorManager = new ColorManager(new EfColorDal());



//Car araba = new Car {BrandId = 3, ColorId = 2, DailyPrice = 680000, ModelYear = 2014, Description = "Fiesta" };

////carManager.Add(araba);


//foreach (var car in carManager.GetAll())
//{

//    Console.WriteLine(car.Description);

//}

//carManager.Update(araba);
//foreach (var car in carManager.GetAllByBrandId(2))
//{

//    Console.WriteLine(car.Description);

//}

//foreach (var car in carManager.GetByUnitPrice(12000, 878000))
//{
//    Console.WriteLine(brandManager.GetById(car.BrandId).Name);
//}
foreach (var car in carManager.GetCarsByBrandId(2))
{

    var BrandName = brandManager.GetById(car.BrandId).Name;
    var CarDescription = car.Description;
    Console.WriteLine($"{BrandName}: {CarDescription}" );
}






Brand brand = new Brand { Id = 5, Name = "Hyundai" };

//Brand brand =brandManager.GetById(1);
//Console.WriteLine(brand.Name);

//foreach (var item in brandManager.GetAll())
//{
//    Console.WriteLine(item.Name);
//}
//brandManager.Delete(brand.Id);
//brandManager.Add(brand);
//Console.WriteLine(brandManager.GetById(1).Name);
//brandManager.Update(brand);



Color color = new Color { Name = "Green" };


//foreach (var color in colorManager.GetAll())
//{
//    Console.WriteLine(color.Name);
//}

//Console.WriteLine( colorManager.GetById(2).Name);
//colorManager.Add(color);
//ColorManager.Update(color);
//colorManager.Delete(5);
