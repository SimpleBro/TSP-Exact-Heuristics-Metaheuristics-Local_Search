namespace TSP
{
    partial class GraphInitialisation
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GUI_GraphInit = new System.ComponentModel.BackgroundWorker();
            this.Btn_LoadGraph = new System.Windows.Forms.Button();
            this.Btn_RandomGraph = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnImageStore = new System.Windows.Forms.Button();
            this.Btn_RemoveEdges = new System.Windows.Forms.Button();
            this.Lst_TSPLIB = new System.Windows.Forms.ListBox();
            this.ChkBox_SymetricGraph = new System.Windows.Forms.CheckBox();
            this.DGV_VertexLocation = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Lab_ConnectionProbability = new System.Windows.Forms.Label();
            this.Txt_ConnectionProbability = new System.Windows.Forms.TextBox();
            this.ChkBox_FullGraph = new System.Windows.Forms.CheckBox();
            this.DGV_GraphMatrix = new System.Windows.Forms.DataGridView();
            this.Lab_DepotGraph = new System.Windows.Forms.Label();
            this.Txt_DepotGraph = new System.Windows.Forms.TextBox();
            this.Lab_CustomersGraph = new System.Windows.Forms.Label();
            this.Txt_CustomersGraph = new System.Windows.Forms.TextBox();
            this.PctBox_Graph = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Lab_AlgorithmData = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_VertexLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_GraphMatrix)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PctBox_Graph)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_LoadGraph
            // 
            this.Btn_LoadGraph.Location = new System.Drawing.Point(547, 74);
            this.Btn_LoadGraph.Name = "Btn_LoadGraph";
            this.Btn_LoadGraph.Size = new System.Drawing.Size(163, 52);
            this.Btn_LoadGraph.TabIndex = 0;
            this.Btn_LoadGraph.Text = "Load TSPLIB Graph";
            this.Btn_LoadGraph.UseVisualStyleBackColor = true;
            this.Btn_LoadGraph.Click += new System.EventHandler(this.Btn_LoadGraph_Click);
            // 
            // Btn_RandomGraph
            // 
            this.Btn_RandomGraph.Location = new System.Drawing.Point(547, 139);
            this.Btn_RandomGraph.Name = "Btn_RandomGraph";
            this.Btn_RandomGraph.Size = new System.Drawing.Size(163, 51);
            this.Btn_RandomGraph.TabIndex = 1;
            this.Btn_RandomGraph.Text = "Generate Random Graph";
            this.Btn_RandomGraph.UseVisualStyleBackColor = true;
            this.Btn_RandomGraph.Click += new System.EventHandler(this.Btn_RandomGraph_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnImageStore);
            this.groupBox1.Controls.Add(this.Btn_RemoveEdges);
            this.groupBox1.Controls.Add(this.Lst_TSPLIB);
            this.groupBox1.Controls.Add(this.ChkBox_SymetricGraph);
            this.groupBox1.Controls.Add(this.DGV_VertexLocation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Lab_ConnectionProbability);
            this.groupBox1.Controls.Add(this.Txt_ConnectionProbability);
            this.groupBox1.Controls.Add(this.Btn_LoadGraph);
            this.groupBox1.Controls.Add(this.ChkBox_FullGraph);
            this.groupBox1.Controls.Add(this.DGV_GraphMatrix);
            this.groupBox1.Controls.Add(this.Lab_DepotGraph);
            this.groupBox1.Controls.Add(this.Btn_RandomGraph);
            this.groupBox1.Controls.Add(this.Txt_DepotGraph);
            this.groupBox1.Controls.Add(this.Lab_CustomersGraph);
            this.groupBox1.Controls.Add(this.Txt_CustomersGraph);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(747, 540);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graph settings";
            // 
            // btnImageStore
            // 
            this.btnImageStore.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnImageStore.FlatAppearance.BorderSize = 0;
            this.btnImageStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.btnImageStore.Location = new System.Drawing.Point(561, 208);
            this.btnImageStore.Name = "btnImageStore";
            this.btnImageStore.Size = new System.Drawing.Size(135, 43);
            this.btnImageStore.TabIndex = 32;
            this.btnImageStore.Text = "Save image to .eps";
            this.btnImageStore.UseVisualStyleBackColor = true;
            this.btnImageStore.Click += new System.EventHandler(this.btnImageStore_Click);
            // 
            // Btn_RemoveEdges
            // 
            this.Btn_RemoveEdges.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Btn_RemoveEdges.FlatAppearance.BorderSize = 0;
            this.Btn_RemoveEdges.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.Btn_RemoveEdges.Location = new System.Drawing.Point(561, 16);
            this.Btn_RemoveEdges.Name = "Btn_RemoveEdges";
            this.Btn_RemoveEdges.Size = new System.Drawing.Size(135, 43);
            this.Btn_RemoveEdges.TabIndex = 28;
            this.Btn_RemoveEdges.Text = "Clear graph";
            this.Btn_RemoveEdges.UseVisualStyleBackColor = true;
            this.Btn_RemoveEdges.Click += new System.EventHandler(this.Btn_RemoveEdges_Click);
            // 
            // Lst_TSPLIB
            // 
            this.Lst_TSPLIB.FormattingEnabled = true;
            this.Lst_TSPLIB.ItemHeight = 12;
            this.Lst_TSPLIB.Location = new System.Drawing.Point(329, 15);
            this.Lst_TSPLIB.MultiColumn = true;
            this.Lst_TSPLIB.Name = "Lst_TSPLIB";
            this.Lst_TSPLIB.Size = new System.Drawing.Size(186, 208);
            this.Lst_TSPLIB.TabIndex = 31;
            // 
            // ChkBox_SymetricGraph
            // 
            this.ChkBox_SymetricGraph.AutoSize = true;
            this.ChkBox_SymetricGraph.Checked = true;
            this.ChkBox_SymetricGraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBox_SymetricGraph.Location = new System.Drawing.Point(104, 54);
            this.ChkBox_SymetricGraph.Name = "ChkBox_SymetricGraph";
            this.ChkBox_SymetricGraph.Size = new System.Drawing.Size(102, 16);
            this.ChkBox_SymetricGraph.TabIndex = 21;
            this.ChkBox_SymetricGraph.Text = "Symetric Graph";
            this.ChkBox_SymetricGraph.UseVisualStyleBackColor = true;
            // 
            // DGV_VertexLocation
            // 
            this.DGV_VertexLocation.AllowUserToAddRows = false;
            this.DGV_VertexLocation.AllowUserToDeleteRows = false;
            this.DGV_VertexLocation.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGV_VertexLocation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_VertexLocation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.X,
            this.Y});
            this.DGV_VertexLocation.Location = new System.Drawing.Point(3, 275);
            this.DGV_VertexLocation.Name = "DGV_VertexLocation";
            this.DGV_VertexLocation.ReadOnly = true;
            this.DGV_VertexLocation.RowHeadersWidth = 51;
            this.DGV_VertexLocation.Size = new System.Drawing.Size(219, 258);
            this.DGV_VertexLocation.TabIndex = 19;
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Index.HeaderText = "ID";
            this.Index.MinimumWidth = 6;
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            // 
            // X
            // 
            this.X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.X.HeaderText = "X";
            this.X.MinimumWidth = 6;
            this.X.Name = "X";
            this.X.ReadOnly = true;
            // 
            // Y
            // 
            this.Y.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Y.HeaderText = "Y";
            this.Y.MinimumWidth = 6;
            this.Y.Name = "Y";
            this.Y.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "Vertex location";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(225, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(311, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Distance matrix - Euclidian distance (Col = start, Row = end)";
            // 
            // Lab_ConnectionProbability
            // 
            this.Lab_ConnectionProbability.AutoSize = true;
            this.Lab_ConnectionProbability.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_ConnectionProbability.Location = new System.Drawing.Point(49, 146);
            this.Lab_ConnectionProbability.Name = "Lab_ConnectionProbability";
            this.Lab_ConnectionProbability.Size = new System.Drawing.Size(118, 12);
            this.Lab_ConnectionProbability.TabIndex = 18;
            this.Lab_ConnectionProbability.Text = "Connection probability";
            // 
            // Txt_ConnectionProbability
            // 
            this.Txt_ConnectionProbability.Location = new System.Drawing.Point(202, 143);
            this.Txt_ConnectionProbability.Name = "Txt_ConnectionProbability";
            this.Txt_ConnectionProbability.Size = new System.Drawing.Size(63, 17);
            this.Txt_ConnectionProbability.TabIndex = 17;
            this.Txt_ConnectionProbability.Text = "40";
            // 
            // ChkBox_FullGraph
            // 
            this.ChkBox_FullGraph.AutoSize = true;
            this.ChkBox_FullGraph.Checked = true;
            this.ChkBox_FullGraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBox_FullGraph.Location = new System.Drawing.Point(104, 29);
            this.ChkBox_FullGraph.Name = "ChkBox_FullGraph";
            this.ChkBox_FullGraph.Size = new System.Drawing.Size(76, 16);
            this.ChkBox_FullGraph.TabIndex = 14;
            this.ChkBox_FullGraph.Text = "Full Graph";
            this.ChkBox_FullGraph.UseVisualStyleBackColor = true;
            // 
            // DGV_GraphMatrix
            // 
            this.DGV_GraphMatrix.AllowUserToAddRows = false;
            this.DGV_GraphMatrix.AllowUserToDeleteRows = false;
            this.DGV_GraphMatrix.BackgroundColor = System.Drawing.SystemColors.Control;
            this.DGV_GraphMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_GraphMatrix.Location = new System.Drawing.Point(228, 275);
            this.DGV_GraphMatrix.MultiSelect = false;
            this.DGV_GraphMatrix.Name = "DGV_GraphMatrix";
            this.DGV_GraphMatrix.RowHeadersWidth = 51;
            this.DGV_GraphMatrix.Size = new System.Drawing.Size(496, 258);
            this.DGV_GraphMatrix.TabIndex = 8;
            this.DGV_GraphMatrix.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_GraphMatrix_CellValueChanged);
            // 
            // Lab_DepotGraph
            // 
            this.Lab_DepotGraph.AutoSize = true;
            this.Lab_DepotGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_DepotGraph.Location = new System.Drawing.Point(49, 120);
            this.Lab_DepotGraph.Name = "Lab_DepotGraph";
            this.Lab_DepotGraph.Size = new System.Drawing.Size(96, 12);
            this.Lab_DepotGraph.TabIndex = 7;
            this.Lab_DepotGraph.Text = "Number of Depots";
            // 
            // Txt_DepotGraph
            // 
            this.Txt_DepotGraph.Location = new System.Drawing.Point(202, 117);
            this.Txt_DepotGraph.Name = "Txt_DepotGraph";
            this.Txt_DepotGraph.Size = new System.Drawing.Size(63, 17);
            this.Txt_DepotGraph.TabIndex = 6;
            this.Txt_DepotGraph.Text = "1";
            // 
            // Lab_CustomersGraph
            // 
            this.Lab_CustomersGraph.AutoSize = true;
            this.Lab_CustomersGraph.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_CustomersGraph.Location = new System.Drawing.Point(49, 94);
            this.Lab_CustomersGraph.Name = "Lab_CustomersGraph";
            this.Lab_CustomersGraph.Size = new System.Drawing.Size(115, 12);
            this.Lab_CustomersGraph.TabIndex = 3;
            this.Lab_CustomersGraph.Text = "Number of Customers";
            // 
            // Txt_CustomersGraph
            // 
            this.Txt_CustomersGraph.Location = new System.Drawing.Point(202, 91);
            this.Txt_CustomersGraph.Name = "Txt_CustomersGraph";
            this.Txt_CustomersGraph.Size = new System.Drawing.Size(63, 17);
            this.Txt_CustomersGraph.TabIndex = 0;
            this.Txt_CustomersGraph.Text = "9";
            // 
            // PctBox_Graph
            // 
            this.PctBox_Graph.Location = new System.Drawing.Point(6, 18);
            this.PctBox_Graph.Name = "PctBox_Graph";
            this.PctBox_Graph.Size = new System.Drawing.Size(850, 518);
            this.PctBox_Graph.TabIndex = 1;
            this.PctBox_Graph.TabStop = false;
            this.PctBox_Graph.Paint += new System.Windows.Forms.PaintEventHandler(this.PctBox_Graph_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Lab_AlgorithmData);
            this.groupBox2.Controls.Add(this.PctBox_Graph);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(756, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(862, 543);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Graph View";
            // 
            // Lab_AlgorithmData
            // 
            this.Lab_AlgorithmData.AutoSize = true;
            this.Lab_AlgorithmData.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lab_AlgorithmData.Location = new System.Drawing.Point(249, 18);
            this.Lab_AlgorithmData.Name = "Lab_AlgorithmData";
            this.Lab_AlgorithmData.Size = new System.Drawing.Size(331, 12);
            this.Lab_AlgorithmData.TabIndex = 29;
            this.Lab_AlgorithmData.Text = "Iteration count = {0}, Solution count: {1}, Objective function = {2}";
            // 
            // GraphInitialisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GraphInitialisation";
            this.Size = new System.Drawing.Size(1621, 547);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_VertexLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_GraphMatrix)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PctBox_Graph)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker GUI_GraphInit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label Lab_DepotGraph;
        private System.Windows.Forms.TextBox Txt_DepotGraph;
        private System.Windows.Forms.Label Lab_CustomersGraph;
        private System.Windows.Forms.TextBox Txt_CustomersGraph;
        private System.Windows.Forms.Button Btn_RandomGraph;
        private System.Windows.Forms.Button Btn_LoadGraph;
        private System.Windows.Forms.DataGridView DGV_GraphMatrix;
        private System.Windows.Forms.CheckBox ChkBox_FullGraph;
        private System.Windows.Forms.Label Lab_ConnectionProbability;
        private System.Windows.Forms.TextBox Txt_ConnectionProbability;
        private System.Windows.Forms.DataGridView DGV_VertexLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox PctBox_Graph;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ChkBox_SymetricGraph;
        public System.Windows.Forms.Button Btn_RemoveEdges;
        private System.Windows.Forms.Label Lab_AlgorithmData;
        private System.Windows.Forms.ListBox Lst_TSPLIB;
        public System.Windows.Forms.Button btnImageStore;
    }
}
