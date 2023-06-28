using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.DTOs.Login;

namespace WebAPI_My_Home_Library.Utils
{
    public class Utilies
    {
        private readonly MyHomeLibraryContext _myHomeLibraryContext;
        public Utilies(MyHomeLibraryContext myHomeLibraryContext)
        {
            _myHomeLibraryContext = myHomeLibraryContext;

        }

        public bool VerificaSeExiste(int tipo, string parametro) //1 - Email; 2 - Ide
        {

            bool isExists = false;

            if (tipo == 1)
            {
                var query = _myHomeLibraryContext.Usuario.Where(x => x.Email == parametro).FirstOrDefault();
                if (query != null) isExists = true;
            }

            if (tipo == 2)
            {
                var query = _myHomeLibraryContext.Usuario.Where(x => x.Ide_Usuario == long.Parse(parametro)).FirstOrDefault();
                if (query != null) isExists = true;
            }

            return isExists;
        }

        #region Encriptação de senha SHA-512

        public static String SHA512(String input)
        {
            return SHA512(UTF8Encoding.UTF8.GetBytes(input));
        }

        public static String SHA512(Byte[] input)
        {
            using (System.Security.Cryptography.SHA512 hash =
                System.Security.Cryptography.SHA512.Create())
            {
                return BitConverter.ToString(hash.ComputeHash(input))
                    .Replace("-", "").ToLower();
            }
        }

        #endregion

        #region GERAÇÃO DE TOKEN
        public static GerarTokenDTO GerarToken(int length = 32)
        {
            GerarTokenDTO tokenDTO = new();

            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            tokenDTO.Token =  new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            tokenDTO.DataExpiracaoToken = DateTime.Now.AddHours(2); 

            return tokenDTO;
        }
        #endregion
    }
}
