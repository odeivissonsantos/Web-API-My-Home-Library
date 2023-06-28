using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.Context;

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
    }
}
