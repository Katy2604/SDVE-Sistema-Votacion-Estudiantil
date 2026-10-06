namespace SDVE
{
    partial class Votacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLimpiar = new Button();
            btnVotar = new Button();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            cmbConvocatoria = new ComboBox();
            grpCandidatos = new GroupBox();
            btnSalir = new Button();
            SuspendLayout();
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.SteelBlue;
            btnLimpiar.Font = new Font("Rockwell", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLimpiar.ForeColor = SystemColors.Control;
            btnLimpiar.Location = new Point(37, 429);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(130, 43);
            btnLimpiar.TabIndex = 13;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVotar
            // 
            btnVotar.BackColor = Color.SteelBlue;
            btnVotar.Font = new Font("Rockwell", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnVotar.ForeColor = SystemColors.Control;
            btnVotar.Location = new Point(127, 360);
            btnVotar.Name = "btnVotar";
            btnVotar.Size = new Size(190, 43);
            btnVotar.TabIndex = 12;
            btnVotar.Text = "Emitir Voto";
            btnVotar.UseVisualStyleBackColor = false;
            btnVotar.Click += btnVotar_Click;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe Fluent Icons", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.Control;
            label2.Location = new Point(15, 68);
            label2.Name = "label2";
            label2.Size = new Size(123, 34);
            label2.TabIndex = 8;
            label2.Text = "Convocatoria";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("NSimSun", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.LightYellow;
            label3.ImageAlign = ContentAlignment.TopCenter;
            label3.Location = new Point(15, 23);
            label3.Name = "label3";
            label3.Size = new Size(413, 30);
            label3.TabIndex = 15;
            label3.Text = "VOTACIÓN ESTUDIANTIL SDVE";
            label3.TextAlign = ContentAlignment.TopCenter;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe Fluent Icons", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(15, 172);
            label1.Name = "label1";
            label1.Size = new Size(224, 34);
            label1.TabIndex = 16;
            label1.Text = "Selecciona un candidato";
            // 
            // cmbConvocatoria
            // 
            cmbConvocatoria.FormattingEnabled = true;
            cmbConvocatoria.Items.AddRange(new object[] { "Sociedad Alumnos", "Consejo Universitario", "Consejo de Representantes" });
            cmbConvocatoria.Location = new Point(57, 114);
            cmbConvocatoria.Name = "cmbConvocatoria";
            cmbConvocatoria.Size = new Size(289, 29);
            cmbConvocatoria.TabIndex = 17;
            cmbConvocatoria.SelectedIndexChanged += cmbConvocatoria_SelectedIndexChanged;
            // 
            // grpCandidatos
            // 
            grpCandidatos.Location = new Point(92, 209);
            grpCandidatos.Name = "grpCandidatos";
            grpCandidatos.Size = new Size(209, 125);
            grpCandidatos.TabIndex = 18;
            grpCandidatos.TabStop = false;
            // 
            // btnSalir
            // 
            btnSalir.BackColor = Color.SteelBlue;
            btnSalir.Font = new Font("Rockwell", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalir.ForeColor = SystemColors.Control;
            btnSalir.Location = new Point(265, 429);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(104, 43);
            btnSalir.TabIndex = 19;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // Votacion
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(438, 497);
            Controls.Add(btnSalir);
            Controls.Add(grpCandidatos);
            Controls.Add(cmbConvocatoria);
            Controls.Add(label1);
            Controls.Add(label3);
            Controls.Add(btnLimpiar);
            Controls.Add(btnVotar);
            Controls.Add(label2);
            Name = "Votacion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Votacion";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLimpiar;
        private Button btnVotar;
        private Label label2;
        private Label label3;
        private Label label1;
        private ComboBox cmbConvocatoria;
        private GroupBox grpCandidatos;
        private Button btnSalir;
    }
}