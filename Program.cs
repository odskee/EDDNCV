using Ionic.Zlib;
using NetMQ;
using NetMQ.Sockets;
using System.Text;
using System.Text.Json.Nodes;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var utf8 = new UTF8Encoding();
            string ContribId = "";
            string MatchSchema = "";
            string Provider = "";
            string FleetCarrier = "";

            // Proc any command args
            if (args.Length > 0)
            {
                // first should be a command, if not, not valid..
                if (!args[0].StartsWith("-"))
                {
                    throw new ArgumentException(args[0]);
                }

                int c = 0;
                // asses each arg in turn..
                foreach (string arg in args)
                {
                    // if it doesn't start with a '-' then we're not interested
                    if (arg.StartsWith("-"))
                    {
                        // starts with '-' so it's a command
                        switch (arg)
                        {
                            case "-uploader":
                            ContribId = args[c + 1];    // we want the argument after this one
                            break;

                            case "-schema":
                            MatchSchema = args[c + 1];
                            break;

                            case "-provider":
                            Provider = args[c + 1];
                            break;

                            case "-fc":
                            FleetCarrier = args[c + 1];
                            break;

                            default:
                            //throw new ArgumentException(args[0]);   // it starts with a '-' but we don't recognise it - it's garbage.
                            Console.WriteLine($"Unknown Command: {args[c]} {args[c + 1]}" + Environment.NewLine);   //soft-fail option
                            break;
                        }
                    }

                    // keep track of the foreach loop
                    c++;
                }
            }

            // connect and listen to EDDN
            using (var client = new SubscriberSocket())
            {
                client.Options.ReceiveHighWatermark = 1000;
                client.Connect("tcp://eddn.edcd.io:9500");
                client.SubscribeToAnyTopic();
                while (true)
                {
                    var bytes = client.ReceiveFrameBytes();
                    var uncompressed = ZlibStream.UncompressBuffer(bytes);

                    string result = utf8.GetString(uncompressed);

                    // no args means all responses get printed
                    if (args.Length == 0)
                    {
                        Console.WriteLine(result + Environment.NewLine);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(result))
                        {
                            // inspect JSON DOM for relevant data
                            JsonNode JResp = JsonNode.Parse(result);

                            if (JResp != null)
                            {
                                // if responsone and command line are both empty they'll match, we don't want that
                                if (!string.IsNullOrEmpty(ContribId) && ContribId.Equals(JResp!["header"]!["uploaderID"]!.GetValue<string>()))
                                {
                                    Console.WriteLine(result + Environment.NewLine);
                                }

                                // Docked at specific carrier
                                if (JResp!["$schemaRef"]!.GetValue<string>().Equals("https://eddn.edcd.io/schemas/journal/1") && JResp!["message"]!["event"]!.GetValue<string>().Equals("Docked"))
                                {
                                    if (FleetCarrier.Equals(JResp!["message"]!["StationName"]!.GetValue<string>()))
                                    {
                                        Console.WriteLine(result + Environment.NewLine);
                                    }
                                }

                                // Match a specific schema type
                                if (JResp!["$schemaRef"]!.GetValue<string>().Equals(MatchSchema))
                                {
                                    Console.WriteLine(result + Environment.NewLine);
                                }

                                // match a specific contribution source
                                if (JResp!["header"]!["softwareName"]!.GetValue<string>().Equals(Provider))
                                {
                                    Console.WriteLine(result + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
