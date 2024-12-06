using Jornada.Worker.Interfaces;
using Jornada.Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jornada.Worker.Servicos
{
    public class RetornoServico : IRetornoServico
    {
        public RetornoServico()
        {
                
        }
        public async Task<bool> DisparRetorno(Notificacao notificacao)
        {
            return true;
        }
    }
}
