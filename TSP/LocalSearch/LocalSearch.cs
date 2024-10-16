using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.LocalSearch
{
    internal class InterOperators
    {
        readonly IMethod tsp = new IMethod();
        ///// <summary>
        ///// Run TSP Hamilton Brute Force on full graph.
        ///// </summary>
        //public void InterRelocate()
        //{
        //    this.tsp.Hamilton();
        //}
        ///// <summary>
        ///// Run TSP Hamilton Brute Force on full graph.
        ///// </summary>
        //public void InterExchange()
        //{
        //    tsp.Hamilton();
        //}
        ///// <summary>
        ///// Run TSP Hamilton Brute Force on full graph.
        ///// </summary>
        //public void InterCrossExchange()
        //{
        //    this.tsp.Hamilton();
        //}
        ///// <summary>
        ///// Run TSP Hamilton Brute Force on full graph.
        ///// </summary>
        //public void InterTwoOptStart()
        //{
        //    this.tsp.Hamilton();
        //}
    }
    internal class IntraOperators
    {
        readonly IMethod tsp = new IMethod();
        /// <summary>
        /// Run Intra Relocate Operator on the current solution.
        /// </summary>
        public void IntraRelocate()
        {
            tsp.IntraRelocate();
        }
        /// <summary>
        /// Run Intra Exchange Operator on the current solution.
        /// </summary>
        public void IntraExchange()
        {
            tsp.IntraExchange();
        }
        /// <summary>
        /// Run combination of Intra Relocate and Intra Exchange that occures in the same iteration.
        /// </summary>
        public void IntraRelocateExchange()
        {
            tsp.IntraRelocateExchange();
        }
        /// <summary>
        /// Run Intra OrOpt 2 operator.
        /// </summary>
        public void IntraOrOpt2()
        {
            tsp.IntraOrOpt2();
        }
        /// <summary>
        /// Run Intra OrOpt 3 operator.
        /// </summary>
        public void IntraOrOpt3()
        {
            tsp.IntraOrOpt3();
        }
        /// <summary>
        /// Run Intra2Opt operator.
        /// </summary>
        public void Intra2Opt()
        {
            tsp.Intra2Opt();
        }
        /// <summary>
        /// Run TSP Hamilton Brute Force on full graph.
        /// </summary>
        public void IntraStationInOut_Not_Working()
        {
            return;
        }
    }
}
