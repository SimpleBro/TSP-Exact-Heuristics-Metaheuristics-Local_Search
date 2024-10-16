using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.InitialSolition
{
    internal class InitialSolution
    {
        readonly IMethod tsp = new IMethod();
        /// <summary>
        /// Run TSP Hamilton Brute Force.
        /// </summary>
        public void BruteForce()
        {
            this.tsp.Hamilton();
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void Sweep_Not_Working()
        {
            return;
        }
        /// <summary>
        /// Run TSP Clarke and Wright Savings Algorithm.
        /// </summary>
        public void CWSavings()
        {
            this.tsp.CWSavings();
        }
        ///// <summary>
        ///// Run TSP Clarke and Wright Savings Algorithm where the first edge is the nearest neighbor of the depot.
        ///// </summary>
        //public void CWSavingsFirstNN()
        //{
        //    this.tsp.CWSavingsFirstNN();
        //}
        /// <summary>
        /// Run TSP NNH.
        /// </summary>
        public void NearestNeighborHeuristic()
        {
            this.tsp.NearestNeighborHeuristic();
        }
        /// <summary>
        /// Run TSP NIH where an insertion can be done at any vertices pair in the tour.
        /// </summary>
        public void NearestInsertionHeuristic()
        {
            this.tsp.NearestInsertionHeuristic();
        }
        ///// <summary>
        ///// Run TSP NIH where an insertion can be done only on the last vertex in the tour.
        ///// </summary>
        //public void NearestInsertionHeuristic_V2()
        //{
        //    this.tsp.NearestInsertionHeuristic_V2();
        //}
        /// <summary>
        /// Run TSP NIH variant: Farthest Insertion.
        /// </summary>
        public void FarthestInsertionHeuristic()
        {
            this.tsp.FarthestInsertionHeuristic();
        }
    }
}
