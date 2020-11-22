using System;
using System.Collections.Generic;

namespace Domain.Queries
{
    public class TemaQueryResult
    {
        public Guid IdTema { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Byte[] Imagem { get; set; }
        public string Tags { get; set; }

        public IEnumerable<TemaRelacionamentosQueryResult> Relacionamentos { get; set; }
        public IEnumerable<DocumentoQueryResult> Documentos { get; set; }
        public IEnumerable<CalendarioQueryResult> Calendarios { get; set; }
        public IEnumerable<SistemaQueryResult> Sistemas { get; set; }
        public IEnumerable<ProjetoQueryResult> Projetos { get; set; }
        public IEnumerable<AreaQueryResult> Areas { get; set; }
        public IEnumerable<ComunicadoQueryResult> Comunicados { get; set; }
    }
}
