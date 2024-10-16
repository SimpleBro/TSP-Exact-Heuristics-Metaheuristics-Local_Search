using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.InitialSolition.InitialAlgorithms
{
    internal class NearestInsertionHeuristic
    {
        public NearestInsertionHeuristic()
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

        public List<Vertex> NearestInsertionHeuristicOptimization()
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

                NearestInsertionHeuristicRecurring();
                distance = GraphMethods.PathDistanceCost(this.usedVertices);

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

        private bool NearestInsertionHeuristicRecurring()
        {
            Dictionary<Tuple<int, int>, Edge> loopValidNeighbor = new Dictionary<Tuple<int, int>, Edge>();

            // 1) If graph is not Full, this will select only vertices that are connected to the current loop vertices and are not yet part of the current loop
            // 2) Select Vertex that is not in the current loop and is connected to AT LEAST 2 vertices of the current loop
            loopValidNeighbor = this.usedVertices.SelectMany(x => x.neighbors).Where(x => !this.usedVertices.Contains(x.Value.vertex2))
                .ToDictionary(x => x.Key, x => x.Value)
                .GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1).SelectMany(x => x).ToDictionary(x => x.Key, x => x.Value);

            Edge bestInsertEdge = null;
            int insertAfterIndex = 0;
            int lastElement = (this.usedVertices.Count > 2) ? 2 : 1;

            // For each consecutive pair of edges present in the current loop; e.g. pair(0,1) or pair(3,4): (i,j) -> Edge
            // Perhaps, out of all the currently possible insertions, the best is e.g. ((0,2)+(1,2)) - (0,1) neighbor - Thats why we check all solutions each recursion
            for (int i = 0; i < this.usedVertices.Count - lastElement; i++)
            {
                int v1ID = this.usedVertices[i].index;
                int v2ID = this.usedVertices[i + 1].index;

                Tuple<int, int> keyEdge = Tuple.Create(v1ID, v2ID);
                double keyEdgeCost = this.graph.edges[keyEdge].distance;

                //1) Get all Neighbors that are valid for the current keyEdge (v1ID, v2ID)
                //2) Each recursion, the returned linq result may differ if a vertex has been added to the loop
                foreach (var vertexGroup in loopValidNeighbor.Where(x => x.Key.Item1 == v1ID || x.Key.Item1 == v2ID).GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1).SelectMany(x => x)
                    .ToDictionary(x => x.Key, x => x.Value).GroupBy(x => x.Key.Item2))
                {
                    // insertCost is the sum of the triangle: e.g. d(v1, vX) + d(vX, v2) - d(v1, v2)
                    // e.g. 2 nodes in path = [1 2 3], possible inserts = [2-4-3], [1-7-3]:
                    // 2-4-3 cost = d(2,4) + d(4,3) - d(2,3) = 15, 1-7-3 cost = d(1,7) + d(7,3) - d(1,3) = 12 + 9 - 10 = 11  -- full SYMMETRIC graph example
                    // 11 is lower than 15 so we insert the cheaper option. new loop [1 2 3 7]
                    double insertCost = vertexGroup.Sum(x => x.Value.distance) - keyEdgeCost;
                    
                    if (bestInsertEdge == null || bestInsertEdge.distance > insertCost)
                    {
                        insertAfterIndex = i;
                        bestInsertEdge = new Edge { vertex1 = this.graph.vertices[v1ID], vertex2 = this.graph.vertices[vertexGroup.Key], distance = insertCost };
                    }
                }
            }

            if (bestInsertEdge != null)
            {
                // Append the best Insert Vortex to the appropriate location
                this.usedVertices.Insert(insertAfterIndex + 1, bestInsertEdge.vertex2);
                // Recall the Method
                return this.NearestInsertionHeuristicRecurring();
            }

            if (this.usedVertices.Count == this.graph.vertices.Count)
            {
                // Connect last vertex to the depot (startVertex)
                this.usedVertices.Add(startVertex);
                return true;
            }

            return false;
        }
    }
}
