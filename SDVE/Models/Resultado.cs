using System;
using System.Collections.Generic;
using System.Text;

namespace SDVE.Models
{
    internal class Resultado
    {
        public string Convocatoria { get; set; } = "";

        public string Candidato { get; set; } = "";

        public int Votos { get; set; }

        public double Porcentaje { get; set; }

        public bool EsWriteIn { get; set; }
    }
}
