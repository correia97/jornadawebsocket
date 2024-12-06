using Jornada.Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jornada.Worker.Interfaces
{
    public interface IProcessarNotificacaoServico
    {
        Task<bool> Processar(Notificacao notificacao);
    }
}
