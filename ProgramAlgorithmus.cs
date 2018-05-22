using namespaceStuktur;
using namespaceUtility;
using System;
using System.Collections;
using System.Collections.Generic;

namespace namespaceAlgorithmus
{
    class Algorithmus
    {

        double result = 0;
        double minResult = Double.MaxValue;
       

        public void zeitOfAlgorithmus(string path, String methode)
        {
            Console.WriteLine(methode);

            Algorithmus algorithmus = new Algorithmus();

            Graph graph = Parse.getGraphByFile(path);

            DateTime befor = System.DateTime.Now;

            if (methode == "Neighbor")
            {
                algorithmus.bestResultFormNearestNeighbor(graph);
            }
            else if (methode == "DoubleTree")
            {
                algorithmus.DoubleTree(graph);
            }
            else if (methode == "bruteforce")
            {
                algorithmus.bruteforce(graph);
            }
            else {
                algorithmus.branchUndBound(graph);
                
            }

            DateTime after = System.DateTime.Now;
            TimeSpan ts = after.Subtract(befor);
            Console.WriteLine("\n\n{0}s", ts.TotalSeconds);
        }

        public double bestResultFormNearestNeighbor(Graph graph)
        {
            double minResult = Double.MaxValue;

            foreach (Node node in graph.nodeList)
            {
                graph.reset();

                double result = neighborAlgorithmus(node, graph);

                Console.WriteLine(" weight :" + Math.Round(result, 2));

                if (result < minResult)
                {
                    minResult = result;
                }
            }

            Console.WriteLine("minimal:" + Math.Round(minResult, 2));
            return minResult;
        }


        public double neighborAlgorithmus(Node startNode, Graph graph)
        {
            Node originalStartNode = startNode;

            double result = 0;
            Edge minEdge = null;

            Console.Write(startNode.id + " ");

            while ((minEdge = findMinEdge(startNode)) != null)
            {
                startNode = minEdge.endNode;
                Console.Write(startNode.id + " ");
                result = result + minEdge.weight;
            }

            minEdge = graph.findEdge(originalStartNode, startNode);
            result = result + minEdge.weight;

            return result;
        }

        public Edge findMinEdge(Node startNode)
        {

            startNode.visited = true;

            List<Edge> minedges = new List<Edge>();
            foreach (Edge e in startNode.edgeList)
            {
                if (!e.endNode.visited)
                {
                    minedges.Add(e);
                }
            }

            if (minedges.Count > 0)
            {
                minedges.Sort();

                return minedges[0];
            }
            else
            {
                return null;
            }
        }


        public void DoubleTree(Graph graph)
        {
            MST mst = kruskal(graph);

            double min = Double.MaxValue;

            foreach (Node node in mst.nodeList)
            {
                List<Node> minTour = new List<Node>();
                DFS(node, minTour);

                double result = weightFromTour(minTour, graph);

                Console.Write(string.Join(",", minTour));
                Console.WriteLine(" weight :" + Math.Round(result, 2));

                if (result < min)
                {
                    min = result;
                }

                mst.reset();
            }

            Console.WriteLine("minimal:" + Math.Round(min, 2));
        }

        public void DFS(Node node, List<Node> result)
        {
            result.Add(node);

            node.visited = true;

            foreach (Node n in node.nodeList)
            {
                if (!n.visited)
                {
                    DFS(n, result);
                }
            }
        }

        public MST kruskal(Graph graph)
        {
            List<Edge> edges = new List<Edge>();

            graph.edgeList.Sort();

            UnionFind uf = new UnionFind();

            uf.MakeSet(graph.nodeList.Count);

            foreach (Edge edge in graph.edgeList)
            {
                if (uf.Union(edge.startNode.id, edge.endNode.id))
                {
                    edges.Add(edge);
                }
            }
            return new MST(edges, graph.nodeList.Count);
        }

        public double weightFromTour(List<Node> nodes, Graph graph)
        {

            double sum = 0;

            if (nodes.Count > 1)
            {
                Node startNode = nodes[nodes.Count - 1];

                for (int i = 0; i < nodes.Count; i++)
                {

                    Edge e = graph.findEdge(startNode, nodes[i]);
                    sum = sum + e.weight;

                    startNode = nodes[i];
                }
            }
            return sum;
        }


        public void bruteforce(Graph graph) {

            List<Node> minTour = new List<Node>();
            Node start = graph.nodeList[0];
            minTour.Add(start);
           

            bruteforceAlgorithmus(start, minTour, graph);

            Console.WriteLine("min weight : " + Math.Round(minResult, 2));
        }

        public void bruteforceAlgorithmus(Node startNode, List<Node> minTour, Graph graph)
        {
            startNode.visited = true;

            foreach (Node node in startNode.nodeList)
            {
                if (!node.visited)
                {
                    
                    minTour.Add(node);
                    Edge e = graph.findEdge(startNode, node);

                    result = result + e.weight;

                    bruteforceAlgorithmus(node, minTour, graph);

                    minTour.Remove(node);
                    result = result - e.weight;
                    node.visited = false;

                }
            }

            if (minTour.Count == graph.nodeList.Count)
            {
                double totalResult = 0;
                Edge e = graph.findEdge(minTour[0], minTour[minTour.Count - 1]);
                totalResult = result + e.weight;

                if (totalResult < minResult)
                {
                    minResult = totalResult;
                }

            }

        }


        public void branchUndBound(Graph graph)
        {

            List<Node> minTour = new List<Node>();
            Node start = graph.nodeList[0];
            minTour.Add(start);

            branchUndBoundAlgorithmus(start, minTour, graph);

            Console.WriteLine("min weight : " + Math.Round(minResult, 2));
           
        }

        public void branchUndBoundAlgorithmus(Node startNode, List<Node> minTour, Graph graph)
        {
            startNode.visited = true;

            if (result > minResult)
            {
               // Console.WriteLine(minTour.Count);
                return;
            }

            foreach (Node node in startNode.nodeList)
            {
                if (!node.visited)
                {

                    minTour.Add(node);
                    Edge e = graph.findEdge(startNode, node);

                    result = result + e.weight;

                    branchUndBoundAlgorithmus(node, minTour, graph);

                    minTour.Remove(node);
                    result = result - e.weight;
                    node.visited = false;

                }
            }

            if (minTour.Count == graph.nodeList.Count)
            {
                double totalResult = 0;
                Edge e = graph.findEdge(minTour[0], minTour[minTour.Count - 1]);
                totalResult = result + e.weight;

                if (totalResult < minResult)
                {
                    minResult = totalResult;
                }

            }




        }

    }
}
