
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace namespaceStuktur
{

    class Graph
    {
        public List<Node> nodeList;

        public List<Edge> edgeList;

        public Dictionary<string,Edge> dictionary = new Dictionary<string, Edge>();

        public Graph(List<Node> nodeList, List<Edge> edgeList)
        {
            this.nodeList = nodeList;
            this.edgeList = edgeList;


            foreach (Edge e in edgeList) {
                dictionary.Add( e.endNode.id +"-"+ e.startNode.id, e);
                dictionary.Add( e.startNode.id +"-"+ e.endNode.id, e);
            }
        }


        public void reset() {
            foreach (Node d in nodeList) {
                d.visited = false;
            }
        }


        public Edge findEdge(Node startNode,Node endNode)
        {
            Edge edge = dictionary[startNode.id + "-" + endNode.id];

            return edge;
        }

    }

    class MST
    {
        public List<Node> nodeList;

        public List<Edge> edgeList;

        public MST(List<Edge> edgeList,int size)
        {
            
            this.edgeList = edgeList;
            this.nodeList = new List<Node>();

            for (int i = 0; i < size; i++)
            {
                Node node = new Node(i);
                nodeList.Add(node);
            }

            foreach (Edge edge in edgeList)
            {
               
                Node startNode = nodeList[edge.startNode.id];
                Node endNode = nodeList[edge.endNode.id];

                startNode.edgeList.Add(edge);
                endNode.edgeList.Add(edge);

                startNode.nodeList.Add(endNode);
                endNode.nodeList.Add(startNode);

                startNode.weight = edge.weight;
                endNode.weight = edge.weight;
            }

        }


        public void reset()
        {
            foreach (Node d in nodeList)
            {
                d.visited = false;
            }

        }


    }

    class Node : IComparable<Node>
    {
        public int id;

        public List<Edge> edgeList;

        public List<Node> nodeList;

        public bool visited = false;

        public double weight;

        public Node(int id)
        {
            this.id = id;
            this.edgeList = new List<Edge>();
            this.nodeList = new List<Node>();
            this.weight = float.MaxValue;
        }

        public int CompareTo(Node other)
        {
            return this.weight.CompareTo(other.weight);
        }

        public override string ToString()
        {
            return "" + id;
        }

    }

    class Edge : IComparable<Edge>
    {
        public Node endNode;
        public Node startNode;
        public double weight;

        public Edge(Node startNode, Node endNode, double weight)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.weight = weight;
        }

        public int CompareTo(Edge other)
        {
           return this.weight.CompareTo(other.weight);
        }
    }

   


}