using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;


namespace DatSanPickleballBE.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password, byte[] salt = null)
        {
            if (salt == null)
            {
                // Tạo salt ngẫu nhiên 16 byte
                salt = RandomNumberGenerator.GetBytes(16);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // số luồng CPU
                Iterations = 4,          // số vòng lặp
                MemorySize = 1024 * 64   // 64 MB RAM
            };

            // Tạo hash 32 byte
            byte[] hashBytes = argon2.GetBytes(32);

            // Ghép salt và hash thành 1 chuỗi (Base64)
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashBytes)}";
        }

        public static bool Verify(string password, string hashed)
        {
            var parts = hashed.Split('.');
            if (parts.Length != 2) return false;

            // Lấy salt từ DB
            byte[] salt = Convert.FromBase64String(parts[0]);

            // Hash lại password người dùng nhập
            string newHash = HashPassword(password, salt);

            // So sánh với hash đã lưu
            return newHash == hashed;
        }
    }
}
