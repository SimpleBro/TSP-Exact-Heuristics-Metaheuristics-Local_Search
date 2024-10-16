using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TSP.Miscellaneous
{
    internal class TSPLIB
    {
        /// <summary>
        /// Parse the target .tsp file stored in the project "TSPLIBFiles" directory.
        /// Store each parsed geo-location as a customer and return the complete weighted graph.
        /// </summary>
        public static Graph ReadTSPFile(string targetFile)
        {
			try
			{
                targetFile = "TSPLIBFiles\\" + targetFile + ".tsp";
                Graph graph = new Graph
                {
                    fullGraph = true,
                    symetricGraph = true
                };

                string fileName = String.Empty;
                int dimension = 0;
                int optimalObjectiveFunction = 0;

                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (FileStream fs = new FileStream(targetFile, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line on spaces
                        string[] parts = line.Split(' ');

                        // Check the first part to see what type of information is on this line
                        if (parts[0].Contains("NAME"))
                        {
                            // The line contains the name of the TSP problem
                            fileName = parts.Last();
                        }
                        else if (parts[0].Contains("DIMENSION"))
                        {
                            // The line contains the dimension of the TSP problem
                            dimension = int.Parse(parts.Last());
                        }
                        else if (parts[0].Contains("OPTIMAL:"))
                        {
                            optimalObjectiveFunction = int.Parse(parts.Last());
                        }
                        else if (parts[0].Contains("NODE_COORD_SECTION"))
                        {
                            // The next lines contain the coordinates of the nodes
                            while ((line = reader.ReadLine()) != null)
                            {
                                parts = line.Split(' ');
                                parts = parts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                                if (parts[0] == "EOF")
                                {
                                    // End of the node coordinates section
                                    break;
                                }
                                else
                                {
                                    // Extract the node index and coordinates
                                    int.TryParse(parts[0], out int index);
                                    double.TryParse(parts[1].Replace('.', ','), out double x);
                                    double.TryParse(parts[2].Replace('.', ','), out double y);

                                    // Store the node information in your program
                                    GeoLoc tempLoc = new GeoLoc
                                    {
                                        latX = x,
                                        longY = y
                                    };
                                    Vertex tempVertex = new Vertex();
                                    tempVertex.index = index - 1;
                                    tempVertex.geoLoc = tempLoc;

                                    graph.vertices[index - 1] = tempVertex;
                                }
                            }
                        }
                    }
                }

                graph.vertices[0].isDepot = true;
                graph.depots[0] = graph.vertices[0];
                graph.depotCount = 1;
                graph.csCount = 0;
                graph.customerCount = graph.vertices.Count - 1;
                graph = GraphMethods.GenerateEdgeConnectionsAndDistances(graph);

                GUI.EventLog("TSPLIB", MethodBase.GetCurrentMethod().Name,
                        "INFO", sw.Elapsed.TotalSeconds.ToString(), 
                        "TSPLIB: " + fileName + ", Dimension: " + dimension + 
                        "\nOptimal objective function: " + optimalObjectiveFunction);

                return graph;
            }
            catch (Exception ex)
            {
                GUI.EventLog("TSPLIB", MethodBase.GetCurrentMethod().Name, "ERROR", "0", ex.Message);
                return new Graph();
            }
        }
    }
}
