namespace sistema_votacion_de_alumnos
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F);
            label1.Location = new Point(277, 9);
            label1.Name = "label1";
            label1.Size = new Size(215, 30);
            label1.TabIndex = 0;
            label1.Text = "Votacion de alumnos";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 16F);
            label2.Location = new Point(12, 85);
            label2.Name = "label2";
            label2.Size = new Size(211, 30);
            label2.TabIndex = 1;
            label2.Text = "Candidato de area A";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 16F);
            label3.Location = new Point(12, 150);
            label3.Name = "label3";
            label3.Size = new Size(210, 30);
            label3.TabIndex = 2;
            label3.Text = "Candidato de area B";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 16F);
            label4.Location = new Point(12, 210);
            label4.Name = "label4";
            label4.Size = new Size(211, 30);
            label4.TabIndex = 3;
            label4.Text = "Candidato de area C";
            label4.Click += label4_Click;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 16F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(229, 77);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(210, 38);
            comboBox1.TabIndex = 4;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 16F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(229, 142);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(210, 38);
            comboBox2.TabIndex = 5;
            // 
            // comboBox3
            // 
            comboBox3.Font = new Font("Segoe UI", 16F);
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(229, 202);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(210, 38);
            comboBox3.TabIndex = 6;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 16F);
            button1.Location = new Point(603, 146);
            button1.Name = "button1";
            button1.Size = new Size(117, 38);
            button1.TabIndex = 7;
            button1.Text = "Aceptar";
            button1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(comboBox3);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Button button1;
    }
}
