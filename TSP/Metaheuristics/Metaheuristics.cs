using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.Metaheuristics
{
    internal class Metaheuristics
    {
        readonly IMethod tsp = new IMethod();
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void SimulatedAnneling_Not_Working()
        {
            return;
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void TabuSearch_Not_Working()
        {
            return;
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void VariableNeighborhoodSearch_Not_Working()
        {
            return;
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void IteratedLocalSearch_Not_Working()
        {
            return;
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void AdaptiveLargeNeighborhoodSearch_Not_Working()
        {
            return;
        }
    }

    internal class PopulationBased : Metaheuristics
    {
        readonly IMethod tsp = new IMethod();
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void SimpleGeneticAlgorithm()
        {
            this.tsp.SimpleGeneticAlgorithm();
        }
        ///// <summary>
        ///// Run TSP Hamilton Brute Force on full graph.
        ///// </summary>
        //public void GeneticAlgorithm()
        //{
        //    this.tsp.GeneticAlgorithm();
        //}
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void PCGeneticAlgorithm()
        {
            this.tsp.PCGeneticAlgorithm();
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void AntColony()
        {
            this.tsp.AntColony();
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void ParticleSwarmOptimization_Not_Working()
        {
            return;
        }
    }
}
