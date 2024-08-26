using Business.Concrete;
using Core.Utilities.Security.Hashing;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;

class Program
{
    static void Main(string[] args)
    {
        // UserManager örneği oluşturun
        UserManager userManager = new UserManager(new EfUserDal());

        // Test şifresi
        string testPassword = "123456";

        // Şifre Hash ve Salt oluştur
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(testPassword, out passwordHash, out passwordSalt);

        // Yeni oluşturduğunuz hash ve salt ile doğrulama yapın
        bool isCorrectPassword = HashingHelper.VerifyPasswordHash(testPassword, passwordHash, passwordSalt);
        Console.WriteLine("Doğru şifre için doğrulama: " + isCorrectPassword); // True olmalı

        // Yanlış şifre ile test
        isCorrectPassword = HashingHelper.VerifyPasswordHash("wrongpassword", passwordHash, passwordSalt);
        Console.WriteLine("Yanlış şifre için doğrulama: " + isCorrectPassword); // False olmalı

        // Veritabanındaki kullanıcıya göre şifre kontrolü
        var user1 = userManager.GetById(11).Data; // ID 11 olan kullanıcıyı getir
        if (user1 != null)
        {
            bool isDbPasswordCorrect = HashingHelper.VerifyPasswordHash(testPassword, user1.PasswordHash, user1.PasswordSalt);
            Console.WriteLine("Veritabanı şifresi doğru mu: " + isDbPasswordCorrect);
        }
        else
        {
            Console.WriteLine("Kullanıcı bulunamadı.");
        }


        string testEmail = "deneme1@deneme";  // Var olan bir kullanıcı emaili girilmeli

        var result = userManager.GetByEmail(testEmail);

        if (result.IsSuccess)
        {
            Console.WriteLine($"User Found: {result.Data.FirstName} {result.Data.LastName}");
        }
        else
        {
            Console.WriteLine(result.Message); // Eğer kullanıcı bulunamazsa hata mesajını gösterir
        }
    }
}
