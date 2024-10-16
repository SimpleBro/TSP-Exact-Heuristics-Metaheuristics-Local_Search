using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TSP.InitialSolition;

namespace TSP.Miscellaneous
{
    internal class MiscGui
    {
        /// <summary>
        /// Returns a string list of all non-static public methods of class InitialSolution.
        /// </summary>
        public static List<string> GetInitialSolutionMethods()
        {
            List<string> initialSolutionMethods = new List<string>();

            Type myType = (typeof(InitialSolution));
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in myArrayMethodInfo)
            {
                initialSolutionMethods.Add(item.Name);
            }

            return initialSolutionMethods;
        }
        /// <summary>
        /// Returns a string list of all non-static public methods of class LocalSearch.InterOperators.
        /// </summary>
        public static List<string> GetLocalSearchInterMethods()
        {
            List<string> localSearchMethods = new List<string>();

            Type myType = (typeof(LocalSearch.InterOperators));
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in myArrayMethodInfo)
            {
                localSearchMethods.Add(item.Name);
            }

            return localSearchMethods;
        }
        /// <summary>
        /// Returns a string list of all non-static public methods of class LocalSearch.InterOperators.
        /// </summary>
        public static List<string> GetLocalSearchIntraMethods()
        {
            List<string> localSearchMethods = new List<string>();

            Type myType = (typeof(LocalSearch.IntraOperators));
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in myArrayMethodInfo)
            {
                localSearchMethods.Add(item.Name);
            }

            return localSearchMethods;
        }
        /// <summary>
        /// Returns a string list of all non-static public methods of class Metaheuristics.
        /// </summary>
        public static List<string> GetMetaheuristicsMethods()
        {
            List<string> metaheuristicsMethods = new List<string>();

            Type myType = (typeof(Metaheuristics.Metaheuristics));
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in myArrayMethodInfo)
            {
                metaheuristicsMethods.Add(item.Name);
            }

            return metaheuristicsMethods;
        }

        /// <summary>
        /// Returns a string list of all non-static public methods of class Metaheuristics.PopulationBased.
        /// </summary>
        public static List<string> GetMetaheuristicsPopulationBasedMethods()
        {
            List<string> metaheuristicsMethods = new List<string>();

            Type myType = (typeof(Metaheuristics.PopulationBased));
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo item in myArrayMethodInfo)
            {
                metaheuristicsMethods.Add(item.Name);
            }

            return metaheuristicsMethods;
        }

        /// <summary>
        /// Returns a string list of all .tsp TSPLIB files in folder ~/TSPLIBFiles/.
        /// </summary>
        public static List<string> GetTSPLIBFiles()
        {
            string[] fileNames = Directory.GetFiles("TSPLIBFiles", "*.tsp", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace("TSPLIBFiles\\", "");
                fileNames[i] = fileNames[i].Replace(".tsp", "");
            }

            return fileNames.ToList();
        }

        /// <summary>
        /// Open the File Browser Dialog and return the absolute path of the selected file.
        /// </summary>
        public static string FileBrowser()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.Title = "Select the target .txt file.";

                while (!dialog.FileName.EndsWith(".txt"))
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        return dialog.FileName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                GUI.EventLog("Misc", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Store the provided Event log to a .log file. Format: ~/DayOfYear_hour_min_sec.log
        /// </summary>
        public static string SaveLogToFile(List<string> eventLog)
        {
            try
            {
                if (eventLog.Count == 0)
                    return String.Empty;

                string finalPath = String.Empty;
                string parentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

                if (!Directory.Exists(parentDirectory + @"\Log_Output"))
                    Directory.CreateDirectory(parentDirectory + @"\Log_Output");

                DateTime fileDate = DateTime.Now;
                finalPath = fileDate.Day.ToString() + "_" + fileDate.Month.ToString() + "_" + fileDate.Year.ToString() + "_"
                    + fileDate.Hour.ToString() + "_" + fileDate.Minute.ToString() + "_" +
                    Math.Round(Convert.ToDouble(fileDate.Second), 2) + ".log";
                finalPath = parentDirectory + @"\Log_Output\" + finalPath;

                StreamWriter sw = new StreamWriter(finalPath);

                foreach (string log_event in eventLog)
                {
                    sw.WriteLine(log_event);
                }

                sw.Flush();
                sw.Close();

                return finalPath;
            }
            catch (Exception ex)
            {
                GUI.EventLog("Misc", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return String.Empty;
            }
        }
    }
}
