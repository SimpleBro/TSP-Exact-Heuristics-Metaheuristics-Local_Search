using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TSP.InitialSolition;
using TSP.Miscellaneous;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using ImageMagick;

namespace TSP
{
    public partial class GraphInitialisation : UserControl
    {
        public static GraphInitialisation IGraphInit;
        private Graph graph;
        private bool removeEdges;

        public GraphInitialisation()
        {
            InitializeComponent();
            this.Lst_TSPLIB.MultiColumn = true;
            this.Lst_TSPLIB.ColumnWidth = 60;
            try
            {
                this.Lst_TSPLIB.Items.AddRange(MiscGui.GetTSPLIBFiles().ToArray());
            }
            catch (Exception)
            {
                this.Lst_TSPLIB.Text = "TSP Library files are currently not available";
            }

            this.DGV_GraphMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV_GraphMatrix.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.DGV_VertexLocation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV_VertexLocation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.graph = new Graph();
            IGraphInit = this;

            Properties.Settings.Default.graphWidth = this.PctBox_Graph.Size.Width;
            Properties.Settings.Default.graphHeight = this.PctBox_Graph.Size.Height;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Get Graph object.
        /// </summary>
        public static Graph IGetGraph()
        {
            return IGraphInit.graph;
        }

        /// <summary>
        /// After running a method, post the result data in the data label box above the solution PictureBox Graph.
        /// </summary>
        public static void SetAlgorithmDataLabel(string dataInput)
        {
            IGraphInit.Lab_AlgorithmData.Text = dataInput;
        }

        /// <summary>
        /// Re-Invoke the PictureBox Graph.
        /// </summary>
        public static void IInvalidateGraphPaint()
        {
            IGraphInit.PctBox_Graph.Invalidate();
            IGraphInit.PctBox_Graph.Refresh();
        }

        /// <summary>
        /// Scale the geo location of the set of points in the graph to fit the dimensions of the picturebox.
        /// </summary>
        public void ScaleGeoLocationToPictureBox()
        {
            try
            {
                // Original coordinates.
                List<double> xCoords = this.graph.vertices.Select(x => x.Value.geoLoc.latX).ToList();
                List<double> yCoords = this.graph.vertices.Select(x => x.Value.geoLoc.longY).ToList();

                // Size of the original coordinates.
                double originalWidth = xCoords.Max() - xCoords.Min();
                double originalHeight = yCoords.Max() - yCoords.Min();

                // Size of the picture box.
                double pictureBoxWidth = Properties.Settings.Default.graphWidth - 100;
                double pictureBoxHeight = Properties.Settings.Default.graphHeight - 100;

                // Calculate the scale factor.
                double scaleX = (double)pictureBoxWidth / originalWidth;
                double scaleY = (double)pictureBoxHeight / originalHeight;

                // Scale the coordinates.
                List<double> scaledXCoords = xCoords.Select(x => Math.Max((double)((x - xCoords.Min()) * scaleX), 30)).ToList();
                List<double> scaledYCoords = yCoords.Select(y => Math.Max((double)((y - yCoords.Min()) * scaleY), 30)).ToList();

                for (int i = 0; i < scaledXCoords.Count; i++)
                {
                    GeoLoc scaledLoc = new GeoLoc
                    {
                        latX = scaledXCoords[i],
                        longY = scaledYCoords[i]
                    };

                    graph.vertices[i].pctBoxScaledGeoLoc = scaledLoc;
                }
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        // Store the Graph object edge distances in the GUI data matrix.
        private void FillGraphDataMatrix()
        {
            try
            {
                // Distance matrix.
                double[,] graphMatrix = TSP.GraphMethods.GraphDistanceMatrix(this.graph);

                DataTable dtGraph = new DataTable();
                dtGraph.Columns.Add("ID", typeof(int));
                for (int i = 0; i < graphMatrix.GetLength(1); i++)
                {
                    dtGraph.Columns.Add((i).ToString());
                }
                this.DGV_GraphMatrix.DataSource = dtGraph;

                DataRow drGraph;
                for (int i = 0; i < graphMatrix.GetLength(0); i++)
                {
                    drGraph = dtGraph.NewRow();
                    drGraph[0] = i;
                    for (int j = 0; j < graphMatrix.GetLength(1); j++)
                    {
                        drGraph[j + 1] = graphMatrix[i, j];
                    }
                    dtGraph.Rows.Add(drGraph);

                    this.DGV_GraphMatrix.Columns[i + 1].DefaultCellStyle.ForeColor = Color.Black;
                }

                for (int i = 0; i < Properties.Settings.Default.depotCount; i++)
                {
                    this.DGV_GraphMatrix.Columns[i + 1].DefaultCellStyle.ForeColor = Color.Red;
                }

                this.DGV_VertexLocation.Rows.Clear();

                // Location matrix.
                foreach (KeyValuePair<int, Vertex> vertex in this.graph.vertices)
                {
                    this.DGV_VertexLocation.Rows.Add(new string[] { vertex.Value.index.ToString(), vertex.Value.geoLoc.latX.ToString(), vertex.Value.geoLoc.longY.ToString() });
                }

                IGraphInit = this;

                this.PctBox_Graph.Invalidate();
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// Initialize properties at program load.
        /// </summary>
        public void InitProperties()
        {
            try
            {
                Properties.Settings.Default.fullGraph = this.ChkBox_FullGraph.Checked;
                Properties.Settings.Default.symetricGraph = this.ChkBox_SymetricGraph.Checked;
                Properties.Settings.Default.connectioProbability = Convert.ToDouble(this.Txt_ConnectionProbability.Text.ToString());
                Properties.Settings.Default.pheromoneValue = Convert.ToDouble(0.001);
                Properties.Settings.Default.vehicleCount = Convert.ToInt32(1);
                Properties.Settings.Default.depotCount = Convert.ToInt32(this.Txt_DepotGraph.Text.ToString());
                Properties.Settings.Default.customerCount = Convert.ToInt32(this.Txt_CustomersGraph.Text.ToString());
                Properties.Settings.Default.lastPath = string.Empty;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void Btn_RandomGraph_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitProperties();
                this.graph = TSP.GraphMethods.CreateRandomGraph();

                this.FillGraphDataMatrix();
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void PctBox_Graph_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // Draw all edges of the graph.
                foreach (Edge item in this.graph.edges.Values)
                {
                    Pen p = new Pen(Color.FromArgb(80, Color.LightGray));

                    Point p1 = new Point((int)item.vertex1.pctBoxScaledGeoLoc.latX, (int)item.vertex1.pctBoxScaledGeoLoc.longY);
                    Rectangle r1 = new Rectangle(p1, new Size(18, 18));
                    Point p2 = new Point((int)item.vertex2.pctBoxScaledGeoLoc.latX, (int)item.vertex2.pctBoxScaledGeoLoc.longY);
                    Rectangle r2 = new Rectangle(p2, new Size(18, 18));

                    // Rectangle center.
                    p1 = new Point(r1.Left + r1.Width / 2, r1.Top + r1.Height / 2);
                    p2 = new Point(r2.Left + r2.Width / 2, r2.Top + r2.Height / 2);

                    e.Graphics.DrawLine(p, p1, p2);
                }

                // Draw final tour (solution).
                if (!this.removeEdges)
                {
                    if (Properties.Settings.Default.lastPath != String.Empty)
                    {
                        string[] separatingStrings = { " --> " };
                        string[] path = Properties.Settings.Default.lastPath.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < path.Length - 1; i++)
                        {
                            int graphVertex = Convert.ToInt32(path[i]);
                            int nextVertex = Convert.ToInt32(path[i + 1]);

                            if (this.graph.vertices[graphVertex].neighbors.ContainsKey(Tuple.Create(graphVertex, nextVertex)))
                            {
                                AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5);
                                Pen p = new Pen(Brushes.Black);
                                p.CustomEndCap = bigArrow;

                                Edge tempEdge = this.graph.vertices[graphVertex].neighbors[Tuple.Create(graphVertex, nextVertex)];

                                Point p1 = new Point((int)tempEdge.vertex1.pctBoxScaledGeoLoc.latX, (int)tempEdge.vertex1.pctBoxScaledGeoLoc.longY);
                                Rectangle r1 = new Rectangle(p1, new Size(18, 18));
                                Point p2 = new Point((int)tempEdge.vertex2.pctBoxScaledGeoLoc.latX, (int)tempEdge.vertex2.pctBoxScaledGeoLoc.longY);
                                Rectangle r2 = new Rectangle(p2, new Size(18, 18));

                                // Rectangle center.
                                p1 = new Point(r1.Left + r1.Width / 2, r1.Top + r1.Height / 2);
                                p2 = new Point(r2.Left + r2.Width / 2, r2.Top + r2.Height / 2);

                                // Line has to end at the intersection of the elipse and the line.
                                double radius = 10;
                                double dx = p2.X - p1.X;
                                double dy = p2.Y - p1.Y;
                                double length = Math.Sqrt(dx * dx + dy * dy);
                                if (length > 0)
                                {
                                    dx /= length;
                                    dy /= length;
                                }
                                dx *= length - radius;
                                dy *= length - radius;

                                int x3 = (int)(p1.X + dx);
                                int y3 = (int)(p1.Y + dy);

                                p1 = new Point((int)(p2.X - dx), (int)(p2.Y - dy));
                                p2 = new Point(x3, y3);

                                e.Graphics.DrawLine(p, p1, p2);
                            }
                        }
                    }
                }
                this.removeEdges = false;

                // Draw locations (customers) of the graph.
                foreach (KeyValuePair<int, Vertex> vertex in this.graph.vertices)
                {
                    Point pLoc = new Point((int)vertex.Value.pctBoxScaledGeoLoc.latX, (int)vertex.Value.pctBoxScaledGeoLoc.longY);
                    Rectangle rLoc = new Rectangle(pLoc, new Size(18, 18));
                    e.Graphics.FillEllipse(Brushes.White, rLoc);

                    if (vertex.Value.index >= 10)
                        e.Graphics.DrawString(vertex.Value.index.ToString(), this.Font, Brushes.Black, new PointF(pLoc.X + 1, pLoc.Y + 2));
                    else
                        e.Graphics.DrawString(vertex.Value.index.ToString(), this.Font, Brushes.Black, new PointF(pLoc.X + 4, pLoc.Y + 2));

                    if (vertex.Value.index < graph.depotCount)
                    {
                        e.Graphics.DrawEllipse(new Pen(Brushes.Red), rLoc);
                    }
                    else
                    {
                        e.Graphics.DrawEllipse(new Pen(Brushes.Black), rLoc);
                    }
                }
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// Load TSPLIB .tsp file.
        /// </summary>
        public void LoadGraphFromFile()
        {
            try
            {
                if (this.graph == null)
                    return;
                else
                {
                    this.FillGraphDataMatrix();
                }
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void Btn_LoadGraph_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string targetFile in this.Lst_TSPLIB.SelectedItems)
                {
                    this.graph = TSPLIB.ReadTSPFile(targetFile);
                    this.ScaleGeoLocationToPictureBox();
                }

                Properties.Settings.Default.fullGraph = this.graph.fullGraph;
                Properties.Settings.Default.symetricGraph = this.graph.symetricGraph;
                Properties.Settings.Default.connectioProbability = 100.0;
                Properties.Settings.Default.pheromoneValue = Convert.ToDouble(0.001);
                Properties.Settings.Default.vehicleCount = 1;
                Properties.Settings.Default.depotCount = 1;
                Properties.Settings.Default.customerCount = this.graph.vertices.Count;
                Properties.Settings.Default.lastPath = string.Empty;
                Properties.Settings.Default.Save();

                GraphInitialisation.IInvalidateGraphPaint();
                Properties.Settings.Default.Save();

                IGraphInit = this;

                this.LoadGraphFromFile();
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void Btn_RemoveEdges_Click(object sender, EventArgs e)
        {
            try
            {
                this.removeEdges = true;
                this.PctBox_Graph.Invalidate();
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// User manually changed a value in the data matrix, update the parent Graph object.
        /// </summary>
        private void DGV_GraphMatrix_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Get the new value of the cell.
                double newValue = Convert.ToDouble(this.DGV_GraphMatrix.CurrentCell.Value);

                // Get the row and column indices of the cell.
                var row = this.DGV_GraphMatrix.CurrentCell.RowIndex;
                var col = this.DGV_GraphMatrix.CurrentCell.ColumnIndex - 1;  // -1 as the first column (0) is ID.

                Tuple<int, int> key = Tuple.Create(col, row);

                if (!this.graph.edges.ContainsKey(Tuple.Create(row, col)))
                {
                    Edge edge = new Edge
                    {
                        distance = newValue,
                        pheromoneValue = Properties.Settings.Default.pheromoneValue,
                        vertex1 = graph.vertices[row],
                        vertex2 = graph.vertices[col]
                    };
                    this.graph.edges[key] = edge;
                }

                // Update the corresponding element in the dictionary.
                this.graph.edges[key].distance = newValue;

                // If -1 remove the v2 (col) from the neighbour list of v1 (row)
                if (newValue == -1)
                {
                    if (this.graph.vertices[row].neighbors.ContainsKey(key))
                    {
                        this.graph.vertices[row].neighbors.Remove(key);
                    }
                }
                else
                {
                    if (!this.graph.vertices[row].neighbors.ContainsKey(key))
                    {
                        this.graph.vertices[row].neighbors[key] = this.graph.edges[key];
                    }
                }
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void btnImageStore_Click(object sender, EventArgs e)
        {
            // Create a temporary file path to save the bitmap
            string tempFilePath = Path.GetTempFileName();

            // Save the bitmap to the temporary file path
            using (Bitmap bitmap = new Bitmap(this.PctBox_Graph.Width, this.PctBox_Graph.Height))
            {
                this.PctBox_Graph.DrawToBitmap(bitmap, this.PctBox_Graph.ClientRectangle);
                bitmap.Save(tempFilePath, ImageFormat.Bmp);
            }

            // Load the temporary file into a MagickImage
            using (MagickImage image = new MagickImage(tempFilePath))
            {
                // Set the output format to EPS.
                image.Format = MagickFormat.Eps;

                // Specify the output file path.
                string outputPath = "slika.eps";

                // Save the image in EPS format.
                image.Write(outputPath);
            }

            // Delete the temporary file
            File.Delete(tempFilePath);
        }
    }
}