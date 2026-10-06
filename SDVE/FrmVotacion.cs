namespace SDVE
{
    /// <summary>Pantalla de emision de voto: datos del alumno + una seccion por convocatoria activa.</summary>
    public class FrmVotacion : Form
    {
        private const string OpcionWriteIn = "Candidato no registrado (escribir nombre)";

        private readonly VotacionService _service;
        private readonly Dictionary<string, List<string>> _candidatos;
        private readonly Dictionary<string, (ComboBox Combo, TextBox WriteIn)> _controles = new();

        private readonly TextBox txtId = new();
        private readonly TextBox txtGrupo = new();
        private readonly TextBox txtCarrera = new();
        private readonly TextBox txtCentro = new();
        private readonly FlowLayoutPanel panelConvocatorias = new();

        /// <summary>
        /// candidatosPorConvocatoria: nombre de convocatoria -> candidatos registrados.
        /// Solo las convocatorias incluidas aqui se muestran (las "activas").
        /// Sin parametros usa datos de prueba hasta integrar el modulo de Katrina.
        /// </summary>
        public FrmVotacion(Dictionary<string, List<string>>? candidatosPorConvocatoria = null,
                           VotacionService? service = null)
        {
            _service = service ?? new VotacionService();
            _candidatos = candidatosPorConvocatoria ?? new Dictionary<string, List<string>>
            {
                ["Sociedad de Alumnos"] = new() { "Candidato A1", "Candidato A2" },
                ["Consejo Universitario"] = new() { "Candidato B1", "Candidato B2" },
                ["Consejo de Representantes"] = new() { "Candidato C1", "Candidato C2" },
            };
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Text = "Emision de voto";
            ClientSize = new Size(620, 640);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            // Color de fondo general de la ventana y color de letra por defecto
            BackColor = Color.MidnightBlue;
            ForeColor = Color.White;

            var titulo = new Label
            {
                Text = "Votacion de alumnos",
                Font = new Font("Segoe UI", 16F),
                AutoSize = true,
                Location = new Point(12, 8)
            };

            // --- Datos del alumno ---
            var grpAlumno = new GroupBox { Text = "Datos del alumno", Location = new Point(12, 50), Size = new Size(596, 170), ForeColor = Color.White };
            AgregarCampo(grpAlumno, "ID:", txtId, 28);
            AgregarCampo(grpAlumno, "Grupo:", txtGrupo, 62);
            AgregarCampo(grpAlumno, "Carrera:", txtCarrera, 96);
            AgregarCampo(grpAlumno, "Centro universitario:", txtCentro, 130);

            // --- Convocatorias activas ---
            panelConvocatorias.Location = new Point(12, 230);
            panelConvocatorias.Size = new Size(596, 340);
            panelConvocatorias.FlowDirection = FlowDirection.TopDown;
            panelConvocatorias.WrapContents = false;
            panelConvocatorias.AutoScroll = true;

            foreach (var (convocatoria, candidatos) in _candidatos)
                panelConvocatorias.Controls.Add(CrearSeccion(convocatoria, candidatos));

            var btnVotar = new Button { Text = "Emitir voto", Size = new Size(140, 36), Location = new Point(468, 585) };
            btnVotar.Click += BtnVotar_Click;
            var btnLimpiar = new Button { Text = "Limpiar", Size = new Size(110, 36), Location = new Point(348, 585) };
            btnLimpiar.Click += (_, _) => LimpiarFormulario();

            Controls.AddRange(new Control[] { titulo, grpAlumno, panelConvocatorias, btnLimpiar, btnVotar });
            AcceptButton = btnVotar;
        }

        private static void AgregarCampo(GroupBox grp, string etiqueta, TextBox txt, int y)
        {
            grp.Controls.Add(new Label { Text = etiqueta, AutoSize = true, Location = new Point(12, y + 3),
                ForeColor = Color.White});
            txt.Location = new Point(190, y);
            txt.Width = 390;
            grp.Controls.Add(txt);
        }

        private GroupBox CrearSeccion(string convocatoria, List<string> candidatos)
        {
            var grp = new GroupBox { Text = convocatoria, Size = new Size(560, 98), ForeColor = Color.White };

            var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(12, 26), Width = 530 };
            combo.Items.AddRange(candidatos.Cast<object>().ToArray());
            combo.Items.Add(OpcionWriteIn);

            var txtWriteIn = new TextBox { Location = new Point(12, 60), Width = 530, Visible = false, PlaceholderText = "Nombre del candidato no registrado" };

            combo.SelectedIndexChanged += (_, _) =>
            {
                txtWriteIn.Visible = combo.SelectedItem as string == OpcionWriteIn;
                if (!txtWriteIn.Visible) txtWriteIn.Clear();
            };

            grp.Controls.Add(combo);
            grp.Controls.Add(txtWriteIn);
            _controles[convocatoria] = (combo, txtWriteIn);
            return grp;
        }

        private void BtnVotar_Click(object? sender, EventArgs e)
        {
            // 1) Validar datos del alumno
            var alumno = new Alumno
            {
                Id = txtId.Text,
                Grupo = txtGrupo.Text,
                Carrera = txtCarrera.Text,
                CentroUniversitario = txtCentro.Text
            };
            if (!alumno.Validar(out string error)) { Aviso(error); return; }

            // 2) Validar que todas las elecciones activas esten completas
            var votos = new List<Voto>();
            foreach (var (convocatoria, (combo, txtWriteIn)) in _controles)
            {
                if (combo.SelectedIndex < 0)
                {
                    Aviso($"Selecciona una opcion en \"{convocatoria}\".");
                    return;
                }
                bool esWriteIn = combo.SelectedItem as string == OpcionWriteIn;
                string candidato = esWriteIn ? txtWriteIn.Text.Trim() : (string)combo.SelectedItem!;
                if (candidato.Length == 0)
                {
                    Aviso($"Escribe el nombre del candidato no registrado en \"{convocatoria}\".");
                    return;
                }
                votos.Add(new Voto { Convocatoria = convocatoria, Candidato = candidato, EsWriteIn = esWriteIn });
            }

            // 3) Voto duplicado
            var repetidas = _service.ConvocatoriasYaVotadas(alumno.Id, _controles.Keys);
            if (repetidas.Count > 0)
            {
                Aviso("Este alumno ya voto en: " + string.Join(", ", repetidas) + ".");
                return;
            }

            // 4) Confirmacion
            string resumen = string.Join(Environment.NewLine,
                votos.Select(v => $"- {v.Convocatoria}: {v.Candidato}{(v.EsWriteIn ? " (no registrado)" : "")}"));
            var r = MessageBox.Show(
                $"Alumno: {alumno.Id}{Environment.NewLine}{Environment.NewLine}{resumen}{Environment.NewLine}{Environment.NewLine}" +
                "Una vez confirmado, el voto no se puede cambiar. Confirmar?",
                "Confirmar voto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            // 5) Registrar
            if (!_service.RegistrarVotos(alumno, votos, out error)) { Aviso(error); return; }

            MessageBox.Show("Voto registrado correctamente.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            txtId.Clear(); txtGrupo.Clear(); txtCarrera.Clear(); txtCentro.Clear();
            foreach (var (combo, txtWriteIn) in _controles.Values)
            {
                combo.SelectedIndex = -1;
                txtWriteIn.Clear();
                txtWriteIn.Visible = false;
            }
            txtId.Focus();
        }

        private static void Aviso(string mensaje) =>
            MessageBox.Show(mensaje, "Revisa los datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
