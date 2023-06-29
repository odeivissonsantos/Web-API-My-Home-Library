using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_My_Home_Library.DTOs.Critica;

namespace WebAPI_My_Home_Library.DTOs.Livro
{
    public class NovoLivroRetornoDTO : CriticaDTO
    {
        public long Ide_Livro { get; set; }

        public NovoLivroRetornoDTO()
        { 
        
        }

        public NovoLivroRetornoDTO(Models.Livro item)
        {
            Ide_Livro = item.Ide_Livro;
            IsOk = true;
            MensagemRetorno = "Livro cadastrado com sucesso";
        }
    }


}
