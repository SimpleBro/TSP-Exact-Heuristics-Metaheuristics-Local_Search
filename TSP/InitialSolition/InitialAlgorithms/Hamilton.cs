using System;
using System.Collections.Generic;
using System.Threading;

namespace TSP.InitialSolition.InitialAlgorithms
{
    // Source: https://github.com/a-mile/TravellingSalesmanProblem
    public class Hamilton
    {
        readonly public string hamiltonType = typeof(Hamilton).Name;
        public Hamilton()
        {
            verticesStack = new Stack<Vertex>();
            usedVertices = new Stack<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = 0;
        }

        public Graph graph { get; set; }
        public double minDistance { get; private set; }
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        Vertex startVertex;
        List<Vertex> shortestPath;
        Stack<Vertex> verticesStack;
        Stack<Vertex> usedVertices;

        public List<Vertex> ShortestHamiltonCycle()
        {
            startVertex = graph.vertices[0];
            HamiltonCycleRecurring(startVertex);
            return shortestPath;
        }

        int counter = 0;
        double distance = 0;

        void HamiltonCycleRecurring(Vertex vertex)
        {
            counter++;
            usedVertices.Push(vertex);
            verticesStack.Push(vertex);

            foreach (KeyValuePair<Tuple<int, int>, Edge> edge in vertex.neighbors)
            {
                if (edge.Value.vertex2 == startVertex)
                {
                    if (counter == graph.vertices.Count)
                    {
                        verticesStack.Push(edge.Value.vertex2);
                        distance += edge.Value.distance;

                        if (minDistance == 0 || distance < minDistance)
                        {
                            shortestPath.Clear();
                            shortestPath.AddRange(verticesStack);
                            minDistance = distance;
                            this.soulutionCount++;
                        }

                        distance -= edge.Value.distance;
                        verticesStack.Pop();
                        this.iterationCount++;
                    }
                }

                if (!usedVertices.Contains(edge.Value.vertex2))
                {
                    distance += edge.Value.distance;
                    HamiltonCycleRecurring(edge.Value.vertex2);
                    distance -= edge.Value.distance;
                }
            }

            verticesStack.Pop();
            usedVertices.Pop();
            counter--;
        }
    }
}
