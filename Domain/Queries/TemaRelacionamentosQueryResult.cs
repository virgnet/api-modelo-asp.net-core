using System;

namespace Domain.Queries
{
    public class TemaRelacionamentosQueryResult
    {
        public Guid IdTemaRelacionamento { get; set; }
        public Guid IdTema { get; set; }
        public string Tema { get; set; }
        public Guid? IdArea { get; set; }
        public string Area { get; set; }
        public Guid? IdProjeto { get; set; }
        public string Projeto { get; set; }
        public Guid? IdSistema { get; set; }
        public string Sistema { get; set; }
        public Guid? IdDocumento { get; set; }
        public string Documento { get; set; }
    }
}
