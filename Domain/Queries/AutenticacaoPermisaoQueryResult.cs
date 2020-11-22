using System;

namespace Domain.Queries
{
    public class AutenticacaoPermisaoQueryResult
    {
        public Guid IdSistema { get; set; }
        public Guid IdSistemaPerfil { get; set; }
        public Guid IdSistemaAcao { get; set; }
        public string NomeAcao { get; set; }
        public string ReferenciaAcao { get; set; }
    }
}
