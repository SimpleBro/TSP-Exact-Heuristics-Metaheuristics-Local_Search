namespace TSP
{
    partial class GUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Lst_InitialSolution = new System.Windows.Forms.ListBox();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Execution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Domain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGV_Log_Output = new System.Windows.Forms.DataGridView();
            this.Btn_Log_Output_Save = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Btn_RunMetaheuristics = new System.Windows.Forms.Button();
            this.Btn_RunLocalSearch = new System.Windows.Forms.Button();
            this.Btn_RunInitialSolution = new System.Windows.Forms.Button();
            this.Lst_Metaheuristics = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Lst_LocalSearch = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Lab_CustomersGraph = new System.Windows.Forms.Label();
            this.graphInitialisation1 = new TSP.GraphInitialisation();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Log_Output)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lst_InitialSolution
            // 
            this.Lst_InitialSolution.FormattingEnabled = true;
            this.Lst_InitialSolution.ItemHeight = 12;
            this.Lst_InitialSolution.Location = new System.Drawing.Point(6, 43);
            this.Lst_InitialSolution.Name = "Lst_InitialSolution";
            this.Lst_InitialSolution.Size = new System.Drawing.Size(207, 112);
            this.Lst_InitialSolution.TabIndex = 1;
            // 
            // Message
            // 
            this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message.DefaultCellStyle = dataGridViewCellStyle6;
            this.Message.HeaderText = "Message";
            this.Message.MinimumWidth = 6;
            this.Message.Name = "Message";
            // 
            // Execution
            // 
            this.Execution.HeaderText = "Execution [s]";
            this.Execution.MinimumWidth = 6;
            this.Execution.Name = "Execution";
            this.Execution.Width = 125;
            // 
            // Status
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.DefaultCellStyle = dataGridViewCellStyle7;
            this.Status.FillWeight = 80F;
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 70;
            // 
            // Method
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Method.DefaultCellStyle = dataGridViewCellStyle8;
            this.Method.HeaderText = "Method";
            this.Method.MinimumWidth = 6;
            this.Method.Name = "Method";
            this.Method.Width = 125;
            // 
            // Domain
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Domain.DefaultCellStyle = dataGridViewCellStyle9;
            this.Domain.HeaderText = "Domain";
            this.Domain.MinimumWidth = 6;
            this.Domain.Name = "Domain";
            this.Domain.Width = 70;
            // 
            // Time
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Time.DefaultCellStyle = dataGridViewCellStyle10;
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 6;
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 80;
            // 
            // DGV_Log_Output
            // 
            this.DGV_Log_Output.AllowUserToAddRows = false;
            this.DGV_Log_Output.AllowUserToDeleteRows = false;
            this.DGV_Log_Output.AllowUserToOrderColumns = true;
            this.DGV_Log_Output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Log_Output.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(82)))));
            this.DGV_Log_Output.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Log_Output.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Domain,
            this.Method,
            this.Status,
            this.Execution,
            this.Message});
            this.DGV_Log_Output.Location = new System.Drawing.Point(705, 5);
            this.DGV_Log_Output.Name = "DGV_Log_Output";
            this.DGV_Log_Output.RowHeadersWidth = 51;
            this.DGV_Log_Output.Size = new System.Drawing.Size(895, 204);
            this.DGV_Log_Output.TabIndex = 25;
            // 
            // Btn_Log_Output_Save
            // 
            this.Btn_Log_Output_Save.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Btn_Log_Output_Save.FlatAppearance.BorderSize = 0;
            this.Btn_Log_Output_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.Btn_Log_Output_Save.Location = new System.Drawing.Point(705, 5);
            this.Btn_Log_Output_Save.Name = "Btn_Log_Output_Save";
            this.Btn_Log_Output_Save.Size = new System.Drawing.Size(53, 22);
            this.Btn_Log_Output_Save.TabIndex = 26;
            this.Btn_Log_Output_Save.Text = "Save";
            this.Btn_Log_Output_Save.UseVisualStyleBackColor = true;
            this.Btn_Log_Output_Save.Click += new System.EventHandler(this.Btn_Log_Output_Save_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Btn_RunMetaheuristics);
            this.groupBox3.Controls.Add(this.Btn_RunLocalSearch);
            this.groupBox3.Controls.Add(this.Btn_RunInitialSolution);
            this.groupBox3.Controls.Add(this.Lst_Metaheuristics);
            this.groupBox3.Controls.Add(this.Btn_Log_Output_Save);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.Lst_LocalSearch);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.Lab_CustomersGraph);
            this.groupBox3.Controls.Add(this.Lst_InitialSolution);
            this.groupBox3.Controls.Add(this.DGV_Log_Output);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 548);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1606, 215);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Runtime Data - Good is good enough";
            // 
            // Btn_RunMetaheuristics
            // 
            this.Btn_RunMetaheuristics.Location = new System.Drawing.Point(464, 173);
            this.Btn_RunMetaheuristics.Name = "Btn_RunMetaheuristics";
            this.Btn_RunMetaheuristics.Size = new System.Drawing.Size(118, 27);
            this.Btn_RunMetaheuristics.TabIndex = 33;
            this.Btn_RunMetaheuristics.Text = "Run Metaheuristics";
            this.Btn_RunMetaheuristics.UseVisualStyleBackColor = true;
            this.Btn_RunMetaheuristics.Click += new System.EventHandler(this.Btn_RunMetaheuristics_Click);
            // 
            // Btn_RunLocalSearch
            // 
            this.Btn_RunLocalSearch.Location = new System.Drawing.Point(239, 173);
            this.Btn_RunLocalSearch.Name = "Btn_RunLocalSearch";
            this.Btn_RunLocalSearch.Size = new System.Drawing.Size(118, 27);
            this.Btn_RunLocalSearch.TabIndex = 32;
            this.Btn_RunLocalSearch.Text = "Run Local Search";
            this.Btn_RunLocalSearch.UseVisualStyleBackColor = true;
            this.Btn_RunLocalSearch.Click += new System.EventHandler(this.Btn_RunLocalSearch_Click);
            // 
            // Btn_RunInitialSolution
            // 
            this.Btn_RunInitialSolution.Location = new System.Drawing.Point(28, 173);
            this.Btn_RunInitialSolution.Name = "Btn_RunInitialSolution";
            this.Btn_RunInitialSolution.Size = new System.Drawing.Size(118, 27);
            this.Btn_RunInitialSolution.TabIndex = 31;
            this.Btn_RunInitialSolution.Text = "Run Initial Solution";
            this.Btn_RunInitialSolution.UseVisualStyleBackColor = true;
            this.Btn_RunInitialSolution.Click += new System.EventHandler(this.Btn_RunInitialSolution_Click);
            // 
            // Lst_Metaheuristics
            // 
            this.Lst_Metaheuristics.FormattingEnabled = true;
            this.Lst_Metaheuristics.ItemHeight = 12;
            this.Lst_Metaheuristics.Location = new System.Drawing.Point(430, 43);
            this.Lst_Metaheuristics.Name = "Lst_Metaheuristics";
            this.Lst_Metaheuristics.Size = new System.Drawing.Size(232, 112);
            this.Lst_Metaheuristics.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(426, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 29;
            this.label2.Text = "Metaheuristics";
            // 
            // Lst_LocalSearch
            // 
            this.Lst_LocalSearch.FormattingEnabled = true;
            this.Lst_LocalSearch.ItemHeight = 12;
            this.Lst_LocalSearch.Location = new System.Drawing.Point(218, 43);
            this.Lst_LocalSearch.Name = "Lst_LocalSearch";
            this.Lst_LocalSearch.Size = new System.Drawing.Size(207, 112);
            this.Lst_LocalSearch.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(214, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 27;
            this.label1.Text = "Local search";
            // 
            // Lab_CustomersGraph
            // 
            this.Lab_CustomersGraph.AutoSize = true;
            this.Lab_CustomersGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_CustomersGraph.Location = new System.Drawing.Point(2, 20);
            this.Lab_CustomersGraph.Name = "Lab_CustomersGraph";
            this.Lab_CustomersGraph.Size = new System.Drawing.Size(109, 17);
            this.Lab_CustomersGraph.TabIndex = 26;
            this.Lab_CustomersGraph.Text = "Initial solution";
            // 
            // graphInitialisation1
            // 
            this.graphInitialisation1.Location = new System.Drawing.Point(0, 0);
            this.graphInitialisation1.Name = "graphInitialisation1";
            this.graphInitialisation1.Size = new System.Drawing.Size(1621, 547);
            this.graphInitialisation1.TabIndex = 28;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1603, 765);
            this.Controls.Add(this.graphInitialisation1);
            this.Controls.Add(this.groupBox3);
            this.Name = "GUI";
            this.Text = "In principio erat TSP, et TSP erat apud Deum, et Deus erat TSP";
            this.Load += new System.EventHandler(this.GUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Log_Output)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox Lst_InitialSolution;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn Execution;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Method;
        private System.Windows.Forms.DataGridViewTextBoxColumn Domain;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridView DGV_Log_Output;
        private System.Windows.Forms.Button Btn_Log_Output_Save;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox Lst_Metaheuristics;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox Lst_LocalSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Lab_CustomersGraph;
        private System.Windows.Forms.Button Btn_RunMetaheuristics;
        private System.Windows.Forms.Button Btn_RunLocalSearch;
        private System.Windows.Forms.Button Btn_RunInitialSolution;
        private GraphInitialisation GUI_GraphInitialisation;
        private GraphInitialisation graphInitialisation1;
    }
}

