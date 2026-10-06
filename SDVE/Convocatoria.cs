using System;
using System.Collections.Generic;
using System.Text;

namespace SDVE
{
    internal class Convocatoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activa { get; set; }
        public List<Candidato> Candidatos { get; set; }

        public Convocatoria()
        {
            Candidatos = new List<Candidato>();
        }
    }
}