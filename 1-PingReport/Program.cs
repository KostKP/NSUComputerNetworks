using System.Net.NetworkInformation;

Console.WriteLine("Task 1 - Ping report");

List<string> addresses = new List<string>
{
    "127.0.0.1",
    "github.com",
    "google.com",
    "stackoverflow.com",
    "developer.mozilla.org",
    "w3schools.com",
    "youtube.com",
    "wikipedia.org",
    "weather.com",
    "duckduckgo.com"
};

int timeout = 120;
int repeats = 5;

Dictionary<string, List<long>> pingReport = new Dictionary<string, List<long>>();
using (Ping ping = new Ping())
{
    for (int i = 0; i < addresses.Count; i++)
    {
        string padding = string.Empty;
        for (int j = 0; j < addresses.Count.ToString().Length; j++) { padding += "0"; }

        Console.WriteLine($"[{(i + 1).ToString(padding)}/{addresses.Count}] Pinging {addresses[i]}...");
        pingReport.Add(addresses[i], new List<long>());
        for (int r = 0; r < repeats; r++)
        {
            try
            {
                PingReply reply = ping.Send(addresses[i], timeout);
                if (reply.Status == IPStatus.Success) { pingReport[addresses[i]].Add(reply.RoundtripTime); }
            } catch { }
        }
        if (pingReport[addresses[i]].Count == 0) { pingReport[addresses[i]].Add(-1); }
    }
}

Console.WriteLine("Output:");
Console.WriteLine("address, min, avg, max");
foreach (string address in addresses)
{
    Console.WriteLine($"{address}, {pingReport[address].Min()}, {(int)pingReport[address].Average()}, {pingReport[address].Max()}");
}

Console.ReadKey();