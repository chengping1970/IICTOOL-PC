namespace IICTool
{
    partial class I2CTool
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
            this.Label1 = new System.Windows.Forms.Label();
            this.SlaveAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SUBAddress = new System.Windows.Forms.TextBox();
            this.RegValue = new System.Windows.Forms.TextBox();
            this.cbBIT7 = new System.Windows.Forms.CheckBox();
            this.cbBIT6 = new System.Windows.Forms.CheckBox();
            this.cbBIT5 = new System.Windows.Forms.CheckBox();
            this.cbBIT4 = new System.Windows.Forms.CheckBox();
            this.cbBIT3 = new System.Windows.Forms.CheckBox();
            this.cbBIT2 = new System.Windows.Forms.CheckBox();
            this.cbBIT1 = new System.Windows.Forms.CheckBox();
            this.cbBIT0 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.RegData = new System.Windows.Forms.DataGridView();
            this.addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column00 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column05 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column06 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column07 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column08 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column09 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0D = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0E = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column0F = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.ReadAll = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RegData)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label1.Location = new System.Drawing.Point(10, 24);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(164, 21);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Device Address";
            // 
            // SlaveAddress
            // 
            this.SlaveAddress.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.SlaveAddress.Location = new System.Drawing.Point(118, 20);
            this.SlaveAddress.MaxLength = 2;
            this.SlaveAddress.Name = "SlaveAddress";
            this.SlaveAddress.Size = new System.Drawing.Size(55, 31);
            this.SlaveAddress.TabIndex = 1;
            this.SlaveAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SlaveAddress.TextChanged += new System.EventHandler(this.SlaveAddress_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "SUB Address";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(10, 104);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(98, 21);
            this.Label4.TabIndex = 4;
            this.Label4.Text = "BIT Edit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 21);
            this.label5.TabIndex = 5;
            this.label5.Text = "Value";
            // 
            // SUBAddress
            // 
            this.SUBAddress.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.SUBAddress.Location = new System.Drawing.Point(99, 61);
            this.SUBAddress.MaxLength = 2;
            this.SUBAddress.Name = "SUBAddress";
            this.SUBAddress.ReadOnly = true;
            this.SUBAddress.Size = new System.Drawing.Size(37, 31);
            this.SUBAddress.TabIndex = 6;
            this.SUBAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RegValue
            // 
            this.RegValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RegValue.Location = new System.Drawing.Point(198, 62);
            this.RegValue.MaxLength = 2;
            this.RegValue.Name = "RegValue";
            this.RegValue.Size = new System.Drawing.Size(37, 31);
            this.RegValue.TabIndex = 7;
            this.RegValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RegValue.TextChanged += new System.EventHandler(this.Value_TextChanged);
            // 
            // cbBIT7
            // 
            this.cbBIT7.AutoSize = true;
            this.cbBIT7.Location = new System.Drawing.Point(74, 105);
            this.cbBIT7.Name = "cbBIT7";
            this.cbBIT7.Size = new System.Drawing.Size(22, 21);
            this.cbBIT7.TabIndex = 8;
            this.cbBIT7.UseVisualStyleBackColor = true;
            this.cbBIT7.CheckedChanged += new System.EventHandler(this.cbBIT7_CheckedChanged);
            // 
            // cbBIT6
            // 
            this.cbBIT6.AutoSize = true;
            this.cbBIT6.Location = new System.Drawing.Point(95, 105);
            this.cbBIT6.Name = "cbBIT6";
            this.cbBIT6.Size = new System.Drawing.Size(22, 21);
            this.cbBIT6.TabIndex = 9;
            this.cbBIT6.UseVisualStyleBackColor = true;
            this.cbBIT6.CheckedChanged += new System.EventHandler(this.cbBIT6_CheckedChanged);
            // 
            // cbBIT5
            // 
            this.cbBIT5.AutoSize = true;
            this.cbBIT5.Location = new System.Drawing.Point(116, 105);
            this.cbBIT5.Name = "cbBIT5";
            this.cbBIT5.Size = new System.Drawing.Size(22, 21);
            this.cbBIT5.TabIndex = 10;
            this.cbBIT5.UseVisualStyleBackColor = true;
            this.cbBIT5.CheckedChanged += new System.EventHandler(this.cbBIT5_CheckedChanged);
            // 
            // cbBIT4
            // 
            this.cbBIT4.AutoSize = true;
            this.cbBIT4.Location = new System.Drawing.Point(137, 105);
            this.cbBIT4.Name = "cbBIT4";
            this.cbBIT4.Size = new System.Drawing.Size(22, 21);
            this.cbBIT4.TabIndex = 11;
            this.cbBIT4.UseVisualStyleBackColor = true;
            this.cbBIT4.CheckedChanged += new System.EventHandler(this.cbBIT4_CheckedChanged);
            // 
            // cbBIT3
            // 
            this.cbBIT3.AutoSize = true;
            this.cbBIT3.Location = new System.Drawing.Point(158, 105);
            this.cbBIT3.Name = "cbBIT3";
            this.cbBIT3.Size = new System.Drawing.Size(22, 21);
            this.cbBIT3.TabIndex = 12;
            this.cbBIT3.UseVisualStyleBackColor = true;
            this.cbBIT3.CheckedChanged += new System.EventHandler(this.cbBIT3_CheckedChanged);
            // 
            // cbBIT2
            // 
            this.cbBIT2.AutoSize = true;
            this.cbBIT2.Location = new System.Drawing.Point(179, 105);
            this.cbBIT2.Name = "cbBIT2";
            this.cbBIT2.Size = new System.Drawing.Size(22, 21);
            this.cbBIT2.TabIndex = 13;
            this.cbBIT2.UseVisualStyleBackColor = true;
            this.cbBIT2.CheckedChanged += new System.EventHandler(this.cbBIT2_CheckedChanged);
            // 
            // cbBIT1
            // 
            this.cbBIT1.AutoSize = true;
            this.cbBIT1.Location = new System.Drawing.Point(200, 105);
            this.cbBIT1.Name = "cbBIT1";
            this.cbBIT1.Size = new System.Drawing.Size(22, 21);
            this.cbBIT1.TabIndex = 14;
            this.cbBIT1.UseVisualStyleBackColor = true;
            this.cbBIT1.CheckedChanged += new System.EventHandler(this.cbBIT1_CheckedChanged);
            // 
            // cbBIT0
            // 
            this.cbBIT0.AutoSize = true;
            this.cbBIT0.Location = new System.Drawing.Point(221, 105);
            this.cbBIT0.Name = "cbBIT0";
            this.cbBIT0.Size = new System.Drawing.Size(22, 21);
            this.cbBIT0.TabIndex = 15;
            this.cbBIT0.UseVisualStyleBackColor = true;
            this.cbBIT0.CheckedChanged += new System.EventHandler(this.cbBIT0_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(75, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 21);
            this.label6.TabIndex = 16;
            this.label6.Text = "7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(96, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 21);
            this.label7.TabIndex = 17;
            this.label7.Text = "6";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 21);
            this.label8.TabIndex = 18;
            this.label8.Text = "5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(138, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 21);
            this.label9.TabIndex = 19;
            this.label9.Text = "4";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(159, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 21);
            this.label10.TabIndex = 20;
            this.label10.Text = "3";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(179, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 21);
            this.label11.TabIndex = 21;
            this.label11.Text = "2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(201, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(21, 21);
            this.label12.TabIndex = 22;
            this.label12.Text = "1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(222, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 21);
            this.label13.TabIndex = 23;
            this.label13.Text = "0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbBIT0);
            this.groupBox1.Controls.Add(this.cbBIT1);
            this.groupBox1.Controls.Add(this.cbBIT2);
            this.groupBox1.Controls.Add(this.cbBIT3);
            this.groupBox1.Controls.Add(this.cbBIT4);
            this.groupBox1.Controls.Add(this.cbBIT5);
            this.groupBox1.Controls.Add(this.cbBIT6);
            this.groupBox1.Controls.Add(this.cbBIT7);
            this.groupBox1.Controls.Add(this.RegValue);
            this.groupBox1.Controls.Add(this.SUBAddress);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SlaveAddress);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 136);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(333, 94);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(58, 33);
            this.button10.TabIndex = 33;
            this.button10.Text = "-10";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(333, 54);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(58, 34);
            this.button9.TabIndex = 32;
            this.button9.Text = "+10";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(262, 94);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(58, 33);
            this.button8.TabIndex = 31;
            this.button8.Text = "-1";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(262, 54);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(58, 34);
            this.button7.TabIndex = 30;
            this.button7.Text = "+1";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(412, 54);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 34);
            this.button4.TabIndex = 27;
            this.button4.Text = "Write";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(412, 94);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(66, 33);
            this.button3.TabIndex = 26;
            this.button3.Text = "Read";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cbSerial
            // 
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(90, 77);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbSerial.Size = new System.Drawing.Size(119, 29);
            this.cbSerial.TabIndex = 26;
            this.cbSerial.Click += new System.EventHandler(this.comboBox1_click);
            // 
            // RegData
            // 
            this.RegData.AllowUserToAddRows = false;
            this.RegData.AllowUserToDeleteRows = false;
            this.RegData.AllowUserToResizeColumns = false;
            this.RegData.AllowUserToResizeRows = false;
            this.RegData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RegData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addr,
            this.Column00,
            this.Column01,
            this.Column02,
            this.Column03,
            this.Column04,
            this.Column05,
            this.Column06,
            this.Column07,
            this.Column08,
            this.Column09,
            this.Column0A,
            this.Column0B,
            this.Column0C,
            this.Column0D,
            this.Column0E,
            this.Column0F});
            this.RegData.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.RegData.Location = new System.Drawing.Point(15, 161);
            this.RegData.Name = "RegData";
            this.RegData.RowHeadersVisible = false;
            this.RegData.RowTemplate.Height = 23;
            this.RegData.Size = new System.Drawing.Size(632, 390);
            this.RegData.TabIndex = 28;
            // 
            // addr
            // 
            this.addr.FillWeight = 37F;
            this.addr.HeaderText = "";
            this.addr.MaxInputLength = 2;
            this.addr.MinimumWidth = 37;
            this.addr.Name = "addr";
            this.addr.ReadOnly = true;
            this.addr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.addr.Width = 37;
            // 
            // Column00
            // 
            this.Column00.FillWeight = 37F;
            this.Column00.HeaderText = "00";
            this.Column00.MaxInputLength = 2;
            this.Column00.MinimumWidth = 37;
            this.Column00.Name = "Column00";
            this.Column00.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column00.Width = 37;
            // 
            // Column01
            // 
            this.Column01.FillWeight = 37F;
            this.Column01.HeaderText = "01";
            this.Column01.MaxInputLength = 2;
            this.Column01.MinimumWidth = 37;
            this.Column01.Name = "Column01";
            this.Column01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column01.Width = 37;
            // 
            // Column02
            // 
            this.Column02.FillWeight = 37F;
            this.Column02.HeaderText = "02";
            this.Column02.MaxInputLength = 2;
            this.Column02.MinimumWidth = 37;
            this.Column02.Name = "Column02";
            this.Column02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column02.Width = 37;
            // 
            // Column03
            // 
            this.Column03.FillWeight = 37F;
            this.Column03.HeaderText = "03";
            this.Column03.MaxInputLength = 2;
            this.Column03.MinimumWidth = 37;
            this.Column03.Name = "Column03";
            this.Column03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column03.Width = 37;
            // 
            // Column04
            // 
            this.Column04.FillWeight = 37F;
            this.Column04.HeaderText = "04";
            this.Column04.MaxInputLength = 2;
            this.Column04.MinimumWidth = 37;
            this.Column04.Name = "Column04";
            this.Column04.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column04.Width = 37;
            // 
            // Column05
            // 
            this.Column05.FillWeight = 37F;
            this.Column05.HeaderText = "05";
            this.Column05.MaxInputLength = 2;
            this.Column05.MinimumWidth = 37;
            this.Column05.Name = "Column05";
            this.Column05.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column05.Width = 37;
            // 
            // Column06
            // 
            this.Column06.FillWeight = 37F;
            this.Column06.HeaderText = "06";
            this.Column06.MaxInputLength = 2;
            this.Column06.MinimumWidth = 37;
            this.Column06.Name = "Column06";
            this.Column06.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column06.Width = 37;
            // 
            // Column07
            // 
            this.Column07.FillWeight = 37F;
            this.Column07.HeaderText = "07";
            this.Column07.MaxInputLength = 2;
            this.Column07.MinimumWidth = 37;
            this.Column07.Name = "Column07";
            this.Column07.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column07.Width = 37;
            // 
            // Column08
            // 
            this.Column08.FillWeight = 37F;
            this.Column08.HeaderText = "08";
            this.Column08.MaxInputLength = 2;
            this.Column08.MinimumWidth = 37;
            this.Column08.Name = "Column08";
            this.Column08.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column08.Width = 37;
            // 
            // Column09
            // 
            this.Column09.FillWeight = 37F;
            this.Column09.HeaderText = "09";
            this.Column09.MaxInputLength = 2;
            this.Column09.MinimumWidth = 37;
            this.Column09.Name = "Column09";
            this.Column09.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column09.Width = 37;
            // 
            // Column0A
            // 
            this.Column0A.FillWeight = 37F;
            this.Column0A.HeaderText = "0A";
            this.Column0A.MaxInputLength = 2;
            this.Column0A.MinimumWidth = 37;
            this.Column0A.Name = "Column0A";
            this.Column0A.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0A.Width = 37;
            // 
            // Column0B
            // 
            this.Column0B.FillWeight = 37F;
            this.Column0B.HeaderText = "0B";
            this.Column0B.MaxInputLength = 2;
            this.Column0B.MinimumWidth = 37;
            this.Column0B.Name = "Column0B";
            this.Column0B.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0B.Width = 37;
            // 
            // Column0C
            // 
            this.Column0C.FillWeight = 37F;
            this.Column0C.HeaderText = "0C";
            this.Column0C.MaxInputLength = 2;
            this.Column0C.MinimumWidth = 37;
            this.Column0C.Name = "Column0C";
            this.Column0C.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0C.Width = 37;
            // 
            // Column0D
            // 
            this.Column0D.FillWeight = 37F;
            this.Column0D.HeaderText = "0D";
            this.Column0D.MaxInputLength = 2;
            this.Column0D.MinimumWidth = 37;
            this.Column0D.Name = "Column0D";
            this.Column0D.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0D.Width = 37;
            // 
            // Column0E
            // 
            this.Column0E.FillWeight = 37F;
            this.Column0E.HeaderText = "0E";
            this.Column0E.MaxInputLength = 2;
            this.Column0E.MinimumWidth = 37;
            this.Column0E.Name = "Column0E";
            this.Column0E.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0E.Width = 37;
            // 
            // Column0F
            // 
            this.Column0F.FillWeight = 37F;
            this.Column0F.HeaderText = "0F";
            this.Column0F.MaxInputLength = 2;
            this.Column0F.MinimumWidth = 37;
            this.Column0F.Name = "Column0F";
            this.Column0F.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column0F.Width = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.cbSerial);
            this.groupBox2.Location = new System.Drawing.Point(541, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 129);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Port Select";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Location = new System.Drawing.Point(90, 39);
            this.textBox1.MaxLength = 2;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(119, 31);
            this.textBox1.TabIndex = 34;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(21, 81);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(79, 25);
            this.radioButton2.TabIndex = 29;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "UART";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(21, 45);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(68, 25);
            this.radioButton1.TabIndex = 28;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "USB";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // ReadAll
            // 
            this.ReadAll.Location = new System.Drawing.Point(660, 161);
            this.ReadAll.Name = "ReadAll";
            this.ReadAll.Size = new System.Drawing.Size(124, 36);
            this.ReadAll.TabIndex = 24;
            this.ReadAll.Text = "Read All";
            this.ReadAll.UseVisualStyleBackColor = true;
            this.ReadAll.Click += new System.EventHandler(this.ReadAll_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(660, 220);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 36);
            this.button2.TabIndex = 25;
            this.button2.Text = "Write All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(660, 510);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(124, 36);
            this.button6.TabIndex = 29;
            this.button6.Text = "Save";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(660, 452);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(124, 36);
            this.button5.TabIndex = 28;
            this.button5.Text = "Load";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(660, 278);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(124, 36);
            this.button11.TabIndex = 30;
            this.button11.Text = "Run Script";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(660, 336);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(124, 36);
            this.button12.TabIndex = 31;
            this.button12.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(660, 394);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(124, 36);
            this.button13.TabIndex = 32;
            this.button13.UseVisualStyleBackColor = true;
            // 
            // I2CTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 561);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.ReadAll);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.RegData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "I2CTool";
            this.Text = "I2CTool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RegData)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox SlaveAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SUBAddress;
        private System.Windows.Forms.TextBox RegValue;
        private System.Windows.Forms.CheckBox cbBIT7;
        private System.Windows.Forms.CheckBox cbBIT6;
        private System.Windows.Forms.CheckBox cbBIT5;
        private System.Windows.Forms.CheckBox cbBIT4;
        private System.Windows.Forms.CheckBox cbBIT3;
        private System.Windows.Forms.CheckBox cbBIT2;
        private System.Windows.Forms.CheckBox cbBIT1;
        private System.Windows.Forms.CheckBox cbBIT0;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.DataGridView RegData;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn addr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column00;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column04;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column05;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column06;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column07;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column08;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column09;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0A;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0B;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0C;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0D;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0E;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column0F;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button ReadAll;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
    }
}

