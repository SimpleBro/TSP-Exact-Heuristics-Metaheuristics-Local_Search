using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TSP.InitialSolition.InitialAlgorithms
{
    internal class CWSavings
    {
        public CWSavings()
        {
            usedVertices = new List<Vertex>();
            shortestPath = new List<Vertex>();
            savingsList = new Dictionary<int, List<Edge>>();
            minDistance = Double.MaxValue;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        double distance = 0;
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        List<Vertex> shortestPath;
        Dictionary<int, List<Edge>> savingsList;
        List<Vertex> usedVertices;

        public List<Vertex> CWSavingsOptimization()
        {
            this.usedVertices.Clear();

            CWSavingsRecurring();
            shortestPath.Clear();
            shortestPath.AddRange(usedVertices);
            this.soulutionCount++;
            this.iterationCount++;

            return shortestPath;
        }

        /// <summary>
        /// Generate a CW savings list from the starting depot.
        /// </summary>
        public List<Edge> CWSavingsList(Vertex depot)
        {
            List<Edge> cwList = new List<Edge>();

            // For each Neighbor of the current Depot -> We want to create a list of savings that concern only the current Depot.
            foreach (KeyValuePair<Tuple<int, int>, Edge> depotNeighbor in depot.neighbors)
            {
                // The current Vertex is the neighbor of the current Depot.
                Vertex vertex = depotNeighbor.Value.vertex2;

                // For each neighbor of a vertex that is connected to the current Depot.
                foreach (KeyValuePair<Tuple<int, int>, Edge> neighbor in vertex.neighbors)
                {
                    //// Skip if the Edge end node is a depot.
                    if (neighbor.Value.vertex2 == depot)
                        continue;

                    // S(i,j) = d(depot,i) + d(j,depot) - d(i,j).
                    double cwCostSaving = this.graph.edges[Tuple.Create(depot.index, neighbor.Value.vertex1.index)].distance +
                        this.graph.edges[Tuple.Create(neighbor.Value.vertex2.index, depot.index)].distance - neighbor.Value.distance;

                    Edge validEdge = new Edge { vertex1 = neighbor.Value.vertex1, vertex2 = neighbor.Value.vertex2, distance = cwCostSaving };
                    cwList.Add(validEdge);
                }
            }

            // Sort the list by the savings value - Descending.
            return cwList.OrderByDescending(x => x.distance).ToList();
        }

        private void CWSavingsRecurring()
        {
            // For each Depot in the Graph.
            foreach (KeyValuePair<int, Vertex> depot in this.graph.depots)
            {
                // STEP 1 - Create Savings list of all nodes connected to the current Depot.
                if (!this.savingsList.ContainsKey(depot.Key))
                    this.savingsList[depot.Key] = new List<Edge>();

                this.savingsList[depot.Key] = this.CWSavingsList(depot.Value);

                //-----------------------------------------------------------
                // STEP 2 - Generate a path from the Depot savings list.

                // Add the biggest saving edge to the path.
                List<Vertex> tempUsedVertices = new List<Vertex> { this.savingsList[depot.Key][0].vertex1, this.savingsList[depot.Key][0].vertex2 };

                Edge lastAddedEdge = this.savingsList[depot.Key][0];
                bool v1ToFirstVertex = false, v2OutOfLastVertex = false;
                int counter = 0;

                // Loop because we want to rerun the foreach loop if some connections were not established because of the ordering and all customers were not visited
                while (tempUsedVertices.Count() < this.graph.vertices.Count - 1)
                {
                    if (counter > this.graph.vertices.Count)
                        break;
                    counter++;

                    // Build rest of the path. Note: an existing pair in the path MUST NOT be broken, only edges to the left or the right of the path can be added.
                    foreach (Edge edge in this.savingsList[depot.Key])
                    {
                        Vertex v1 = edge.vertex1;  // Start vertex of the edge.
                        Vertex v2 = edge.vertex2;  // End vertex of the edge.

                        bool v1InTour = tempUsedVertices.Contains(v1), v2InTour = tempUsedVertices.Contains(v2);

                        if (v1InTour && v2InTour)
                            continue;

                        // One or both vertices of the edge are not yet present in the path (tour)

                        // If the savings value is the same as the previous added edge savings value, check if the last added combination should be overwritten by this edge.
                        // If multiple edges have the same value, we should take the last of them that complies with the conditions.

                        if (edge.distance == lastAddedEdge.distance)
                        {
                            // If the current edge has the same v2 as the lastAddedEdge, replace v1 of the lastAddedEdge with the v1 of the current edge.
                            // v2 was the first element of the path before the last vertex was added.
                            // i.e. v1ToFirstVertex == true

                            if (edge.vertex2 == lastAddedEdge.vertex2)
                            {
                                tempUsedVertices[0] = edge.vertex1;
                                lastAddedEdge = edge;
                            }

                            // Else, current edge has the v1 same as the last added edge.
                            // v1 was the last element of the path before the last vertex was added.
                            // i.e. v2OutOfLastVertex == true

                            else if (edge.vertex1 == lastAddedEdge.vertex1)
                            {
                                tempUsedVertices[tempUsedVertices.Count - 1] = edge.vertex2;
                                lastAddedEdge = edge;
                            }
                        }
                        else
                        {
                            // The edge must either have the first vertex in the path as the v2 in the edge: x -> first element : (go to the first vertex in the temp path).
                            // Or the last vertex in the path as the v1 in the edge: last element -> x : (lead out of the last vertex in the temp path).
                            // e.g. temp path [2, 5], temp edge [1, 3] -> 3 != 2 and 1 != 5 -> we cant connect the edge to the temp path, skip.

                            bool tempV1ToFirstVertex = v2 == tempUsedVertices.First();
                            bool tempV2OutOfLastVertex = v1 == tempUsedVertices.Last();

                            // If the vertices of the current edge can not be connected to the first or last elements of the path, skip
                            if (!tempV1ToFirstVertex && !tempV2OutOfLastVertex)
                                continue;

                            if (tempV1ToFirstVertex)
                            {
                                if (v1.index == tempUsedVertices[0].index)
                                    continue;

                                tempUsedVertices.Insert(0, v1);
                                v1ToFirstVertex = tempV1ToFirstVertex;
                                v2OutOfLastVertex = tempV2OutOfLastVertex;
                            }
                            else if (tempV2OutOfLastVertex)
                            {
                                if (v2.index == tempUsedVertices[tempUsedVertices.Count - 1].index)
                                    continue;

                                tempUsedVertices.Add(v2);
                                v1ToFirstVertex = tempV1ToFirstVertex;
                                v2OutOfLastVertex = tempV2OutOfLastVertex;
                            }
                        }
                    }
                }

                // Add the depot to as the first and last element of the path
                tempUsedVertices.Insert(0, depot.Value);
                tempUsedVertices.Add(depot.Value);

                double tempDistance = GraphMethods.PathDistanceCost(tempUsedVertices);
                if (this.minDistance > tempDistance)
                {
                    this.minDistance = tempDistance;
                    this.usedVertices.Clear();
                    this.usedVertices.AddRange(tempUsedVertices);

                    //string a = "";

                    //foreach (var item in this.savingsList[depot.Key])
                    //{
                    //    a += "\n" + item.vertex1.index + "," + item.vertex2.index + " - " + item.distance;
                    //}

                    //MessageBox.Show(a);
                }
            }
        }
    }
}
