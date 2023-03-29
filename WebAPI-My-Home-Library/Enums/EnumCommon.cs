using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_My_Home_Library.Enums
{
    public class EnumCommon
    {
        public enum SystemMessageTypeEnum
        {
            Success,
            Info,
            Error,
        };

        public enum PerfilEnum
        {
            [Description("Administrador")]
            Admin = 1,
            [Description("Padrão")]
            Padrao = 2
        }
    }
}
