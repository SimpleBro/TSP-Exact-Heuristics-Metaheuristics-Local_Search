using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TSP.InitialSolition;
using TSP.Metaheuristics;
using TSP.Miscellaneous;

namespace TSP
{
    public partial class GUI : Form
    {
        public static GUI mainGui;
        readonly public string guiType;
        public GUI()
        {
            InitializeComponent();

            Properties.Settings.Default.Upgrade();

            mainGui = this;
            guiType = typeof(GUI).Name;

            this.DGV_Log_Output.Columns[this.DGV_Log_Output.ColumnCount - 1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            this.DGV_Log_Output.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            try
            {
                // Get all non-static public methods of class Test
                this.Lst_InitialSolution.Items.AddRange(MiscGui.GetInitialSolutionMethods().ToArray());
                this.Lst_LocalSearch.Items.AddRange(MiscGui.GetLocalSearchInterMethods().ToArray());
                this.Lst_LocalSearch.Items.AddRange(MiscGui.GetLocalSearchIntraMethods().ToArray());
                this.Lst_Metaheuristics.Items.AddRange(MiscGui.GetMetaheuristicsPopulationBasedMethods().ToArray());
                this.Lst_Metaheuristics.Items.AddRange(MiscGui.GetMetaheuristicsMethods().ToArray());
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// This method uses reflection to create an instance of a class "InitialSolution" and invoke a method on the instance.
        /// </summary>
        private void Btn_RunInitialSolution_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string targetMethod in this.Lst_InitialSolution.SelectedItems)
                {
                    // Get the constructor and create an instance of Class
                    Type classType = (typeof(InitialSolution));
                    ConstructorInfo classConstructor = classType.GetConstructor(Type.EmptyTypes);
                    object classObject = classConstructor.Invoke(new object[] { });

                    // Get the class method and invoke
                    MethodInfo classMethod = classType.GetMethod(targetMethod);
                    object magicValue = classMethod.Invoke(classObject, new object[] { });
                }
                mainGui = this;

                GraphInitialisation.IInvalidateGraphPaint();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// This method uses reflection to create an instance of a class "IntraOperators" and invoke a method on the instance.
        /// </summary>
        private void Btn_RunLocalSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> interMethods = MiscGui.GetLocalSearchInterMethods();
                List<string> intraMethods = MiscGui.GetLocalSearchIntraMethods();
                foreach (string targetMethod in this.Lst_LocalSearch.SelectedItems)
                {
                    // Get the constructor and create an instance of Class
                    Type classType = (typeof(LocalSearch.InterOperators));
                    if (intraMethods.Contains(targetMethod))
                        classType= (typeof(LocalSearch.IntraOperators));

                    ConstructorInfo classConstructor = classType.GetConstructor(Type.EmptyTypes);
                    object classObject = classConstructor.Invoke(new object[] { });

                    // Get the class method and invoke
                    MethodInfo classMethod = classType.GetMethod(targetMethod);
                    object magicValue = classMethod.Invoke(classObject, new object[] { });
                }
                mainGui = this;

                GraphInitialisation.IInvalidateGraphPaint();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// This method uses reflection to create an instance of a class "Metaheuristics"|"Metaheuristics" and invoke a method on the instance.
        /// </summary>
        private void Btn_RunMetaheuristics_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> pBMethods = MiscGui.GetMetaheuristicsPopulationBasedMethods();
                List<string> metaMethods = MiscGui.GetMetaheuristicsMethods();
                foreach (string targetMethod in this.Lst_Metaheuristics.SelectedItems)
                {
                    // Get the constructor and create an instance of Class
                    Type classType = (typeof(Metaheuristics.Metaheuristics));
                    if (pBMethods.Contains(targetMethod))
                        classType = (typeof(Metaheuristics.PopulationBased));

                    ConstructorInfo classConstructor = classType.GetConstructor(Type.EmptyTypes);
                    object classObject = classConstructor.Invoke(new object[] { });

                    // Get the class method and invoke
                    MethodInfo classMethod = classType.GetMethod(targetMethod);
                    object magicValue = classMethod.Invoke(classObject, new object[] { });
                }
                mainGui = this;

                GraphInitialisation.IInvalidateGraphPaint();
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        ///  Create Event log and store it in the main GUI logger datagridview object.
        /// </summary>
        public static void EventLog(string domain, string method, string status, string execution, string message, bool messagebox = false)
        {
            try
            {
                if (messagebox)
                    MessageBox.Show(message);

                ILogger temp_log = new ILogger(domain, method, status, execution, message);

                string[] row = new string[] { temp_log.time, temp_log.domain, temp_log.method, temp_log.status, temp_log.execution, temp_log.message };
                mainGui.DGV_Log_Output.Rows.Add(row);
                mainGui.DGV_Log_Output.FirstDisplayedScrollingRowIndex = mainGui.DGV_Log_Output.RowCount - 1;

                //if (status == "ERROR")
                //    Properties.Settings.Default.lastPath = String.Empty;
                //Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        /// <summary>
        /// Store the provided Event log to a .log file. Format: ~/DayOfYear_hour_min_sec.log
        /// </summary>
        public void SaveLogToFile()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                List<string> logData = new List<string>();

                foreach (System.Windows.Forms.DataGridViewRow row in this.DGV_Log_Output.Rows)
                {
                    string temp_row = row.Cells[0].Value.ToString() + " | " + row.Cells[1].Value.ToString() + " | " +
                        row.Cells[2].Value.ToString() + " | " + row.Cells[3].Value.ToString() + " | " + 
                        row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString();

                    logData.Add(temp_row);
                }

                string final_path = MiscGui.SaveLogToFile(logData);
                if (final_path != String.Empty)
                {
                    sw.Stop();
                    GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, 
                        "INFO", sw.Elapsed.TotalSeconds.ToString(), "Log trace saved to: " + final_path);
                }
            }
            catch (Exception ex)
            {
                GUI.EventLog(ConstructorInfo.ConstructorName, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        private void Btn_Log_Output_Save_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to save the Event Log to a file?", "Save Event Log", MessageBoxButtons.YesNo);
            
            if (dr == DialogResult.Yes)
                this.SaveLogToFile();
        }
    }
}
