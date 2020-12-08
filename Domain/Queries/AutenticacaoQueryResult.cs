﻿using System;
using System.Collections.Generic;

namespace Domain.Queries
{
    public class AutenticacaoQueryResult
    {
        public Guid Token { get; set; }
        public DateTime DataUltimoLogon { get; set; }
        public Guid IdFuncionario { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string CPF { get; set; }
        public Guid IdUnidade { get; set; }
        public string Unidade { get; set; }
        public Guid IdCoordenacao { get; set; }
        public string Coordenacao { get; set; }
        public string Perfil { get; set; }
        public int IdUnidadeSISTF { get; set; }

        public IEnumerable<PermissaoQueryResult> Permissoes { get; set; }
    }

    public class PermissaoQueryResult
    {
        public string Nome { get; set; }
        public string Referencia { get; set; }
    }
}
