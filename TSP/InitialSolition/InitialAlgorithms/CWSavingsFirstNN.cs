using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.InitialSolition.InitialAlgorithms
{
    internal class CWSavingsFirstNN
    {
        public CWSavingsFirstNN()
        {
            usedVertices = new List<Vertex>();
            shortestPath = new List<Vertex>();
            savingsList = new Dictionary<int, List<Edge>>();
            minDistance = Double.MaxValue;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        List<Vertex> shortestPath;
        Dictionary<int, List<Edge>> savingsList;
        List<Vertex> usedVertices;

        public List<Vertex> CWSavingsFirstNNOptimization()
        {
            this.usedVertices.Clear();

            CWSavingsFirstNNRecurring();
            shortestPath.Clear();
            shortestPath.AddRange(usedVertices);

            return shortestPath;
        }

        private void CWSavingsFirstNNRecurring()
        {
            // For each Depot in the Graph.
            foreach (KeyValuePair<int, Vertex> depot in this.graph.depots)
            {
                // STEP 1 - Create Savings list of all nodes connected to the current Depot.
                if (!this.savingsList.ContainsKey(depot.Key))
                    this.savingsList[depot.Key] = new List<Edge>();

                // For each Neighbor of the current Depot -> We want to create a list of savings that concern only the current Depot.
                foreach (KeyValuePair<Tuple<int, int>, Edge> depotNeighbor in depot.Value.neighbors)
                {
                    // The current Vertex is the neighbor of the current Depot.
                    Vertex vertex = depotNeighbor.Value.vertex2;
                    //if (vertex.isDepot)
                    //    continue;

                    // For each neighbor of a vertex that is connected to the current Depot.
                    foreach (KeyValuePair<Tuple<int, int>, Edge> neighbor in vertex.neighbors)
                    {
                        // Skip if the Edge end node is a depot.
                        //if (neighbor.Value.vertex2.isDepot)
                        //    continue;

                        if (neighbor.Value.vertex2 == depot.Value)
                            continue;

                        // S(i,j) = d(depot,i) + d(j,depot) - d(i,j).
                        double cwCostSaving = this.graph.edges[Tuple.Create(depot.Key, neighbor.Value.vertex1.index)].distance + 
                            this.graph.edges[Tuple.Create(neighbor.Value.vertex2.index, depot.Key)].distance - neighbor.Value.distance;

                        Edge validEdge = new Edge { vertex1 = neighbor.Value.vertex1, vertex2 = neighbor.Value.vertex2, distance = cwCostSaving };
                        this.savingsList[depot.Key].Add(validEdge);
                    }
                }
                // Sort the list by the savings value - Descending.
                this.savingsList[depot.Key] = this.savingsList[depot.Key].OrderByDescending(x => x.distance).ToList();

                //-----------------------------------------------------------
                // STEP 2 - Generate a path from the Depot savings list.

                // The first edge is Edge(Depot, NearestNeighbor of the depot)
                Vertex depotNN = GraphMethods.NearestNeighbor(depot.Value, new List<Vertex>());
                List<Vertex> tempUsedVertices = new List<Vertex> { depot.Value, depotNN };

                Edge lastAddedEdge = this.graph.edges[Tuple.Create(depot.Key, depotNN.index)];

                // Build rest of the path. Note: an existing pair in the path MUST NOT be broken AND IN THIS VERSION we can only add new vertices to the end of the path sequence.
                int counter = 0;
                while(tempUsedVertices.Count() < this.graph.vertices.Count)
                {
                    if (counter > this.graph.vertices.Count)
                        break;
                    counter++;
                    // Find the first occurence of an edge that has the first vertex (v1) == the last vertex in the temp path.
                    // Take the second vertex of the edge (v2) and add it to the path if v2 is not yet present in the temp path.

                    // When a new vertex has been added to the path, rerun the foreach loop as there might be candidate edges at indexes of the saving list that were passed.

                    List<Edge> v1IsLastVertexInPath = this.savingsList[depot.Key].GroupBy(x => x.vertex1).SelectMany(x => x).Where(x => x.vertex1 == tempUsedVertices[tempUsedVertices.Count - 1]).ToList();

                    // The grouped list is sorted by descending value of the savings
                    // Select the first instance that meets the conditions and remove the instance from the savings list

                    foreach (Edge edge in v1IsLastVertexInPath)
                    {
                        if (!tempUsedVertices.Contains(edge.vertex2))
                        {
                            tempUsedVertices.Add(edge.vertex2);

                            this.savingsList[depot.Key].Remove(edge);
                            break;
                        }
                    }
                }           

                // Add the depot as the last element of the path
                tempUsedVertices.Add(depot.Value);

                double tempDistance = GraphMethods.PathDistanceCost(tempUsedVertices);
                if (this.minDistance > tempDistance && tempUsedVertices.Count - 1 == this.graph.vertices.Count)
                {
                    this.minDistance = tempDistance;
                    this.usedVertices.Clear();
                    this.usedVertices.AddRange(tempUsedVertices);

                    this.soulutionCount++;

                    //string a = "";

                    //foreach (var item in this.savingsList[depot.Key])
                    //{
                    //    a += "\n" + item.vertex1.index + "," + item.vertex2.index + " - " + item.distance;
                    //}

                    //MessageBox.Show(a);
                }
                this.iterationCount++;
            }
        }
    }
}
