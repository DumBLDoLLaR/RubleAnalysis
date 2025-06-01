namespace RubleAnalysis
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
            dataGridView1 = new DataGridView();
            groupBox1 = new GroupBox();
            groupBox3 = new GroupBox();
            button7 = new Button();
            textBox5 = new TextBox();
            label5 = new Label();
            textBox3 = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label1 = new Label();
            button5 = new Button();
            button3 = new Button();
            button1 = new Button();
            groupBox2 = new GroupBox();
            groupBox4 = new GroupBox();
            button8 = new Button();
            textBox6 = new TextBox();
            label6 = new Label();
            textBox4 = new TextBox();
            textBox2 = new TextBox();
            label4 = new Label();
            label2 = new Label();
            button6 = new Button();
            button4 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(399, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(646, 600);
            dataGridView1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(2, 1);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(391, 298);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "ВВП и ВНП России";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button7);
            groupBox3.Controls.Add(textBox5);
            groupBox3.Controls.Add(label5);
            groupBox3.Location = new Point(6, 97);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(378, 131);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Экстраполяция";
            // 
            // button7
            // 
            button7.Location = new Point(228, 30);
            button7.Name = "button7";
            button7.Size = new Size(122, 61);
            button7.TabIndex = 8;
            button7.Text = "Произвести рассчет";
            button7.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(6, 55);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(206, 31);
            textBox5.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 27);
            label5.Name = "label5";
            label5.Size = new Size(206, 25);
            label5.TabIndex = 8;
            label5.Text = "Введите количество лет";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(234, 267);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(150, 31);
            textBox3.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(234, 231);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(150, 31);
            textBox1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(134, 267);
            label3.Name = "label3";
            label3.Size = new Size(97, 25);
            label3.TabIndex = 4;
            label3.Text = "Минимум:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(134, 231);
            label1.Name = "label1";
            label1.Size = new Size(103, 25);
            label1.TabIndex = 3;
            label1.Text = "Максимум:";
            // 
            // button5
            // 
            button5.Location = new Point(6, 231);
            button5.Name = "button5";
            button5.Size = new Size(122, 61);
            button5.TabIndex = 2;
            button5.Text = "Показать экстремумы";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button3
            // 
            button3.Location = new Point(134, 30);
            button3.Name = "button3";
            button3.Size = new Size(122, 61);
            button3.TabIndex = 1;
            button3.Text = "Построить график";
            button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(6, 30);
            button1.Name = "button1";
            button1.Size = new Size(122, 61);
            button1.TabIndex = 0;
            button1.Text = "Отобразить таблицу";
            button1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(textBox4);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(button2);
            groupBox2.Location = new Point(2, 314);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(391, 298);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Средняя ЗП по России";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button8);
            groupBox4.Controls.Add(textBox6);
            groupBox4.Controls.Add(label6);
            groupBox4.Location = new Point(6, 97);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(378, 131);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "Экстраполяция";
            // 
            // button8
            // 
            button8.Location = new Point(228, 40);
            button8.Name = "button8";
            button8.Size = new Size(122, 61);
            button8.TabIndex = 9;
            button8.Text = "Произвести рассчет";
            button8.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(6, 55);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(206, 31);
            textBox6.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 27);
            label6.Name = "label6";
            label6.Size = new Size(206, 25);
            label6.TabIndex = 9;
            label6.Text = "Введите количество лет";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(234, 267);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(150, 31);
            textBox4.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(234, 228);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(150, 31);
            textBox2.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(134, 267);
            label4.Name = "label4";
            label4.Size = new Size(97, 25);
            label4.TabIndex = 5;
            label4.Text = "Минимум:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(134, 231);
            label2.Name = "label2";
            label2.Size = new Size(103, 25);
            label2.TabIndex = 4;
            label2.Text = "Максимум:";
            // 
            // button6
            // 
            button6.Location = new Point(6, 231);
            button6.Name = "button6";
            button6.Size = new Size(122, 61);
            button6.TabIndex = 3;
            button6.Text = "Показать экстремумы";
            button6.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(134, 30);
            button4.Name = "button4";
            button4.Size = new Size(122, 61);
            button4.TabIndex = 2;
            button4.Text = "Построить график";
            button4.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(6, 30);
            button2.Name = "button2";
            button2.Size = new Size(122, 61);
            button2.TabIndex = 1;
            button2.Text = "Отобразить таблицу";
            button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1055, 615);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private GroupBox groupBox1;
        private Button button1;
        private GroupBox groupBox2;
        private Button button2;
        private Label label1;
        private Button button5;
        private Button button3;
        private Label label2;
        private Button button6;
        private Button button4;
        private Label label3;
        private Label label4;
        private TextBox textBox3;
        private TextBox textBox1;
        private TextBox textBox4;
        private TextBox textBox2;
        private GroupBox groupBox3;
        private Button button7;
        private TextBox textBox5;
        private Label label5;
        private GroupBox groupBox4;
        private Button button8;
        private TextBox textBox6;
        private Label label6;
    }
}
