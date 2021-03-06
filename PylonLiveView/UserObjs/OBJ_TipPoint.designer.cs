﻿namespace PylonLiveView
{
    partial class OBJ_TipPoint
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_move = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_accept = new System.Windows.Forms.Button();
            this.numericUpDown_Seque = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ZValue = new System.Windows.Forms.TextBox();
            this.comboBox_ZSymbol = new System.Windows.Forms.ComboBox();
            this.comboBox_lineSequence = new System.Windows.Forms.ComboBox();
            this.label_return = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_posZ = new System.Windows.Forms.Label();
            this.label_posY = new System.Windows.Forms.Label();
            this.label_posX = new System.Windows.Forms.Label();
            this.comboBox_AngleSymbol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_angle = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Seque)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SteelBlue;
            this.groupBox1.Controls.Add(this.button_move);
            this.groupBox1.Controls.Add(this.button_cancel);
            this.groupBox1.Controls.Add(this.button_accept);
            this.groupBox1.Controls.Add(this.numericUpDown_Seque);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_ZValue);
            this.groupBox1.Controls.Add(this.comboBox_ZSymbol);
            this.groupBox1.Controls.Add(this.comboBox_lineSequence);
            this.groupBox1.Controls.Add(this.label_return);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label_posZ);
            this.groupBox1.Controls.Add(this.label_posY);
            this.groupBox1.Controls.Add(this.label_posX);
            this.groupBox1.Controls.Add(this.comboBox_AngleSymbol);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_angle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.Chocolate;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(144, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "標點_";
            // 
            // button_move
            // 
            this.button_move.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_move.ForeColor = System.Drawing.Color.Black;
            this.button_move.Location = new System.Drawing.Point(87, 40);
            this.button_move.Name = "button_move";
            this.button_move.Size = new System.Drawing.Size(53, 23);
            this.button_move.TabIndex = 64;
            this.button_move.Text = "Go";
            this.button_move.UseVisualStyleBackColor = true;
            this.button_move.Click += new System.EventHandler(this.button_move_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Enabled = false;
            this.button_cancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_cancel.Location = new System.Drawing.Point(109, 65);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(29, 21);
            this.button_cancel.TabIndex = 5;
            this.button_cancel.Text = "X";
            this.toolTip1.SetToolTip(this.button_cancel, "恢復原排序");
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_accept
            // 
            this.button_accept.Enabled = false;
            this.button_accept.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_accept.Location = new System.Drawing.Point(80, 65);
            this.button_accept.Name = "button_accept";
            this.button_accept.Size = new System.Drawing.Size(29, 21);
            this.button_accept.TabIndex = 4;
            this.button_accept.Text = "√";
            this.toolTip1.SetToolTip(this.button_accept, "確定修改排序");
            this.button_accept.UseVisualStyleBackColor = true;
            this.button_accept.Click += new System.EventHandler(this.button_accept_Click);
            // 
            // numericUpDown_Seque
            // 
            this.numericUpDown_Seque.BackColor = System.Drawing.Color.Gold;
            this.numericUpDown_Seque.Enabled = false;
            this.numericUpDown_Seque.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_Seque.Location = new System.Drawing.Point(42, 65);
            this.numericUpDown_Seque.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDown_Seque.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Seque.Name = "numericUpDown_Seque";
            this.numericUpDown_Seque.Size = new System.Drawing.Size(39, 21);
            this.numericUpDown_Seque.TabIndex = 3;
            this.numericUpDown_Seque.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Seque.ValueChanged += new System.EventHandler(this.numericUpDown_Seque_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(3, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 62;
            this.label2.Text = "排序:";
            // 
            // textBox_ZValue
            // 
            this.textBox_ZValue.BackColor = System.Drawing.Color.BurlyWood;
            this.textBox_ZValue.Enabled = false;
            this.textBox_ZValue.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ZValue.Location = new System.Drawing.Point(83, 104);
            this.textBox_ZValue.Name = "textBox_ZValue";
            this.textBox_ZValue.Size = new System.Drawing.Size(58, 23);
            this.textBox_ZValue.TabIndex = 61;
            this.textBox_ZValue.Text = "0.00";
            this.textBox_ZValue.Leave += new System.EventHandler(this.textBox_ZValue_Leave);
            // 
            // comboBox_ZSymbol
            // 
            this.comboBox_ZSymbol.BackColor = System.Drawing.Color.BurlyWood;
            this.comboBox_ZSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ZSymbol.Enabled = false;
            this.comboBox_ZSymbol.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_ZSymbol.FormattingEnabled = true;
            this.comboBox_ZSymbol.Items.AddRange(new object[] {
            "+",
            "-"});
            this.comboBox_ZSymbol.Location = new System.Drawing.Point(45, 104);
            this.comboBox_ZSymbol.Name = "comboBox_ZSymbol";
            this.comboBox_ZSymbol.Size = new System.Drawing.Size(37, 22);
            this.comboBox_ZSymbol.TabIndex = 60;
            // 
            // comboBox_lineSequence
            // 
            this.comboBox_lineSequence.BackColor = System.Drawing.Color.BurlyWood;
            this.comboBox_lineSequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_lineSequence.Enabled = false;
            this.comboBox_lineSequence.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_lineSequence.FormattingEnabled = true;
            this.comboBox_lineSequence.Items.AddRange(new object[] {
            "正序",
            "反序"});
            this.comboBox_lineSequence.Location = new System.Drawing.Point(109, 0);
            this.comboBox_lineSequence.Name = "comboBox_lineSequence";
            this.comboBox_lineSequence.Size = new System.Drawing.Size(77, 20);
            this.comboBox_lineSequence.TabIndex = 1;
            this.comboBox_lineSequence.Visible = false;
            this.comboBox_lineSequence.SelectedIndexChanged += new System.EventHandler(this.comboBox_lineSequence_SelectedIndexChanged);
            // 
            // label_return
            // 
            this.label_return.AutoSize = true;
            this.label_return.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_return.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_return.Location = new System.Drawing.Point(3, 43);
            this.label_return.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_return.Name = "label_return";
            this.label_return.Size = new System.Drawing.Size(83, 12);
            this.label_return.TabIndex = 11;
            this.label_return.Text = "運動到標籤點:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(52, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "線序設定:";
            this.label3.Visible = false;
            // 
            // label_posZ
            // 
            this.label_posZ.AutoSize = true;
            this.label_posZ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_posZ.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_posZ.Location = new System.Drawing.Point(8, 108);
            this.label_posZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_posZ.Name = "label_posZ";
            this.label_posZ.Size = new System.Drawing.Size(28, 14);
            this.label_posZ.TabIndex = 9;
            this.label_posZ.Text = "Z :";
            // 
            // label_posY
            // 
            this.label_posY.AutoSize = true;
            this.label_posY.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_posY.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_posY.Location = new System.Drawing.Point(72, 24);
            this.label_posY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_posY.Name = "label_posY";
            this.label_posY.Size = new System.Drawing.Size(17, 12);
            this.label_posY.TabIndex = 8;
            this.label_posY.Text = "Y:";
            // 
            // label_posX
            // 
            this.label_posX.AutoSize = true;
            this.label_posX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_posX.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_posX.Location = new System.Drawing.Point(6, 24);
            this.label_posX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_posX.Name = "label_posX";
            this.label_posX.Size = new System.Drawing.Size(17, 12);
            this.label_posX.TabIndex = 7;
            this.label_posX.Text = "X:";
            // 
            // comboBox_AngleSymbol
            // 
            this.comboBox_AngleSymbol.BackColor = System.Drawing.Color.BurlyWood;
            this.comboBox_AngleSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AngleSymbol.Enabled = false;
            this.comboBox_AngleSymbol.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_AngleSymbol.FormattingEnabled = true;
            this.comboBox_AngleSymbol.Items.AddRange(new object[] {
            "+",
            "-"});
            this.comboBox_AngleSymbol.Location = new System.Drawing.Point(42, 65);
            this.comboBox_AngleSymbol.Name = "comboBox_AngleSymbol";
            this.comboBox_AngleSymbol.Size = new System.Drawing.Size(39, 20);
            this.comboBox_AngleSymbol.TabIndex = 1;
            this.comboBox_AngleSymbol.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(4, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "角度:";
            this.label1.Visible = false;
            // 
            // comboBox_angle
            // 
            this.comboBox_angle.BackColor = System.Drawing.Color.BurlyWood;
            this.comboBox_angle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_angle.Enabled = false;
            this.comboBox_angle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_angle.FormattingEnabled = true;
            this.comboBox_angle.Items.AddRange(new object[] {
            "0",
            "180"});
            this.comboBox_angle.Location = new System.Drawing.Point(80, 65);
            this.comboBox_angle.Name = "comboBox_angle";
            this.comboBox_angle.Size = new System.Drawing.Size(58, 20);
            this.comboBox_angle.TabIndex = 63;
            this.comboBox_angle.Visible = false;
            this.comboBox_angle.SelectedIndexChanged += new System.EventHandler(this.comboBox_angle_SelectedIndexChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.DarkSalmon;
            // 
            // OBJ_TipPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "OBJ_TipPoint";
            this.Size = new System.Drawing.Size(144, 97);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Seque)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_posY;
        private System.Windows.Forms.Label label_posX;
        private System.Windows.Forms.ComboBox comboBox_AngleSymbol;
        private System.Windows.Forms.ComboBox comboBox_ZSymbol;
        private System.Windows.Forms.TextBox textBox_ZValue;
        private System.Windows.Forms.Label label_posZ;
        private System.Windows.Forms.NumericUpDown numericUpDown_Seque;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_accept;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBox_angle;
        private System.Windows.Forms.ComboBox comboBox_lineSequence;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_move;
        private System.Windows.Forms.Label label_return;

    }
}
