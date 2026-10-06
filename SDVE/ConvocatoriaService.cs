using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SDVE
{
    internal class ConvocatoriaService
    {
        private List<Convocatoria> convocatorias;

        public ConvocatoriaService()
        {
            convocatorias = new List<Convocatoria>();
            CargarConvocatorias();
        }

        // Carga las convocatorias y sus candidatos
        private void CargarConvocatorias()
        {
            // ==========================================
            // 1. SOCIEDAD DE ALUMNOS
            // ==========================================

            Convocatoria sociedadAlumnos = new Convocatoria
            {
                Id = 1,
                Nombre = "Sociedad de Alumnos",
                Activa = true
            };

            sociedadAlumnos.Candidatos.Add(new Candidato
            {
                Id = 1,
                Nombre = "Ana López",
                Registrado = true,
                ConvocatoriaId = 1
            });

            sociedadAlumnos.Candidatos.Add(new Candidato
            {
                Id = 2,
                Nombre = "Luis Pérez",
                Registrado = true,
                ConvocatoriaId = 1
            });


            // ==========================================
            // 2. CONSEJO UNIVERSITARIO
            // ==========================================

            Convocatoria consejoUniversitario = new Convocatoria
            {
                Id = 2,
                Nombre = "Consejo Universitario",
                Activa = true
            };

            consejoUniversitario.Candidatos.Add(new Candidato
            {
                Id = 3,
                Nombre = "María García",
                Registrado = true,
                ConvocatoriaId = 2
            });

            consejoUniversitario.Candidatos.Add(new Candidato
            {
                Id = 4,
                Nombre = "Carlos Ruíz",
                Registrado = true,
                ConvocatoriaId = 2
            });


            // ==========================================
            // 3. CONSEJO DE REPRESENTANTES
            // ==========================================

            Convocatoria consejoRepresentantes = new Convocatoria
            {
                Id = 3,
                Nombre = "Consejo de Representantes",
                Activa = true
            };

            consejoRepresentantes.Candidatos.Add(new Candidato
            {
                Id = 5,
                Nombre = "Sofía Torres",
                Registrado = true,
                ConvocatoriaId = 3
            });

            consejoRepresentantes.Candidatos.Add(new Candidato
            {
                Id = 6,
                Nombre = "Juan Martínez",
                Registrado = true,
                ConvocatoriaId = 3
            });


            // Agregar las convocatorias a la lista
            convocatorias.Add(sociedadAlumnos);
            convocatorias.Add(consejoUniversitario);
            convocatorias.Add(consejoRepresentantes);
        }


        // ==========================================
        // OBTENER TODAS LAS CONVOCATORIAS
        // ==========================================

        public List<Convocatoria> ObtenerConvocatorias()
        {
            return convocatorias;
        }


        // ==========================================
        // OBTENER SOLO LAS CONVOCATORIAS ACTIVAS
        // ==========================================

        public List<Convocatoria> ObtenerConvocatoriasActivas()
        {
            return convocatorias
                .Where(c => c.Activa).ToList();
        }


        // ==========================================
        // BUSCAR UNA CONVOCATORIA POR ID
        // ==========================================

        public Convocatoria ObtenerConvocatoriaPorId(int id)
        {
            return convocatorias.FirstOrDefault(c => c.Id == id);
        }


        // ==========================================
        // OBTENER CANDIDATOS DE UNA CONVOCATORIA
        // ==========================================

        public List<Candidato> ObtenerCandidatos(int convocatoriaId)
        {
            Convocatoria convocatoria = ObtenerConvocatoriaPorId(convocatoriaId);

            if (convocatoria == null)
            {
                return new List<Candidato>();
            }

            return convocatoria.Candidatos;
        }


        // ==========================================
        // CAMBIAR ESTADO DE UNA CONVOCATORIA
        // ==========================================

        public void CambiarEstadoConvocatoria(int id, bool activa)
        {
            Convocatoria convocatoria = ObtenerConvocatoriaPorId(id);

            if (convocatoria != null)
            {
                convocatoria.Activa = activa;
            }
        }


        // ==========================================
        // AGREGAR CANDIDATO NO REGISTRADO
        // ==========================================

        public void AgregarCandidatoNoRegistrado(
            int convocatoriaId,
            string nombre)
        {
            Convocatoria convocatoria =
                ObtenerConvocatoriaPorId(convocatoriaId);

            if (convocatoria != null &&
                !string.IsNullOrWhiteSpace(nombre))
            {
                int nuevoId = ObtenerSiguienteIdCandidato();

                convocatoria.Candidatos.Add(new Candidato
                {
                    Id = nuevoId,
                    Nombre = nombre,
                    Registrado = false,
                    ConvocatoriaId = convocatoriaId
                });
            }
        }


        // ==========================================
        // OBTENER EL SIGUIENTE ID DE CANDIDATO
        // ==========================================

        private int ObtenerSiguienteIdCandidato()
        {
            if (!convocatorias.Any())
            {
                return 1;
            }

            int ultimoId = convocatorias
                .SelectMany(c => c.Candidatos)
                .Select(c => c.Id)
                .DefaultIfEmpty(0)
                .Max();

            return ultimoId + 1;
        }
    }
}