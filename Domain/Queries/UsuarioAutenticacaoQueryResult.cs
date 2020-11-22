using System;

namespace Domain.Queries
{
    public class UsuarioAutenticacaoQueryResult
    {
        public string Login { get; set; }
        public string Area { get; set; }
        public string Nome { get; set; }
        public string Origem { get; set; }
        public string CodigoCargo { get; set; }
        public string Cargo { get; set; }
        public Guid IdUnidade { get; set; }
        public Guid? Token { get; set; }
        public byte[] Foto { get; set; }
    }
}
