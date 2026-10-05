using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace SDVE
{
    public partial class Votacion : Form
    {
        private ConvocatoriaService servicio;
        public Votacion()
        {
            InitializeComponent();
            servicio = new ConvocatoriaService();
            CargarConvocatorias();
        }

        //Crear método para cargar las convocatorias en el ComboBox
        private void CargarConvocatorias()
        {
            cmbConvocatoria.DataSource = servicio.ObtenerConvocatoriasActivas();
            cmbConvocatoria.DisplayMember = "Nombre";
            cmbConvocatoria.ValueMember = "Id";
        }

        private void cmbConvocatoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarCandidatos();
        }

        //Crear método para mostrar los candidatos
        private void MostrarCandidatos()
        {
            grpCandidatos.Controls.Clear();
            if (cmbConvocatoria.SelectedItem == null)
            {
                return;
            }

            Convocatoria convocatoria = (Convocatoria)cmbConvocatoria.SelectedItem;
            List<Candidato> candidatos = servicio.ObtenerCandidatos(convocatoria.Id);

            int posicionY = 30;
            foreach (Candidato candidato in candidatos)
            {
                RadioButton radio = new RadioButton();
                radio.Text = candidato.Nombre;
                radio.Tag = candidato.Id;
                radio.AutoSize = true;
                radio.Location = new Point(20, posicionY);
                grpCandidatos.Controls.Add(radio);
                posicionY += 35;
            }
        }
    }
}
