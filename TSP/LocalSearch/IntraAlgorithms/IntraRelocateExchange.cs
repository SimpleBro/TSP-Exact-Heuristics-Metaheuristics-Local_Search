using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.LocalSearch.IntraAlgorithms
{
    internal class IntraRelocateExchange
    {
        public IntraRelocateExchange()
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

        public List<Vertex> IntraRelocateExchangeOptimization()
        {
            this.usedVertices.Clear();
            this.usedVertices = GraphMethods.PathStringToVertexList();
            this.IntraRelocateExchangeRecurring();

            shortestPath.Clear();
            shortestPath.AddRange(usedVertices);

            minDistance = GraphMethods.PathDistanceCost(shortestPath);

            return shortestPath;
        }

        private void IntraRelocateExchangeRecurring()
        {
            while (true)
            {
                Edge bestRelocationEdge = new Edge() { distance = double.MaxValue, vertex1 = null, vertex2 = null };

                // For each Vertex in the current Path see if exchanging it with any other vertex would improve the objective function.
                // Start at i = 1, because 0 is the depot, and end before the last element in the list, the depot.
                for (int i = 1; i < this.usedVertices.Count - 2; i++)
                {
                    Vertex v_i = this.usedVertices[i];
                    // Check the exchange cost of switching the vertex i with the j-th location vertex.
                    for (int j = 1; j < this.usedVertices.Count - 1; j++)
                    {
                        Vertex v_j = this.usedVertices[j];

                        // If vertex i == vertex j, skip
                        if (v_i == v_j)
                            continue;

                        // If j == i + 1 then the vertex i and vertex j are two consecutive vertices in the path.
                        // Exchange i to location j (== i + 1) would not change the path vertex sequence as i would still come before i + 1 (j).
                        if (j == i + 1 || i == j + 1)
                            continue;

                        // Calculate the relocation cost as:
                        // d(s) = -d(i-1,i) - d(i,i+1) + d(i-1,i+1) - d(j-1,j) + d(j-1,i) + d(i,j)
                        double operationCost = double.MaxValue;

                        // Some Edges may not be connected - skip those error checks
                        try
                        {
                            operationCost =
                            -this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, v_i.index)].distance  // We remove this edge from the path, thats why its negative
                            - this.graph.edges[Tuple.Create(v_i.index, this.usedVertices[i + 1].index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, this.usedVertices[i + 1].index)].distance  // Add this edge -> positive
                            - this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_j.index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_i.index)].distance  // Add this edge
                            + this.graph.edges[Tuple.Create(v_i.index, v_j.index)].distance;  // Add this edge
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        // We seek the lowest possible cost. i.e. if we relocate vertex i to location j, our cost should be lower than the previous cost.
                        // 0 > relocationCost - if rCost == 0 then there is no change in the objective function cost, we save only when rCost is less than 0.
                        if (bestRelocationEdge.distance > operationCost && 0 > operationCost)
                        {
                            bestRelocationEdge.distance = operationCost;
                            bestRelocationEdge.vertex1 = v_i;  // Relocate v_i vertex
                            bestRelocationEdge.vertex2 = this.usedVertices[j];  // v_i will be connected to vertex j.
                            bestRelocationEdge.insertAfterIndex = j - 1;  // Insert v_i before index j in the path list. i.e. i will go in to j.
                            this.iterationCount++;
                        }

                        // Calculate the Exchange cost as:
                        // d(s) = -d(i-1,i) - d(i,i+1) + d(i-1,j) + d(j,i+1)
                        //        -d(j-1,j) - d(j,j+1) + d(j-1,i) + d(i,j+1)
                        //double exchangeCost = double.MaxValue;

                        // Some Edges may not be connected - skip those error checks
                        try
                        {
                            operationCost =
                            -this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, v_i.index)].distance  // We remove this edge from the path, thats why its negative
                            - this.graph.edges[Tuple.Create(v_i.index, this.usedVertices[i + 1].index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, v_j.index)].distance  // Add this edge
                            + this.graph.edges[Tuple.Create(v_j.index, this.usedVertices[i + 1].index)].distance  // Add this edge
                            - this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_j.index)].distance  // Remove this edge
                            - this.graph.edges[Tuple.Create(v_j.index, this.usedVertices[j + 1].index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_i.index)].distance  // Add this edge
                            + this.graph.edges[Tuple.Create(v_i.index, this.usedVertices[j + 1].index)].distance; // Add this edge
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        // We seek the lowest possible cost. i.e. if we exchange vertex i with location j vertex, our cost should be lower than the previous cost.
                        // 0 > exchangeCost - if eCost == 0 then there is no change in the objective function cost, we save only when eCost is less than 0.
                        if (bestRelocationEdge.distance > operationCost && 0 > operationCost)
                        {
                            bestRelocationEdge.distance = operationCost;
                            bestRelocationEdge.vertex1 = v_i;  // Exchange v_i vertex
                            bestRelocationEdge.vertex2 = v_j;  // with vertex v_j
                            this.iterationCount++;
                            bestRelocationEdge.insertAfterIndex = -100;
                        }
                    }
                }

                // If a better solution was found, execute the exchange in the path list.
                if (bestRelocationEdge.vertex1 != null)
                {
                    if (bestRelocationEdge.insertAfterIndex == -100)
                    {
                        int indexOfV1 = this.usedVertices.IndexOf(bestRelocationEdge.vertex1);
                        int indexOfV2 = this.usedVertices.IndexOf(bestRelocationEdge.vertex2);

                        this.usedVertices[indexOfV1] = bestRelocationEdge.vertex2;
                        this.usedVertices[indexOfV2] = bestRelocationEdge.vertex1;
                    }
                    else
                    {
                        this.usedVertices.Remove(bestRelocationEdge.vertex1);

                        // If we should insert the element at the n-1 location, then indeof will return the first element = depot, not the last element, which is also the depot
                        if (this.usedVertices.IndexOf(bestRelocationEdge.vertex2) != 0)
                            this.usedVertices.Insert(this.usedVertices.IndexOf(bestRelocationEdge.vertex2), bestRelocationEdge.vertex1);
                        else
                            this.usedVertices.Insert(bestRelocationEdge.insertAfterIndex, bestRelocationEdge.vertex1);
                    }

                    this.soulutionCount++;
                }
                else
                    break;
            }
        }
    }
}
