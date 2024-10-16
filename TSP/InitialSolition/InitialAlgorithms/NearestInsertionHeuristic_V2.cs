using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TSP.InitialSolition.InitialAlgorithms
{
    internal class NearestInsertionHeuristic_V2
    {
        public NearestInsertionHeuristic_V2()
        {
            usedVertices = new List<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = 0;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        double distance = 0;
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        List<Vertex> shortestPath;
        Vertex startVertex;
        Vertex nextVertex;
        List<Vertex> usedVertices;

        public List<Vertex> NearestInsertionHeuristicOptimization_V2()
        {
            foreach (KeyValuePair<int, Vertex> v in graph.vertices)
            {
                // startVertex + nextVertex = Initial Tour
                this.usedVertices.Clear();
                startVertex = v.Value;
                nextVertex = TSP.GraphMethods.NearestNeighbor(startVertex, this.usedVertices);

                if (nextVertex.index == -1)
                    continue;

                usedVertices.Add(startVertex);
                usedVertices.Add(nextVertex);

                NearestInsertionHeuristicRecurring_V2();
                if (this.usedVertices.Count - 1 == this.graph.vertices.Count)
                    distance = GraphMethods.PathDistanceCost(this.usedVertices);
                else
                    distance = Double.MaxValue;

                if (minDistance == 0 || distance < minDistance)
                {
                    minDistance = distance;
                    shortestPath.Clear();
                    shortestPath.AddRange(usedVertices);
                    this.soulutionCount++;
                    this.distance = 0;
                }
                this.iterationCount++;
            }

            return shortestPath;
        }

        /// <summary>
        /// A
        /// </summary>
        private bool NearestInsertionHeuristicRecurring_V2()
        {
            try
            {
                Dictionary<Tuple<int, int>, Edge> loopValidNeighbor = new Dictionary<Tuple<int, int>, Edge>();

                // 1) If graph is not Full, this will select only vertices that are connected to the current loop vertices and are not yet part of the current loop
                // 2) Select Vertex that is not in the current loop and is connected to AT LEAST 2 vertices of the current loop
                loopValidNeighbor = this.usedVertices.SelectMany(x => x.neighbors).Where(x => !this.usedVertices.Contains(x.Value.vertex2))
                    .ToDictionary(x => x.Key, x => x.Value)
                    .GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1).SelectMany(x => x).ToDictionary(x => x.Key, x => x.Value);

                Edge bestInsertEdge = null;
                int insertAfterIndex = 0;

                // NIH_V2 does not care if out of all possible insertions the best one lies in the beggining of the graph, V2 only checks best solution that connects to the last vertex of the loop
                int lastVertexID = this.usedVertices[this.usedVertices.Count - 1].index;
                int lVIDUsedVertices = this.usedVertices.IndexOf(this.usedVertices[this.usedVertices.Count - 1]);

                //1) Get all neighbors of the lastVertex that are not in the current solution
                //2) Get only the edges from 1) with its start vertex present in the current solution
                List<int> lastVertexNeighbors = loopValidNeighbor.Where(x => x.Value.vertex1.index == lastVertexID).Select(x => x.Value.vertex2.index).ToList();
                foreach (var vertexGroup in loopValidNeighbor.Where(x => lastVertexNeighbors.Contains(x.Value.vertex2.index)).GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1))
                {
                    List<int> tempGroup = vertexGroup.Select(x => x.Key.Item1).ToList();
                    double keyEdgeCost = this.graph.edges[Tuple.Create(tempGroup[0], tempGroup[1])].distance;

                    // insertCost is the sum of the triangle: e.g. d(v1, vX) + d(vX, v2) - d(v1, v2)
                    double insertCost = vertexGroup.Sum(x => x.Value.distance) - keyEdgeCost;

                    if (bestInsertEdge == null || bestInsertEdge.distance > insertCost)
                    {
                        insertAfterIndex = lVIDUsedVertices - 1;
                        bestInsertEdge = new Edge { vertex1 = this.graph.vertices[tempGroup[0]], vertex2 = this.graph.vertices[vertexGroup.Key], distance = insertCost };
                    }
                }

                if (bestInsertEdge != null)
                {
                    // Append the best Insert Vortex to the appropriate location
                    this.usedVertices.Insert(insertAfterIndex + 1, bestInsertEdge.vertex2);
                    // Recall the Method
                    return this.NearestInsertionHeuristicRecurring_V2();
                }

                if (this.usedVertices.Count == this.graph.vertices.Count)
                {
                    // Connect last vertex to the depot (startVertex)
                    this.usedVertices.Add(startVertex);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                GUI.EventLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return false;
            }
        }
    }
}
