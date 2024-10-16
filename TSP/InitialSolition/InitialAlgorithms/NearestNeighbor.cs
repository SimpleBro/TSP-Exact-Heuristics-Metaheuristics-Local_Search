using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP.InitialSolition.InitialAlgorithms
{
    // Source: https://github.com/a-mile/TravellingSalesmanProblem
    internal class NearestNeighbor
    {
        readonly public string nearestNeighbourType = typeof(NearestNeighbor).Name;
        public NearestNeighbor()
        {
            usedVertices = new Stack<Vertex>();
            verticesStack = new Stack<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = 0;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        List<Vertex> shortestPath;
        Vertex startVertex;
        Stack<Vertex> usedVertices;
        Stack<Vertex> verticesStack;

        /// <summary>
        /// For each Vertex calculate the minimum distance path. Return the minimum cost path.
        /// The Repetitive NN applies the nn repeatedly, using each vertices as a starting point.
        /// It selects the starting point that produced the shortest circuit.
        /// </summary>
        /// <returns>Minimum Distance Path List</returns>
        public List<Vertex> NearestNeighbourOptimization()
        {
            foreach (KeyValuePair<int, Vertex> v in graph.vertices)
            {
                startVertex = v.Value;
                NearestNeighbourRecurring(startVertex);
                if (minDistance == 0 || distance < minDistance)
                {
                    minDistance = distance;
                    shortestPath.Clear();
                    shortestPath.AddRange(verticesStack.ToList());
                    shortestPath.Reverse();
                    this.soulutionCount++;
                }
                this.iterationCount++;
            }

            return shortestPath;
        }

        int counter = 0;
        double distance = 0;

        /// <summary>
        /// The NN begins at a vertex and follows the edge of least weight from that vertex.
        /// At every subsequent vertex, it follows the edge of least weight that leads to a city not yet visited, 
        /// until it returns to the starting point.
        /// </summary>
        private bool NearestNeighbourRecurring(Vertex vertex)
        {
            counter++;
            usedVertices.Push(vertex);  // Add the Vertex to the stack of used vertices
            verticesStack.Push(vertex);

            // Last best knows edge
            Edge nextEdge = null;

            foreach (KeyValuePair<Tuple<int, int>, Edge> e in vertex.neighbors)
            {
                // Start Vertex should be the Last Vertex in the stack
                if (e.Value.vertex2 == startVertex)
                {
                    if (counter == graph.vertices.Count)
                    {
                        verticesStack.Push(e.Value.vertex2);
                        distance += e.Value.distance;
                        return true;
                    }
                }

                // Select the Edge with the smalles distance cost
                // i.e. Select the closest Vortex neighbour 
                if (!usedVertices.Contains(e.Value.vertex2))
                {
                    // If the distance of the previous selected neighbour edge is bigger than current edge, select current
                    if (nextEdge == null || nextEdge.distance > e.Value.distance)
                    {
                        nextEdge = e.Value;
                    }
                }
            }

            if (nextEdge != null)
            {
                // If the solution is feasable, the edge is selected to the path
                // Go into that edges vertex2 and find the next least cost edge
                if (NearestNeighbourRecurring(nextEdge.vertex2))
                {
                    // Distance of the Edge - Recursively - Count of visited = Count of vertices;
                    // Append the distance (cost) of each selected edge
                    distance += nextEdge.distance;
                    return true;
                }
            }

            // Temp Vertex did not meet the requirements, remove it from the stack
            usedVertices.Pop();
            counter--;
            // Infeasable solution
            return false;
        }
    }
}
