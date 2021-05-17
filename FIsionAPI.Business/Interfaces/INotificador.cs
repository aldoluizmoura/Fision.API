using FIsionAPI.Business.Notificacões;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
