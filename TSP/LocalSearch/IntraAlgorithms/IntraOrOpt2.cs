using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.LocalSearch.IntraAlgorithms
{
    internal class IntraOrOpt2
    {
        public IntraOrOpt2()
        {
            usedVertices = new List<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = Double.MaxValue;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        List<Vertex> shortestPath;
        List<Vertex> usedVertices;

        public List<Vertex> IntraOrOpt2Optimization()
        {
            this.usedVertices.Clear();
            this.usedVertices = GraphMethods.PathStringToVertexList();
            this.IntraOrOpt2Recurring();

            shortestPath.Clear();
            shortestPath.AddRange(usedVertices);

            minDistance = GraphMethods.PathDistanceCost(shortestPath);

            return shortestPath;
        }

        private void IntraOrOpt2Recurring()
        {
            while (true)
            {
                Edge bestRelocationEdge = new Edge() { distance = double.MaxValue, vertex1 = null, vertex2 = null };
                Edge bestRelocationEdge_2 = new Edge() { distance = double.MaxValue, vertex1 = null, vertex2 = null };

                // For each Vertex in the current Path see if exchanging it with any other vertex would improve the objective function.
                // Start at i = 1, because 0 is the depot, and end before the last element in the list, the depot.
                // -3 because we have to check the element at index i+2
                for (int i = 1; i < this.usedVertices.Count - 3; i++)
                {
                    Vertex v_i = this.usedVertices[i];
                    // Check the OrOpt2 cost of switching the vertex i and i+1 with the j-th location vertex edge.
                    for (int j = 1; j < this.usedVertices.Count - 1; j++)
                    {
                        Vertex v_j = this.usedVertices[j];

                        // If vertex i == vertex j, skip
                        if (v_i == v_j)
                            continue;

                        // If j == i + 1 then the vertex i and vertex j are two consecutive vertices in the path.
                        // Exchange i to location j (== i + 1) would not change the path vertex sequence as i would still come before i + 1 (j).
                        if (j == i + 1 || i == j + 1 || j == i + 2)
                            continue;

                        // Calculate the OrOpt2 cost as:
                        // d(s) = -d(i-1,i) - d(i+1,i+2) - d(j-1,j)
                        //        +d(i-1,i+2) +d(j-1,i) + d(i+1,j)
                        double orOpt2Cost = double.MaxValue;

                        // Some Edges may not be connected - skip those error checks
                        try
                        {
                            orOpt2Cost =
                            - this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, v_i.index)].distance  // We remove this edge from the path, thats why its negative
                            - this.graph.edges[Tuple.Create(this.usedVertices[i + 1].index, this.usedVertices[i + 2].index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, this.usedVertices[i + 2].index)].distance  // Add this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_i.index)].distance  // Add this edge
                            - this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_j.index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[i + 1].index, v_j.index)].distance; // Add this edge
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        // We seek the lowest possible cost. i.e. if we connect vertex i with location j-1 vertex and i+1 with j, our cost should be lower than the previous cost.
                        // 0 > orOpt2Cost - if orCost == 0 then there is no change in the objective function cost, we save only when orCost is less than 0.
                        if (bestRelocationEdge.distance > orOpt2Cost && 0 > orOpt2Cost)
                        {
                            // New path order: ... -> i-1 -> i+2 -> ... -> j-1 -> i -> i+1 -> j -> ...

                            // First relocation pair
                            bestRelocationEdge.distance = orOpt2Cost;
                            bestRelocationEdge.vertex1 = this.usedVertices[j - 1];
                            bestRelocationEdge.vertex2 = v_i;

                            // Second relocation pair
                            bestRelocationEdge_2.distance = orOpt2Cost;
                            bestRelocationEdge_2.vertex1 = this.usedVertices[i + 1];
                            bestRelocationEdge_2.vertex2 = v_j;

                            this.iterationCount++;
                        }
                    }
                }

                // If a better solution was found, execute the exchange in the path list.
                if (bestRelocationEdge.vertex1 != null)
                {
                    this.usedVertices.Remove(bestRelocationEdge.vertex2);
                    this.usedVertices.Remove(bestRelocationEdge_2.vertex1);

                    this.usedVertices.Insert(this.usedVertices.IndexOf(bestRelocationEdge_2.vertex2), bestRelocationEdge.vertex2);
                    this.usedVertices.Insert(this.usedVertices.IndexOf(bestRelocationEdge_2.vertex2), bestRelocationEdge_2.vertex1);

                    this.soulutionCount++;
                }
                else
                    break;
            }
        }
    }
}
