using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages
    {
        public static string Added = "Başarıyla eklendi";
        public static string Deleted = "BAşarıyla silindi";
        public static string NameInvalid = "Geçersiz isim !";
        public static string MaintenanceTime = "Sistem bakımda !";
        public static string Listed = "Başarıyla listelendi !";
        public static string Modified = "Başarıyla güncellendi !";
        public static string AlreadyExist = "Sistemde mevcut !";

        public static string NotFound = "Böyle bir bilgi yok !";
        public static string OutOfLimit = "Ekleme limiti aşıldı !";




        public static string MailAlreadyExist = " Mail sistemde mevcut !";
        public static string AlreadyRented = "Araba zaten kiralanmış. Önce Teslim ediniz !";

        public static string UserNotFound = "Kullanıcı bulunamadı";

        public static string PasswordError = "Şifre hatalı";

        public static string SuccessfulLogin = "Giriş başarılı";

        public static string UserAlreadyExist = "Kullanıcı mevcut !";

        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi !";

        public static string AccesTokenCreated = "Token başarıyla oluşturuldu !";
    }
}
