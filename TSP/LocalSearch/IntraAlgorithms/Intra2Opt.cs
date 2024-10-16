using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.LocalSearch.IntraAlgorithms
{
    internal class Intra2Opt
    {
        public Intra2Opt()
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

        public List<Vertex> Intra2OptOptimization()
        {
            this.usedVertices.Clear();
            this.usedVertices = GraphMethods.PathStringToVertexList();
            this.Intra2OptRecurring();

            shortestPath.Clear();
            shortestPath.AddRange(usedVertices);

            minDistance = GraphMethods.PathDistanceCost(shortestPath);

            return shortestPath;
        }

        private void Intra2OptRecurring()
        {
            bool symetricGraph = Properties.Settings.Default.symetricGraph;

            while (true)
            {
                Edge bestRelocationEdge = new Edge() { distance = double.MaxValue, vertex1 = null, vertex2 = null };

                // Check if swaping two edge combinations and the direction of the Path of the swaped sequence provides a better objective function output.
                // Start at i = 1, because 0 is the depot, and end before the last element in the list, the depot.
                for (int i = 1; i < this.usedVertices.Count - 1; i++)
                {
                    Vertex v_i = this.usedVertices[i];

                    // Check the 2Opt cost of replacing the edges (i-1,i) and (j-1,j) with (i-1,j-1) and (i,j)
                    for (int j = 1; j <= this.usedVertices.Count - 1; j++)
                    {
                        Vertex v_j = this.usedVertices[j];

                        // If vertex i == vertex j, skip
                        if (v_i == v_j)
                            continue;

                        // If j == i + 1 then the vertex i and vertex j are two consecutive vertices in the path.
                        // Exchange i to location j (== i + 1) would not change the path vertex sequence as i would still come before i + 1 (j).
                        if (j == i + 1 || i == j + 1)
                            continue;

                        if (this.usedVertices[i - 1] == v_j)
                            continue;

                        // Calculate the intra2OptCost cost as:
                        // d(s) = -d(i-1,i) - d(j-1,j)
                        //        +d(i-1,j-1) +d(i,j)
                        double intra2OptCost = double.MaxValue;

                        // Some Edges may not be connected - skip those error checks
                        try
                        {
                            intra2OptCost =
                            - this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, v_i.index)].distance  // We remove this edge from the path, thats why its negative
                            - this.graph.edges[Tuple.Create(this.usedVertices[j - 1].index, v_j.index)].distance  // Remove this edge
                            + this.graph.edges[Tuple.Create(this.usedVertices[i - 1].index, this.usedVertices[j - 1].index)].distance  // Add this edge
                            + this.graph.edges[Tuple.Create(v_i.index, v_j.index)].distance;  // Add this edge
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        double directionSwitchCost = 0;

                        // If the graph is not symetric, changing the direction of the given path may lead to an increase in the objective function.
                        if (!symetricGraph)
                        {
                            // Fetch a sub-tour from index i to index j-1 in the tour.
                            List<Vertex> subTour = new List<Vertex>();
                            int indexI = this.usedVertices.IndexOf(v_i);
                            int indexJ1 = this.usedVertices.IndexOf(this.usedVertices[j - 1]);
                            int nrOfElements = Math.Abs(indexJ1 - indexI) + 1;
                            int pivotIndex = (indexI > indexJ1) ? indexJ1 : indexI;
                            subTour.AddRange(this.usedVertices.GetRange(pivotIndex, nrOfElements));

                            // Calculate the cost of chaning the direction of the subtour
                            double originalCost = 0, newCost = 0;

                            try
                            {
                                for (int k = 0; k < subTour.Count - 1; k++)
                                {
                                    newCost += this.graph.edges[Tuple.Create(subTour[k].index, subTour[k + 1].index)].distance;
                                }
                                for (int k = subTour.Count - 1; k >= 0; k--)
                                {
                                    originalCost += this.graph.edges[Tuple.Create(subTour[k].index, subTour[k + 1].index)].distance;
                                }

                                directionSwitchCost = originalCost + newCost;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                        // Add the cost of switching the direction of the subtour to the final cost
                        intra2OptCost += directionSwitchCost;

                        // We seek the lowest possible cost.
                        // 0 > intra2OptCost - if intra2OptCost == 0 then there is no change in the objective function cost, we save only when intra2OptCost is less than 0.
                        if (bestRelocationEdge.distance > intra2OptCost && 0 > intra2OptCost)
                        {
                            // Old path order: ... -> i-1 -> i -> ... j-1 -> j -> ...
                            // New path order: ... -> i-1 -> j-1 -> ... -> i -> j -> ...

                            bestRelocationEdge.distance = intra2OptCost;
                            bestRelocationEdge.vertex1 = v_i;  // Swap v1 with v2 : i with j-1
                            bestRelocationEdge.vertex2 = this.usedVertices[j - 1];

                            this.iterationCount++;
                        }
                    }
                }

                // If a better solution was found, execute the exchange in the path list.
                if (bestRelocationEdge.vertex1 != null)
                {
                    int viIndex = this.usedVertices.IndexOf(bestRelocationEdge.vertex1);
                    int j1Index = this.usedVertices.IndexOf(bestRelocationEdge.vertex2);

                    // Swap i with j-1
                    //this.usedVertices[viIndex] = bestRelocationEdge.vertex2;
                    //this.usedVertices[j1Index] = bestRelocationEdge.vertex1;

                    // Reverse the sub-tour between i and j-1
                    List<Vertex> subTour = new List<Vertex>();
                    int nrOfElements = Math.Abs(j1Index - viIndex) + 1;
                    int pivotIndex = (viIndex > j1Index) ? j1Index : viIndex;
                    subTour.AddRange(this.usedVertices.GetRange(pivotIndex, nrOfElements));

                    int subTourCounter = subTour.Count - 1;
                    for (int i = pivotIndex; i <= nrOfElements - 1 + pivotIndex; i++) 
                    {
                        this.usedVertices[i] = subTour[subTourCounter];
                        subTourCounter--;
                    }

                    this.soulutionCount++;
                }
                else
                    break;
            }
        }
    }
}
