using System;

namespace Domain.Queries
{
    public class PublicacaoQueryResult
    {
        public Guid IdPublicacao { get; set; }
        public string Identificador { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataPublicacao { get; set; }
    }
}
