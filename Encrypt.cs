using System.Security.Cryptography;

namespace WebApi_Proyecto_Final.Encrypt
{
    public static class Encrypt
    {
        private const int saltSize = 32; // tamaño del salto
        private const int iterations = 10000; //cant iteraciones

        public static string EncryptPassword(string password)
        {
            // generar salto aleatorio
            byte[] salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            // crea una instancia de la clase Rfc2898DeriveBytes para generar la clave derivada
            var encrypt = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256); //genera clave a partir de contraseña y salt
            // generar hash HMACSHA256
            byte[] hash = encrypt.GetBytes(32);
            //combinar salto y hash en una matriz
            byte[] hashBytes = new byte[saltSize + 32]; //Array.Copy es una función que copia una sección de una matriz a otra matriz.
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, 32);
            // convierto matriz de bytes en string
            string passwordHashed = Convert.ToBase64String(hashBytes);
            return passwordHashed; //retorno la pass encriptada. siempre da diferente script por el salt aleatorio
        }

        public static bool VerifyPassword(string password, string hashedPassword) //verificar si la contraseña coincide con el hash
        {
            // Convierte la cadena a una matriz de bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            // Extrae el salt de la matriz de bytes
            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            // Calcula el hash HMACSHA256 utilizando la contraseña proporcionada y el salt extraído
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // Tamaño del hash en bytes
            // Compara los hashes para verificazr si es lacontraseña o no
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + saltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
