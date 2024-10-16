using System;
using System.Collections.Generic;
using System.Linq;
using TSP.Miscellaneous;

namespace TSP.InitialSolition.InitialAlgorithms
{
    internal class FarthestInsertionHeuristic
    {
        public FarthestInsertionHeuristic()
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

        public List<Vertex> FarthestInsertionHeuristicOptimization()
        {
            // startVertex + nextVertex = Initial Tour
            this.usedVertices.Clear();
            List<Vertex> initialPair = GraphMethods.FarthestVetricesPairInGraph();
            startVertex = initialPair[0];
            nextVertex = initialPair[1];

            usedVertices.Add(startVertex);
            usedVertices.Add(nextVertex);

            FarthestInsertionHeuristicRecurring();
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

            return shortestPath;
        }

        private bool FarthestInsertionHeuristicRecurring()
        {
            Edge farthestInsertEdge = null;
            Edge closestInsertEdge = null;
            int lastElement = (this.usedVertices.Count > 2) ? 2 : 1;

            Dictionary<Tuple<int, int>, Edge> loopValidNeighbor = new Dictionary<Tuple<int, int>, Edge>();
            // 1) If graph is not Full, this will select only vertices that are connected to the current loop vertices and are not yet part of the current loop
            // 2) Select Vertex that is not in the current loop and is connected to AT LEAST 2 vertices of the current loop
            loopValidNeighbor = this.usedVertices.SelectMany(x => x.neighbors).Where(x => !this.usedVertices.Contains(x.Value.vertex2))
                .ToDictionary(x => x.Key, x => x.Value)
                .GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1).SelectMany(x => x).ToDictionary(x => x.Key, x => x.Value);

            // For each consecutive pair of edges present in the current loop (tour); e.g. pair(0,1) or pair(3,4): (i,j) -> Edge
            for (int i = 0; i < this.usedVertices.Count; i++)
            {
                int v1ID = this.usedVertices[i].index;
                int v2ID = (i == this.usedVertices.Count - 1) ? this.usedVertices[0].index : this.usedVertices[i + 1].index;

                Tuple<int, int> keyEdge = Tuple.Create(v1ID, v2ID);
                double keyEdgeCost = 0;

                double x1 = this.graph.vertices[v1ID].geoLoc.latX;
                double y1 = this.graph.vertices[v1ID].geoLoc.longY;
                double x2 = this.graph.vertices[v2ID].geoLoc.latX;
                double y2 = this.graph.vertices[v2ID].geoLoc.longY;

                if (this.graph.edges.ContainsKey(keyEdge))
                    keyEdgeCost = graph.edges[keyEdge].distance;
                else
                    keyEdgeCost = MiscMath.EuclidianDistance(x1, y1, x2, y2);

                //1) Get all Neighbors that are valid for the current keyEdge (v1ID, v2ID)
                //2) Each recursion, the returned linq result may differ if a vertex has been added to the loop
                foreach (var vertexGroup in loopValidNeighbor.Where(x => x.Key.Item1 == v1ID || x.Key.Item1 == v2ID).GroupBy(x => x.Key.Item2).Where(x => x.Count() > 1).SelectMany(x => x)
                    .ToDictionary(x => x.Key, x => x.Value).GroupBy(x => x.Key.Item2))
                {
                    double insertCost = 0;
                    double groupX = this.graph.vertices[vertexGroup.Key].geoLoc.latX;
                    double groupY = this.graph.vertices[vertexGroup.Key].geoLoc.longY;

                    foreach (Vertex v in this.usedVertices)
                    {
                        insertCost += MiscMath.EuclidianDistance(groupX, groupY, v.geoLoc.latX, v.geoLoc.longY);
                    }

                    // Farthest point will be added to the tour
                    if (farthestInsertEdge == null || insertCost > farthestInsertEdge.distance)
                    {
                        farthestInsertEdge = new Edge { vertex1 = graph.vertices[v1ID], vertex2 = graph.vertices[vertexGroup.Key], distance = insertCost };
                    }
                }

                if (farthestInsertEdge == null)
                    continue;
              
                double x3 = farthestInsertEdge.vertex2.geoLoc.latX;
                double y3 = farthestInsertEdge.vertex2.geoLoc.longY;

                double a = MiscMath.EuclidianDistance(x1, y1, x3, y3);
                double b = MiscMath.EuclidianDistance(x2, y2, x3, y3);
                double c = keyEdgeCost;
                double farthestVertexHeightFromLine = MiscMath.HeightOfCFromHypotenuse(a, b, c);

                // TODO: Use DistanceFromPointToLine, as HeightOfCFromHypotenuse is not valid for this context.
                //farthestVertexHeightFromLine = MiscMath.DistanceFromPointToLine(x1, y1, x2, y2, x3, y3);

                // Farthest point has to be added at the nearest edge of the tour
                if (closestInsertEdge == null || closestInsertEdge.distance > farthestVertexHeightFromLine)
                {
                    closestInsertEdge = new Edge { vertex1 = graph.vertices[v1ID], vertex2 = graph.vertices[v2ID], distance = farthestVertexHeightFromLine };
                    if (this.usedVertices.Count < 2)
                        closestInsertEdge.insertAfterIndex = i + 1;
                    else
                        closestInsertEdge.insertAfterIndex = i;
                }
            }

            if (farthestInsertEdge != null)
            {
                farthestInsertEdge.insertAfterIndex = closestInsertEdge.insertAfterIndex;
                // Append the best Insert Vortex to the appropriate location
                this.usedVertices.Insert(farthestInsertEdge.insertAfterIndex + 1, farthestInsertEdge.vertex2);
                // Recall the Method
                return this.FarthestInsertionHeuristicRecurring();
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
