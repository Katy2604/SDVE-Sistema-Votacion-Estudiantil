namespace SDVE
{
    partial class Convocatorias
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
            label1 = new Label();
            label2 = new Label();
            chkSociedadAlumnos = new CheckBox();
            chkConsejoUniversitario = new CheckBox();
            chkConsejoRepresentantes = new CheckBox();
            btnContinuar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("NSimSun", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Gold;
            label1.ImageAlign = ContentAlignment.TopCenter;
            label1.Location = new Point(52, 35);
            label1.Name = "label1";
            label1.Size = new Size(333, 60);
            label1.TabIndex = 0;
            label1.Text = "Sistema de Votación \r\nEstudiantil";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe Fluent Icons", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(32, 126);
            label2.Name = "label2";
            label2.Size = new Size(369, 58);
            label2.TabIndex = 1;
            label2.Text = "Selecciona las convocatorias en las que deseas participar";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // chkSociedadAlumnos
            // 
            chkSociedadAlumnos.AutoSize = true;
            chkSociedadAlumnos.CheckAlign = ContentAlignment.TopLeft;
            chkSociedadAlumnos.Font = new Font("Goudy Old Style", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkSociedadAlumnos.Location = new Point(119, 214);
            chkSociedadAlumnos.Name = "chkSociedadAlumnos";
            chkSociedadAlumnos.Size = new Size(185, 27);
            chkSociedadAlumnos.TabIndex = 2;
            chkSociedadAlumnos.Text = "Sociedad Alumnos";
            chkSociedadAlumnos.UseVisualStyleBackColor = true;
            // 
            // chkConsejoUniversitario
            // 
            chkConsejoUniversitario.AutoSize = true;
            chkConsejoUniversitario.CheckAlign = ContentAlignment.TopLeft;
            chkConsejoUniversitario.Font = new Font("Goudy Old Style", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkConsejoUniversitario.Location = new Point(119, 257);
            chkConsejoUniversitario.Name = "chkConsejoUniversitario";
            chkConsejoUniversitario.Size = new Size(213, 27);
            chkConsejoUniversitario.TabIndex = 3;
            chkConsejoUniversitario.Text = "Consejo Universitario";
            chkConsejoUniversitario.UseVisualStyleBackColor = true;
            // 
            // chkConsejoRepresentantes
            // 
            chkConsejoRepresentantes.AutoSize = true;
            chkConsejoRepresentantes.CheckAlign = ContentAlignment.TopLeft;
            chkConsejoRepresentantes.Font = new Font("Goudy Old Style", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkConsejoRepresentantes.Location = new Point(119, 300);
            chkConsejoRepresentantes.Name = "chkConsejoRepresentantes";
            chkConsejoRepresentantes.Size = new Size(253, 27);
            chkConsejoRepresentantes.TabIndex = 4;
            chkConsejoRepresentantes.Text = "Consejo de Representantes";
            chkConsejoRepresentantes.UseVisualStyleBackColor = true;
            // 
            // btnContinuar
            // 
            btnContinuar.BackColor = Color.SteelBlue;
            btnContinuar.Font = new Font("Rockwell", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnContinuar.ForeColor = SystemColors.Control;
            btnContinuar.Location = new Point(19, 379);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(157, 43);
            btnContinuar.TabIndex = 5;
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = false;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.SteelBlue;
            btnCancelar.Font = new Font("Rockwell", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.ForeColor = SystemColors.Control;
            btnCancelar.Location = new Point(244, 379);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(157, 43);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            // 
            // Convocatorias
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(439, 453);
            Controls.Add(btnCancelar);
            Controls.Add(btnContinuar);
            Controls.Add(chkConsejoRepresentantes);
            Controls.Add(chkConsejoUniversitario);
            Controls.Add(chkSociedadAlumnos);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = SystemColors.Control;
            Name = "Convocatorias";
            Text = "Convocatorias";
            TransparencyKey = Color.Transparent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private CheckBox chkSociedadAlumnos;
        private CheckBox chkConsejoUniversitario;
        private CheckBox chkConsejoRepresentantes;
        private Button btnContinuar;
        private Button btnCancelar;
    }
}