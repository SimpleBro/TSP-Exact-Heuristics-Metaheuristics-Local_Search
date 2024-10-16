using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using TSP.InitialSolition.InitialAlgorithms;
using TSP.LocalSearch;
using TSP.Metaheuristics.PopulationAlgorithms;
using TSP.LocalSearch.IntraAlgorithms;

namespace TSP
{
    public class IMethod
    {
        readonly public string tspType = typeof(IMethod).Name;

        private void PrintMethodResult(string methodName, string path, int sumSolution, int solutionImproved, double objFun, string elapsedTime, int custCount)
        {
            objFun = Math.Round(objFun, 2);
            Properties.Settings.Default.lastPath = path;
            Properties.Settings.Default.Save();

            GraphInitialisation.SetAlgorithmDataLabel(String.Format("Total solutions = {0}, Solution improved: {1}, Objective function = {2}",
                           sumSolution, solutionImproved, objFun));

            string logOutput = string.Format("Total solutions: {0}\nSolution improved: {1}\nObjective function: {2}", 
                sumSolution, solutionImproved, objFun);
            if (custCount - 1 <= 13)
                logOutput += "\nPath: " + path;
            GUI.EventLog(this.GetType().Name, methodName, "INFO", elapsedTime, logOutput);
        }

        public void Hamilton()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                Hamilton h = new Hamilton();
                h.graph = graph;

                string path = String.Empty;
                int counter = 0;
                foreach (Vertex v in h.ShortestHamiltonCycle())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, h.iterationCount, 
                    h.soulutionCount, h.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void AntColony()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                AntOptimization ant = new AntOptimization();
                ant.graph = graph;
                ant.alpha = 1;
                ant.beta = 5;
                ant.ro = 0.5;

                string path = String.Empty;
                int counter = 0;
                foreach (Vertex v in ant.AntColonyOptimization(100, 100))
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ant.iterationCount,
                    ant.soulutionCount, ant.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void GeneticAlgorithm()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                Genetic genetic = new Genetic();
                genetic.graph = graph;
                genetic.maxIterations = 2000;

                genetic.GenerateChromosomes(50);

                genetic.mutationRate = (int)0.047 * genetic.ChromosomePool.Count;
                genetic.crossingRate = genetic.ChromosomePool.Count;

                string path = String.Empty;
                int counter = 0;
                foreach (Vertex v in genetic.GeneticOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, genetic.iterationCount,
                    genetic.solutionImprovedCount, genetic.ChromosomePool[0].fitness, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void PCGeneticAlgorithm()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                Genetic genetic = new Genetic();
                genetic.graph = graph;
                genetic.maxIterations = 150;
                genetic.InitLocalSearch();

                genetic.GenerateChromosomes(50);

                genetic.mutationRate = (int)0.047 * genetic.ChromosomePool.Count;
                genetic.crossingRate = genetic.ChromosomePool.Count;

                string path = String.Empty;
                int counter = 0;
                foreach (Vertex v in genetic.GeneticOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, genetic.iterationCount,
                    genetic.solutionImprovedCount, genetic.bestSolution.fitness, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void SimpleGeneticAlgorithm()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                SimpleGenetic genetic = new SimpleGenetic();
                genetic.graph = graph;

                genetic.GenerateChromosomes(50);

                genetic.mutationRate = (int)0.047 * genetic.chromosomes.Count;
                genetic.crossingRate = genetic.chromosomes.Count;

                string path = String.Empty;
                int counter = 0;
                foreach (Vertex v in genetic.GeneticOptimization(100))
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, genetic.iterationCount,
                    genetic.solutionImprovedCount, genetic.chromosomes[0].rating, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void NearestNeighborHeuristic()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                NearestNeighbor nn = new NearestNeighbor();
                nn.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // NNO - Returns the final Path list
                foreach (Vertex v in nn.NearestNeighbourOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, nn.iterationCount,
                    nn.soulutionCount, nn.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void NearestInsertionHeuristic()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                NearestInsertionHeuristic nih = new NearestInsertionHeuristic();
                nih.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in nih.NearestInsertionHeuristicOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, nih.iterationCount,
                    nih.soulutionCount, nih.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void NearestInsertionHeuristic_V2()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                NearestInsertionHeuristic_V2 nih = new NearestInsertionHeuristic_V2();
                nih.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in nih.NearestInsertionHeuristicOptimization_V2())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, nih.iterationCount,
                    nih.soulutionCount, nih.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }
        public void FarthestInsertionHeuristic()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                FarthestInsertionHeuristic nih = new FarthestInsertionHeuristic();
                nih.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in nih.FarthestInsertionHeuristicOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, nih.iterationCount,
                    nih.soulutionCount, nih.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void CWSavings()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                CWSavings cws = new CWSavings();
                cws.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in cws.CWSavingsOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, cws.iterationCount,
                    cws.soulutionCount, cws.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }
        public void CWSavingsFirstNN()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                CWSavingsFirstNN cws = new CWSavingsFirstNN();
                cws.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in cws.CWSavingsFirstNNOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, cws.iterationCount,
                    cws.soulutionCount, cws.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void IntraRelocate()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                IntraRelocate ir = new IntraRelocate();
                ir.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ir.IntraRelocateOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ir.iterationCount,
                    ir.soulutionCount, ir.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void IntraExchange()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                IntraExchange ie = new IntraExchange();
                ie.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ie.IntraExchangeOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ie.iterationCount,
                    ie.soulutionCount, ie.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void IntraRelocateExchange()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                IntraRelocateExchange ie = new IntraRelocateExchange();
                ie.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ie.IntraRelocateExchangeOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ie.iterationCount,
                    ie.soulutionCount, ie.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void IntraOrOpt2()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                IntraOrOpt2 ie = new IntraOrOpt2();
                ie.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ie.IntraOrOpt2Optimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ie.iterationCount,
                    ie.soulutionCount, ie.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void IntraOrOpt3()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                IntraOrOpt3 ie = new IntraOrOpt3();
                ie.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ie.IntraOrOpt3Optimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ie.iterationCount,
                    ie.soulutionCount, ie.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }

        public void Intra2Opt()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                Intra2Opt ie = new Intra2Opt();
                ie.graph = graph;

                string path = String.Empty;
                int counter = 0;

                // nih - Returns the final Path list
                foreach (Vertex v in ie.Intra2OptOptimization())
                {
                    if (counter == 0)
                        path += v.index;
                    else
                        path += " --> " + v.index;
                    counter++;
                }

                this.PrintMethodResult(MethodBase.GetCurrentMethod().Name, path, ie.iterationCount,
                    ie.soulutionCount, ie.minDistance, sw.Elapsed.TotalSeconds.ToString(), counter);
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
            }
        }
    }
}
