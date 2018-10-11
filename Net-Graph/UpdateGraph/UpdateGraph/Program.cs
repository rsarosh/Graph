using System;
using System.Collections.Generic;

namespace UpdateReceipe
{
    class Program
    {

        static void Main(string[] args)
        {
            bool bProperty = true;
            Graph _g = new Graph(bProperty);
            //UpdateGraph(_g, bProperty);
            QueryGraph(_g, bProperty);
        }

        private static void QueryGraph(Graph g, bool bProperty)
        {
            string s;

            if (bProperty)
            {
                s = g.QueryGraph("g.V().has('Ingredients', 'MILK')");
            } else
            {
                s = g.QueryGraph("g.V('MILK').inE().outV()");  // - Get all the vertex which are coming to Turmeric
            }
            Console.WriteLine(s);
            


        }

        private static void UpdateGraph(Graph g, bool asProperty)
        {
            string[] lines = System.IO.File.ReadAllLines(@"products.csv");
            string Recipe = string.Empty;
            List<string> Ingredients = new List<string>();

            int ctr = 0;
            foreach (string line in lines)
            {
                if (ctr == 0)
                {
                    ctr++;
                    continue;
                }
                Ingredients.Clear();

                string[] columns = line.Split(',');

                for (int i = 0; i < columns.Length; i++)
                {
                    if (i == 1)
                    {
                        string s = CleanString(columns, i);

                        if (asProperty)
                        {
                            Recipe = s;   //No need to add the vertext now
                        } else
                        {
                            Recipe = g.AddVertex(s);   
                        }
                    }

                    if (i > 6)
                    {
                        string s = CleanString(columns, i);
                        Ingredients.Add(s);
                    }
                }

                if (!String.IsNullOrEmpty(Recipe) && Ingredients.Count > 0)
                {
                    if (asProperty)
                    {
                        g.AddVertex(Recipe, Ingredients);
                    }
                    else
                    {
                        g.AddEdge(Recipe, Ingredients);
                    }
                }


            }
        }

        private static string CleanString(string[] columns, int i)
        {

            //get rid of string before colon :
            string s = columns[i].Substring(columns[i].LastIndexOf(':') + 1);

            //remove "(" or ")"
            s = s.Replace('(', ' ');
            s = s.Replace(')', ' ');
            s = s.Replace('\\', ' ');
            s = s.Replace('/', ' ');
            s = s.Replace('.', ' ');
            s = s.Replace(';', ' ');
            s = s.Replace(':', ' ');
            s = s.Replace('"', ' ');
            s = s.Replace('\"', ' ');
            s = s.Replace('\'', ' ');
            s = s.Replace('&', ' ');
            s = s.Replace('%', ' ');
            s = s.Replace('^', ' ');
            s = s.Replace('#', ' ');
            s = s.Replace('*', ' ');
            s = s.Replace('!', ' ');
            s = s.Replace('&', ' ');
            s = s.Replace('&', ' ');
            s = s.Replace("\"", "");

            s = s.Trim();

            return s;
        }

    }
   
}
