using System;

namespace Domain.Queries
{
    public class PublicacaoQueryResult
    {
        public Guid IdPublicacao { get; set; }
        public Guid? IdTema { get; set; }
        public string Tema { get; set; }
        public Guid IdTipoDeConteudo { get; set; }
        public string TipoDeConteudo { get; set; }
        public string Identificador { get; set; }
        public string Titulo { get; set; }
        public string Chamada { get; set; }
        public string Conteudo { get; set; }
        public string Tags { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataPublicacao { get; set; }
    }
}
