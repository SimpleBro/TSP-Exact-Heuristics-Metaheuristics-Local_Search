using System;
using System.Collections.Generic;

namespace TSP
{
    public class Vertex
    {
        readonly public string vertexType = typeof(Vertex).Name;
        public Vertex()
        {
            this.neighbors = new Dictionary<Tuple<int, int>, Edge>();
            this.isVisited = false;
            this.isCS = false;
            this.isDepot = false;
        }
        public bool isDepot { get; set; }
        public bool isCS { get; set; }
        public bool isVisited { get; set; }
        public int index { get; set; }
        public double data { get; set; }
        public GeoLoc geoLoc { get; set; }
        public GeoLoc pctBoxScaledGeoLoc { get; set; }
        public Dictionary<Tuple<int, int>, Edge> neighbors { get; set; }
        public int demand { get; set; }
        //ReadyTime DueDate ServiceTime 
        public double serviceTime { get; set; }
        public double earliestStartTime { get; set; }
        public double latestStartTime { get; set; }
        public int linkID { get; set; }
    }

    public class Edge
    {
        readonly public string edgeType = typeof(Edge).Name;
        public Edge()
        {
            this.vertex1 = new Vertex();
            this.vertex2 = new Vertex();
        }
        public double distance { get; set; }
        public double pheromoneValue { get; set; }
        public int insertAfterIndex { get; set; }
        public Vertex vertex1 { get; set; }
        public Vertex vertex2 { get; set; }
    }

    public class Graph
    {
        public Graph()
        {
            this.vertices = new Dictionary<int, Vertex>();
            this.edges = new Dictionary<Tuple<int, int>, Edge>();
            this.depots = new Dictionary<int, Vertex>();
        }
        public bool fullGraph { get; set; }
        public bool symetricGraph { get; set; }
        public int depotCount { get; set; }
        public int csCount { get; set; }
        public int customerCount { get; set; }
        public double averageDistance { get; set; }
        public Dictionary<int, Vertex> vertices { get; set; }
        public Dictionary<Tuple<int, int>, Edge> edges { get; set; }
        public Dictionary<int, Vertex> depots { get; set; }
        public GeoLoc graphCentroid { get; set; }
    }

    public class GeoLoc
    {
        public double latX { get; set; }
        public double longY { get; set; }
    }
}
