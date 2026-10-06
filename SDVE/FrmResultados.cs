namespace SDVE
{
    /// <summary>Pantalla de resultados: tabla, graficas, filtros, participacion y exportacion.</summary>
    public class FrmResultados : Form
    {
        private const string Todos = "(Todos)";

        private readonly ResultadoService _service;
        private readonly ExportacionService _exportacion = new();

        private readonly ComboBox cmbGrupo = new();
        private readonly ComboBox cmbCarrera = new();
        private readonly ComboBox cmbCentro = new();
        private readonly ComboBox cmbConvocatoria = new();
        private readonly NumericUpDown numPadron = new();
        private readonly DataGridView dgv = new();
        private readonly GraficaBarras graficaResultados = new();
        private readonly GraficaBarras graficaParticipacion = new();
        private readonly Label lblEstadisticas = new();

        private bool _cargando;
        private List<ResultadoConvocatoria> _resultados = new();
        private Estadisticas? _general;
        private List<Estadisticas> _porConvocatoria = new();

        /// <summary>
        /// Pasa el MISMO VotacionService que usa la pantalla de votacion para ver los votos nuevos.
        /// Sin parametros crea uno propio que lee votos.csv.
        /// </summary>
        public FrmResultados(VotacionService? votacion = null)
        {
            _service = new ResultadoService(votacion ?? new VotacionService());
            ConstruirInterfaz();
            CargarFiltros();
            Actualizar();
        }

        private void ConstruirInterfaz()
        {
            Text = "Resultados de la votacion";
            ClientSize = new Size(1020, 700);
            MinimumSize = new Size(900, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);

            var titulo = new Label
            {
                Text = "Resultados",
                Font = new Font("Segoe UI", 16F),
                AutoSize = true,
                Location = new Point(12, 8)
            };

            // --- Filtros ---
            var grpFiltros = new GroupBox
            {
                Text = "Filtros",
                Location = new Point(12, 46),
                Size = new Size(996, 80),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            AgregarFiltro(grpFiltros, "Grupo", cmbGrupo, 12);
            AgregarFiltro(grpFiltros, "Carrera", cmbCarrera, 192);
            AgregarFiltro(grpFiltros, "Centro universitario", cmbCentro, 372);

            grpFiltros.Controls.Add(new Label { Text = "Alumnos en el padron", AutoSize = true, Location = new Point(562, 20) });
            numPadron.Location = new Point(562, 42);
            numPadron.Width = 140;
            numPadron.Minimum = 0;
            numPadron.Maximum = 1000000;
            numPadron.ValueChanged += (_, _) => Actualizar();
            grpFiltros.Controls.Add(numPadron);

            var btnActualizar = new Button { Text = "Actualizar", Location = new Point(720, 38), Size = new Size(120, 32) };
            btnActualizar.Click += (_, _) => { CargarFiltros(); Actualizar(); };
            grpFiltros.Controls.Add(btnActualizar);

            // --- Tabla ---
            dgv.Location = new Point(12, 136);
            dgv.Size = new Size(500, 446);
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.Columns.Add("conv", "Convocatoria");
            dgv.Columns.Add("cand", "Candidato");
            dgv.Columns.Add("tipo", "Tipo");
            dgv.Columns.Add("votos", "Votos");
            dgv.Columns.Add("pct", "%");
            dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // --- Graficas ---
            var lblConv = new Label { Text = "Convocatoria:", AutoSize = true, Location = new Point(524, 141) };
            cmbConvocatoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConvocatoria.Location = new Point(640, 136);
            cmbConvocatoria.Width = 340;
            cmbConvocatoria.SelectedIndexChanged += (_, _) => { if (!_cargando) ActualizarGraficaResultados(); };

            graficaResultados.Location = new Point(524, 172);
            graficaResultados.Size = new Size(484, 230);
            graficaResultados.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            graficaParticipacion.Location = new Point(524, 412);
            graficaParticipacion.Size = new Size(484, 170);
            graficaParticipacion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // --- Estadisticas y exportacion ---
            lblEstadisticas.Location = new Point(12, 590);
            lblEstadisticas.Size = new Size(996, 54);
            lblEstadisticas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            var btnCsv = new Button { Text = "Exportar CSV", Location = new Point(12, 654), Size = new Size(150, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            var btnJson = new Button { Text = "Exportar JSON", Location = new Point(172, 654), Size = new Size(150, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            var btnXml = new Button { Text = "Exportar XML", Location = new Point(332, 654), Size = new Size(150, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            btnCsv.Click += (_, _) => Exportar("Archivo CSV (*.csv)|*.csv", "csv", _exportacion.ExportarCsv);
            btnJson.Click += (_, _) => Exportar("Archivo JSON (*.json)|*.json", "json", _exportacion.ExportarJson);
            btnXml.Click += (_, _) => Exportar("Archivo XML (*.xml)|*.xml", "xml", _exportacion.ExportarXml);

            Controls.AddRange(new Control[]
            {
                titulo, grpFiltros, dgv, lblConv, cmbConvocatoria,
                graficaResultados, graficaParticipacion, lblEstadisticas, btnCsv, btnJson, btnXml
            });
        }

        private void AgregarFiltro(GroupBox grp, string texto, ComboBox cmb, int x)
        {
            grp.Controls.Add(new Label { Text = texto, AutoSize = true, Location = new Point(x, 20) });
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Location = new Point(x, 42);
            cmb.Width = 170;
            cmb.SelectedIndexChanged += (_, _) => Actualizar();
            grp.Controls.Add(cmb);
        }

        /// <summary>Llena los filtros con los valores que existen en los votos (conserva la seleccion).</summary>
        private void CargarFiltros()
        {
            _cargando = true;
            LlenarCombo(cmbGrupo, _service.ObtenerGrupos());
            LlenarCombo(cmbCarrera, _service.ObtenerCarreras());
            LlenarCombo(cmbCentro, _service.ObtenerCentros());
            _cargando = false;
        }

        private static void LlenarCombo(ComboBox cmb, List<string> valores)
        {
            string? seleccion = cmb.SelectedItem as string;
            cmb.Items.Clear();
            cmb.Items.Add(Todos);
            foreach (var v in valores) cmb.Items.Add(v);
            cmb.SelectedIndex = seleccion != null && cmb.Items.Contains(seleccion) ? cmb.Items.IndexOf(seleccion) : 0;
        }

        private FiltroResultados ObtenerFiltro() => new()
        {
            Grupo = Valor(cmbGrupo),
            Carrera = Valor(cmbCarrera),
            CentroUniversitario = Valor(cmbCentro)
        };

        private static string? Valor(ComboBox cmb) =>
            cmb.SelectedItem is string s && s != Todos ? s : null;

        /// <summary>Recalcula y redibuja todo con el filtro y padron actuales.</summary>
        private void Actualizar()
        {
            if (_cargando) return;

            var filtro = ObtenerFiltro();
            _resultados = _service.ContarVotos(filtro);

            // Tabla
            dgv.Rows.Clear();
            foreach (var conv in _resultados)
                foreach (var c in conv.Candidatos)
                    dgv.Rows.Add(conv.Convocatoria, c.Candidato, c.EsWriteIn ? "No registrado" : "Registrado",
                                 c.Votos, $"{c.Porcentaje:0.##} %");

            // Selector de convocatoria para la grafica
            string? anterior = cmbConvocatoria.SelectedItem as string;
            _cargando = true;
            cmbConvocatoria.Items.Clear();
            foreach (var conv in _resultados) cmbConvocatoria.Items.Add(conv.Convocatoria);
            if (cmbConvocatoria.Items.Count > 0)
                cmbConvocatoria.SelectedIndex = anterior != null && cmbConvocatoria.Items.Contains(anterior)
                    ? cmbConvocatoria.Items.IndexOf(anterior) : 0;
            _cargando = false;

            ActualizarGraficaResultados();
            ActualizarParticipacion(filtro);
        }

        private void ActualizarGraficaResultados()
        {
            var conv = _resultados.FirstOrDefault(r => r.Convocatoria == cmbConvocatoria.SelectedItem as string);
            if (conv == null)
            {
                graficaResultados.SetDatos("Votos por candidato", new List<(string, double, string)>());
                return;
            }

            var datos = conv.Candidatos.Select(c => (
                c.Candidato + (c.EsWriteIn ? " (no reg.)" : ""),
                (double)c.Votos,
                $"{c.Votos} ({c.Porcentaje:0.##}%)"));
            graficaResultados.SetDatos($"Votos por candidato - {conv.Convocatoria}", datos);
        }

        private void ActualizarParticipacion(FiltroResultados filtro)
        {
            int padron = (int)numPadron.Value;

            if (padron <= 0)
            {
                _general = null;
                _porConvocatoria = new List<Estadisticas>();
                graficaParticipacion.SetDatos("Participacion y abstencionismo", new List<(string, double, string)>());
                lblEstadisticas.Text = $"Votos registrados con el filtro actual: {_service.TotalVotos(filtro)}. " +
                                       "Escribe cuantos alumnos hay en el padron para calcular participacion y abstencionismo.";
                return;
            }

            _general = _service.ParticipacionGeneral(padron, filtro);
            _porConvocatoria = _service.ParticipacionPorConvocatoria(padron, filtro);

            var datos = new List<(string, double, string)>
            {
                ("Participacion general", _general.PorcentajeParticipacion, $"{_general.PorcentajeParticipacion:0.##}%"),
                ("Abstencionismo general", _general.PorcentajeAbstencionismo, $"{_general.PorcentajeAbstencionismo:0.##}%")
            };
            foreach (var est in _porConvocatoria)
                datos.Add(($"Participacion - {est.Convocatoria}", est.PorcentajeParticipacion, $"{est.PorcentajeParticipacion:0.##}%"));

            graficaParticipacion.SetDatos("Participacion y abstencionismo (%)", datos, 100);
            lblEstadisticas.Text =
                $"Padron: {_general.Padron}  |  Alumnos que votaron: {_general.Votantes}  |  " +
                $"Participacion: {_general.PorcentajeParticipacion:0.##}%  |  " +
                $"Abstencionismo: {_general.PorcentajeAbstencionismo:0.##}% ({_general.Abstenciones} alumnos)";
        }

        private ReporteResultados CrearReporte() => new()
        {
            FechaGeneracion = DateTime.Now,
            Filtro = ObtenerFiltro(),
            Resultados = _resultados,
            ParticipacionGeneral = _general,
            ParticipacionPorConvocatoria = _porConvocatoria
        };

        private void Exportar(string filtroDialogo, string extension, Action<string, ReporteResultados> exportar)
        {
            if (_resultados.Count == 0)
            {
                MessageBox.Show("No hay resultados para exportar con el filtro actual.", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter = filtroDialogo,
                DefaultExt = extension,
                AddExtension = true,
                FileName = $"resultados_{DateTime.Now:yyyyMMdd_HHmm}.{extension}"
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                exportar(dlg.FileName, CrearReporte());
                MessageBox.Show("Archivo guardado en:" + Environment.NewLine + dlg.FileName, "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el archivo (si esta abierto en Excel, cierralo): " + ex.Message,
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
