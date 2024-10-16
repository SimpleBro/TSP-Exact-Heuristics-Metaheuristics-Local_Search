using System;
using System.Collections.Generic;

namespace TSP.Metaheuristics.PopulationAlgorithms
{
    // Source: https://github.com/a-mile/TravellingSalesmanProblem
    public class AntColony
    {
        readonly public string antType = typeof(AntColony).Name;
        public AntColony()
        {
            visitedVertices = new Stack<Vertex>();
            visitedEdges = new List<Edge>();
        }

        public Vertex startVertex { get; set; }
        public Stack<Vertex> visitedVertices { get; set; }
        public List<Edge> visitedEdges { get; set; }
        public double travelledDistance = 0;
    }

    public class AntOptimization
    {
        readonly public string antOptimizationType = typeof(AntOptimization).Name;
        public AntOptimization()
        {
            ants = new List<AntColony>();
            shortestPath = new List<Vertex>();
            random = new Random();
            minDistance = 0;
        }

        Random random;
        public Graph graph { get; set; }
        public List<AntColony> ants { get; set; }
        List<Vertex> shortestPath;
        public double minDistance { get; private set; }
        public int iterationCount { get; private set; }
        public int soulutionCount { get; private set; }

        private void AntGenerator(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                this.iterationCount++;
                ants.Add(
                    new AntColony()
                    {
                        startVertex = graph.vertices[random.Next(0, graph.vertices.Count - 1)]
                    }
                );
            }
        }

        public List<Vertex> AntColonyOptimization(int iterations, int antAmount)
        {
            AntGenerator(antAmount);

            for (int i = 0; i < iterations; i++)
            {
                this.iterationCount++;
                AntReset();

                foreach (AntColony a in ants)
                {
                    this.iterationCount++;
                    AntPath(a);
                    Pheromone(a);

                    if (minDistance == 0 || a.travelledDistance < minDistance)
                    {
                        minDistance = a.travelledDistance;
                        shortestPath.Clear();
                        shortestPath.AddRange(a.visitedVertices);
                        this.soulutionCount++;
                    }
                }
            }
            return shortestPath;
        }

        private void AntReset()
        {
            foreach (AntColony a in ants)
            {
                this.iterationCount++;
                a.travelledDistance = 0;
                a.visitedEdges.Clear();
                a.visitedVertices.Clear();
            }
        }

        int counter;

        private void AntPath(AntColony ant)
        {
            counter = 0;
            AntPathRecurring(ant, ant.startVertex);
        }

        private void AntPathRecurring(AntColony ant, Vertex vertex)
        {
            counter++;
            ant.visitedVertices.Push(vertex);
            Edge nextEdge = null;

            if (counter == graph.vertices.Count)
            {
                foreach (KeyValuePair<Tuple<int, int>, Edge> e in vertex.neighbors)
                {
                    this.iterationCount++;
                    if (e.Value.vertex2 == ant.startVertex)
                    {
                        ant.travelledDistance += e.Value.distance;
                        AntPathRecurring(ant, e.Value.vertex2);
                    }
                }
            }
            else
            {
                nextEdge = NextEdge(ant);
                if (nextEdge != null)
                {
                    ant.visitedEdges.Add(nextEdge);
                    ant.travelledDistance += nextEdge.distance;
                    AntPathRecurring(ant, nextEdge.vertex2);
                }
            }
        }

        private Edge NextEdge(AntColony ant)
        {
            double c = 0;
            double r = random.NextDouble();

            Vertex vertex = ant.visitedVertices.Peek();

            foreach (KeyValuePair<Tuple<int, int>, Edge> e in vertex.neighbors)
            {
                this.iterationCount++;
                if (!ant.visitedVertices.Contains(e.Value.vertex2))
                {
                    c += Probability(ant, e.Value);
                    if (c >= r)
                    {
                        return e.Value;
                    }
                }
            }
            return null;
        }

        public double ro { get; set; }

        private void Pheromone(AntColony ant)
        {
            foreach (KeyValuePair<int, Vertex> v in graph.vertices)
            {
                this.iterationCount++;
                foreach (KeyValuePair<Tuple<int, int>, Edge> e in v.Value.neighbors)
                {
                    this.iterationCount++;
                    if (ant.visitedEdges.Contains(e.Value))
                    {
                        e.Value.pheromoneValue = (1 - ro) * e.Value.pheromoneValue + (1 / (double)ant.travelledDistance);
                    }
                    else
                    {
                        e.Value.pheromoneValue = (1 - ro) * e.Value.pheromoneValue;
                    }
                }
            }
        }

        public int alpha { get; set; }
        public int beta { get; set; }

        private double Probability(AntColony ant, Edge edge)
        {
            double nominator = 0;
            double denominator = 0;
            Vertex vertex = ant.visitedVertices.Peek();

            nominator = Math.Pow(edge.pheromoneValue, alpha) * Math.Pow(1 / (double)edge.distance, beta);

            foreach (KeyValuePair<Tuple<int, int>, Edge> e in vertex.neighbors)
            {
                this.iterationCount++;
                if (!ant.visitedVertices.Contains(e.Value.vertex2))
                {
                    denominator += Math.Pow(e.Value.pheromoneValue, alpha) * Math.Pow(1 / (double)e.Value.distance, beta);
                }
            }
            return nominator / denominator;
        }
    }
}
