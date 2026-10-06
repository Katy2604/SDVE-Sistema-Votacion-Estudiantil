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

                // Aplicar la fuente y el color aquí:
                radio.Font = new Font("Segoe UI", 13.8f, FontStyle.Bold);
                radio.ForeColor = SystemColors.Control;

                grpCandidatos.Controls.Add(radio);
                posicionY += 35;
            }
        }

        private void btnVotar_Click(object sender, EventArgs e)
        {
            //Verificar que haya una convocatoria seleccionada
            if (cmbConvocatoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione una convocatoria.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int? idCandidatoSeleccionado = null;
            string nombreCandidatoSeleccionado = string.Empty;

            // Buscar cuál RadioButton está marcado dentro del GroupBox
            foreach (Control control in grpCandidatos.Controls)
            {
                if (control is RadioButton radio && radio.Checked)
                {
                    idCandidatoSeleccionado = (int)radio.Tag;
                    break;
                }
            }

            //Validar que se haya seleccionado un candidato
            if (idCandidatoSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un candidato para votar.", "Atención",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Convocatoria convocatoria = (Convocatoria)cmbConvocatoria.SelectedItem;
                //Registrar el voto 
                // Como el servicio actual no guarda votos en base de datos ni listas, 
                // puedes simular el registro con un mensaje confirmando la elección:
                MessageBox.Show($"¡Voto registrado con éxito!\n\nConvocatoria: {convocatoria.Nombre}\nCandidato: " +
                    $"{nombreCandidatoSeleccionado}",
                        "Confirmación de Voto",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                // Limpiar formulario tras emitir el voto
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el voto: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Método auxiliar de limpieza
        private void LimpiarFormulario()
        {
            if (cmbConvocatoria.Items.Count > 0)
            {
                cmbConvocatoria.SelectedIndex = -1; // Quita la selección del ComboBox
            }
            grpCandidatos.Controls.Clear(); // Borra los RadioButtons generados
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}