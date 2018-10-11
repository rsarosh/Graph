using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateReceipe
{
    class Graph
    {
        private string hostname = @"rafat-graph.gremlin.cosmosdb.azure.com";
        private int port = 443;
        private string authKey = "";
        private string database = "Recipes";
        private string collection = "Recipes";
        private string collection2 = "Recipes2";
        GremlinClient gremlinClient;

        public Graph(bool property) {
            string CollectionName = string.Empty;
            if (property)
            {
                CollectionName = collection2;
            }
            else
                CollectionName = collection2;


            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true,
                                                    username: "/dbs/" + database + "/colls/" + CollectionName,
                                                    password: authKey);
            gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(), GremlinClient.GraphSON2MimeType);
        }

        public string QueryGraph (string query)
        {
            var task = gremlinClient.SubmitAsync<dynamic>(query);
            task.Wait();
            Console.WriteLine(String.Format("RU Cost {0}", task.Result.StatusAttributes["x-ms-total-request-charge"]));
            return task.Result.ToString();
        }
        public string AddVertex(string v)
        {
            try
            {
                string query = String.Format("g.addV('{0}').property('id', '{1}').property('name', '{2}')", v, v, v);
                var task = gremlinClient.SubmitAsync<dynamic>(query);
                task.Wait();
                Console.Write(".");
                Console.WriteLine(v);
                return task.Result.ToString();
            } catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    //Resource already exist
                    Console.Write("-");
                    return v;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
                return string.Empty;

            }
        }

        public string AddVertex(string v, List<string> properties)
        {
            try
            {
                string sProp = string.Empty;

                foreach  (string prop in properties)
                {
                    sProp = sProp + string.Format(".property ('{0}','{1}')", "Ingredients", prop);
                }

                string query = String.Format("g.addV('{0}').property('id', '{1}').property('name', '{2}')", v, v, v);
                query = query + sProp;

                var task = gremlinClient.SubmitAsync<dynamic>(query);
                task.Wait();
                Console.Write(".");
                Console.WriteLine(v);
                return task.Result.ToString();
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    //Resource already exist
                    Console.Write("-");
                    return v;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
                return string.Empty;

            }
        }

        public bool AddEdge(string v, List<string> vList)
        {
            foreach (string v2 in vList)
            {
                try
                {
                    // string query = String.Format("g.V().has('name', '{0}').addE ('uses').to (g.V().has('name', '{1}'))", v, v2);
                    string query = String.Format("g.V('{0}').addE ('uses').to (g.V('{1}'))", v, v2);
                    var task = gremlinClient.SubmitAsync<dynamic>(query);
                    Console.Write("+");
                    task.Wait();
                }
                catch (Exception ex)
                {
                    if (ex.HResult == -2146233088)
                    {
                        //Resource already exist
                        Console.Write("*");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return vList.Count > 0;
        }

    }
}
