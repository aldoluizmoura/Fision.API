using FIsionAPI.Business.Notificacões;
using System.Collections.Generic;

namespace FIsionAPI.Business.Interfaces;

public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}
