using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using TSP.Miscellaneous;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace TSP
{
    internal class GraphMethods
    {
        readonly public string graphMethodsType = typeof(GraphMethods).Name;
        /// <summary>
        /// Initialise a Graph object with Vertex and Edge objects. Edge distance are random values.
        /// </summary>
        public static Graph CreateRandomGraph()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                int maxWidth = (int)Properties.Settings.Default.graphWidth;
                int maxHeight = (int)Properties.Settings.Default.graphHeight;

                Graph graph = new Graph
                {
                    depotCount = Properties.Settings.Default.depotCount,
                    customerCount = Properties.Settings.Default.customerCount,
                    fullGraph = Properties.Settings.Default.fullGraph,
                    symetricGraph = Properties.Settings.Default.symetricGraph
                };
                Random rand = new Random();

                int size = graph.depotCount + graph.csCount + graph.customerCount;

                for (int i = 0; i < size; i++)
                {
                    Vertex vertex = new Vertex();
                    vertex.index = i;

                    // X,Y coordinates of the random generated Vertex (user) should not be on the edges of the PictureBox.
                    GeoLoc gLoc = new GeoLoc
                    {
                        latX = rand.Next(80, maxWidth - 100),
                        longY = rand.Next(80, maxHeight - 100)
                    };
                    vertex.geoLoc = gLoc;
                    vertex.pctBoxScaledGeoLoc = gLoc;

                    // Mark "Depot" vertices.
                    if (i < graph.depotCount)
                    {
                        vertex.isDepot = true;
                        graph.depots[i] = vertex;
                    }
                    graph.vertices[i] = vertex;
                }

                graph = GenerateEdgeConnectionsAndDistances(graph);
                sw.Stop();

                string logData = String.Format("Full graph: {0}; Symetric graph: {1}; Vertex: {2}; Edge: {3}; Depot: {4}", 
                    graph.fullGraph, graph.symetricGraph, graph.vertices.Count, graph.edges.Count, graph.depotCount);              
                GUI.EventLog("GraphMethods", MethodBase.GetCurrentMethod().Name, "INFO", sw.Elapsed.TotalSeconds.ToString(), logData);

                return graph;
            }
            catch (Exception ex)
            {
                GUI.EventLog("GraphMethods", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new Graph();
            }
        }

        /// <summary>
        /// Generate edge connections and distances for the set of vertices in the input Graph object.
        /// </summary>
        public static Graph GenerateEdgeConnectionsAndDistances(Graph graph)
        {
            try
            {
                Random rand = new Random();
                int size = graph.depotCount + graph.csCount + graph.customerCount;
                double distSum = 0;
                List<GeoLoc> geoLoc = new List<GeoLoc>();

                // For each row/column pair, generate the cost value - Euclidian distance.
                for (int i = 0; i < size; i++)
                {
                    for (int j = i + 1; j < size; j++)
                    {
                        // If it is not a full graph, decide if the matrix i,j pair should have a direct connection.
                        if (!graph.fullGraph)
                        {
                            // All vertices should have a connection to the depot.
                            if (i >= graph.depotCount)
                            {
                                if (rand.Next(0, 100) > Properties.Settings.Default.connectioProbability)
                                {
                                    if (graph.vertices[i].neighbors.Count >= MiscMath.RoundToInt(graph.vertices.Count / 2))
                                        continue;
                                }
                            }
                            else
                                graph.vertices[i].isDepot = true;
                        }

                        double x_1 = Convert.ToDouble(graph.vertices[i].geoLoc.latX);
                        double y_1 = Convert.ToDouble(graph.vertices[i].geoLoc.longY);
                        double x_2 = Convert.ToDouble(graph.vertices[j].geoLoc.latX);
                        double y_2 = Convert.ToDouble(graph.vertices[j].geoLoc.longY);

                        // Edge from vertex 1 to vertex 2.
                        Edge edge = new Edge
                        {
                            distance = Math.Ceiling(MiscMath.EuclidianDistance(x_1, y_1, x_2, y_2)),
                            pheromoneValue = Properties.Settings.Default.pheromoneValue,
                            vertex1 = graph.vertices[i],
                            vertex2 = graph.vertices[j]
                        };

                        graph.vertices[i].neighbors[Tuple.Create(i, j)] = edge;
                        graph.edges[Tuple.Create(i, j)] = edge;

                        double asymetricDist = edge.distance;

                        if (!graph.symetricGraph)
                        {
                            if (rand.Next(1) == 1)
                                asymetricDist = rand.Next((int)(asymetricDist - asymetricDist + 10), (int)asymetricDist);
                            else
                                asymetricDist = rand.Next((int)(asymetricDist - asymetricDist * 0.5), (int)asymetricDist * 2);
                        }

                        // Edge from vertex 2 to vertex 1.
                        Edge edge2 = new Edge
                        {
                            distance = Math.Ceiling(asymetricDist),
                            pheromoneValue = Properties.Settings.Default.pheromoneValue,
                            vertex1 = graph.vertices[j],
                            vertex2 = graph.vertices[i]
                        };

                        graph.vertices[j].neighbors[Tuple.Create(j, i)] = edge2;
                        graph.edges[Tuple.Create(j, i)] = edge2;

                        // Total distance of the graph.
                        distSum += edge.distance + edge2.distance;
                    }

                    // All vertices of the graph.
                    geoLoc.Add(graph.vertices[i].geoLoc);
                }

                graph.averageDistance = Math.Round(distSum / graph.edges.Count, 2);
                graph.graphCentroid = MiscMath.GetCentroid(geoLoc);

                return graph;
            }
            catch (Exception ex)
            {
                GUI.EventLog("GraphMethods", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new Graph();
            }
        }

        /// <summary>
        /// Generate Distance Matrix from initialised Graph object.
        /// </summary>
        public static double[,] GraphDistanceMatrix(Graph graph)
        {
            try
            {
                int vortexSize = graph.vertices.Count;
                double[,] distMatrix = new double[vortexSize, vortexSize];

                // Init distance matrix with negative values == no connection.
                for (int i = 0; i < distMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < distMatrix.GetLength(1); j++)
                    {
                        distMatrix[i, j] = -1;
                        if (i == j)
                            distMatrix[i, j] = 0;
                    }
                }

                // Fill distance matrix with edge values.
                foreach (KeyValuePair<int, Vertex> vertex in graph.vertices)
                {
                    foreach (KeyValuePair<Tuple<int,int>, Edge> edge in vertex.Value.neighbors)
                    {
                        distMatrix[edge.Value.vertex1.index, edge.Value.vertex2.index] = edge.Value.distance;
                    }
                }

                return distMatrix;
            }
            catch (Exception ex)
            {
                GUI.EventLog("GraphMethods", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new double[0, 0];
            }
        }

        /// <summary>
        /// Return the distance nearest neighbor of the target vertex, given that the selected neighbor is not part of the current solution.
        /// </summary>
        public static Vertex NearestNeighbor(Vertex vertex, List<Vertex> usedVertices)
        {
            try
            {
                Edge cheapestEdge = null;

                foreach (KeyValuePair<Tuple<int, int>, Edge> e in vertex.neighbors)
                {
                    if (e.Value.vertex2 == vertex)
                    {
                        continue;
                    }
                    if (!usedVertices.Contains(e.Value.vertex2))
                    {
                        if (cheapestEdge == null || cheapestEdge.distance > e.Value.distance)
                        {
                            cheapestEdge = e.Value;
                        }
                    }
                }

                if (cheapestEdge == null)
                    return new Vertex { index = -1 };

                return cheapestEdge.vertex2;
            }
            catch (Exception ex)
            {
                GUI.EventLog("NearestNeighbor", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new Vertex { index = -1 };
            }
        }

        /// <summary>
        /// Return the distance farthest vertices in the graph. The vertices must have a existing connection.
        /// </summary>
        public static List<Vertex> FarthestVetricesPairInGraph()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();

                List<Vertex> farthestVertices = new List<Vertex>();
                double distance = -1;

                foreach (Vertex v in graph.vertices.Values)
                {
                    foreach (KeyValuePair<Tuple<int, int>, Edge> neighbor in v.neighbors)
                    {
                        if (v == neighbor.Value.vertex2)
                            continue;

                        if (neighbor.Value.distance > distance)
                        {
                            farthestVertices.Clear();
                            farthestVertices.Insert(0, v);
                            farthestVertices.Insert(1, neighbor.Value.vertex2);
                            distance= neighbor.Value.distance;
                        }
                    }
                }

                return farthestVertices;
            }
            catch (Exception ex)
            {
                GUI.EventLog("FarthestVetricesPairInGraph", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new List<Vertex>();
            }
        }

        /// <summary>
        /// Return the total distance cost of the path.
        /// </summary>
        public static double PathDistanceCost(List<Vertex> usedVertices)
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();
                double distance = 0;

                //int lastElement = (usedVertices.Count > 2) ? 2 : 1;
                for (int i = 0; i <= usedVertices.Count - 2; i++)
                {
                    distance += graph.edges[Tuple.Create(usedVertices[i].index, usedVertices[i + 1].index)].distance;
                }

                return Math.Round(distance, 2);
            }
            catch (Exception ex)
            {
                GUI.EventLog("PathDistanceCost", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// From the current path string, construct and return a List of vertices.
        /// </summary>
        public static List<Vertex> PathStringToVertexList()
        {
            try
            {
                Graph graph = GraphInitialisation.IGetGraph();
                List<Vertex> path = new List<Vertex>();

                string[] pathElements = Properties.Settings.Default.lastPath.Split(new[] { " --> " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pElement in pathElements)
                {
                    path.Add(graph.vertices[Convert.ToInt32(pElement)]);
                }

                return path;
            }
            catch (Exception ex)
            {
                GUI.EventLog("PathStringToVertexList", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new List<Vertex>();
            }
        }
    }
}
