using System.Drawing.Drawing2D;

namespace SDVE
{
    /// <summary>
    /// Grafica de barras horizontales dibujada con GDI+ (no necesita paquetes externos).
    /// Se usa para votos por candidato y para participacion/abstencionismo.
    /// </summary>
    public class GraficaBarras : Control
    {
        private static readonly Color[] Paleta =
        {
            Color.FromArgb(52, 120, 198), Color.FromArgb(230, 126, 34), Color.FromArgb(46, 160, 100),
            Color.FromArgb(155, 89, 182), Color.FromArgb(214, 69, 65), Color.FromArgb(22, 160, 170)
        };

        private List<(string Etiqueta, double Valor, string Texto)> _datos = new();
        private string _titulo = "";
        private double _maximoFijo;

        public GraficaBarras()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;
        }

        /// <param name="titulo">Titulo de la grafica.</param>
        /// <param name="datos">Etiqueta, valor (largo de la barra) y texto que se muestra junto a la barra.</param>
        /// <param name="maximoFijo">Si es mayor que 0, la escala llega hasta ese valor (ej. 100 para porcentajes).</param>
        public void SetDatos(string titulo, IEnumerable<(string Etiqueta, double Valor, string Texto)> datos, double maximoFijo = 0)
        {
            _titulo = titulo;
            _datos = datos.ToList();
            _maximoFijo = maximoFijo;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using var fuenteTitulo = new Font(Font, FontStyle.Bold);
            g.DrawString(_titulo, fuenteTitulo, Brushes.Black, 8, 6);

            if (_datos.Count == 0)
            {
                g.DrawString("Sin datos para mostrar", Font, Brushes.Gray, 8, 36);
                return;
            }

            const int top = 34;
            const int margen = 8;
            int anchoEtiqueta = Math.Min(190, Width / 3);
            int anchoTexto = 80;
            int inicioBarra = margen + anchoEtiqueta + 6;
            int anchoMax = Math.Max(10, Width - inicioBarra - anchoTexto - margen);
            double maximo = _maximoFijo > 0 ? _maximoFijo : Math.Max(1, _datos.Max(d => d.Valor));
            float alto = Math.Min(36f, (Height - top - margen) / (float)_datos.Count);

            using var formato = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };

            for (int i = 0; i < _datos.Count; i++)
            {
                var (etiqueta, valor, texto) = _datos[i];
                float y = top + i * alto;

                g.DrawString(etiqueta, Font, Brushes.Black, new RectangleF(margen, y, anchoEtiqueta, alto), formato);

                float largo = (float)(Math.Min(valor, maximo) / maximo * anchoMax);
                var barra = new RectangleF(inicioBarra, y + alto * 0.18f, Math.Max(largo, 1f), alto * 0.64f);
                using var pincel = new SolidBrush(Paleta[i % Paleta.Length]);
                g.FillRectangle(pincel, barra);

                g.DrawString(texto, Font, Brushes.Black, new RectangleF(barra.Right + 4, y, anchoTexto, alto), formato);
            }
        }
    }
}
