using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

internal class Program
{
    static ChromeDriver driver = new ChromeDriver("../../../.");

    public class ModInfo
    {
        public string? avatar { get; private set; }
        public string title { get; private set; }
        public string author { get; private set; }
        public string? description { get; private set; }
        public string environment { get; private set; }

        public ModInfo(string title, string author, string environment, string? description, string? avatar)
        {
            this.title = title;
            this.author = author;
            this.environment = environment;
            this.description = description;
            this.avatar = avatar;
        }
    }

    public static List<ModInfo> ParseModsPage(int page = 1)
    {
        driver.Navigate().GoToUrl($"https://modrinth.com/mods?page={page}");

        var mods = driver.FindElement(By.Id("search-results")).FindElements(By.XPath("./*"));

        List<ModInfo> modsList = new List<ModInfo>();
        foreach (var mod in mods)
        {
            string avatar = mod.FindElement(By.ClassName("avatar")).GetAttribute("src");
            string title = mod.FindElement(By.ClassName("title")).FindElements(By.TagName("a"))[0].Text;
            string author = mod.FindElement(By.ClassName("title")).FindElements(By.TagName("a"))[1].Text;
            string description = mod.FindElement(By.ClassName("description")).Text;
            string environment = mod.FindElement(By.ClassName("environment")).Text;

            modsList.Add(new ModInfo(title, author, environment, description, avatar));
        }
        return modsList;
    }

    private static void Main(string[] args)
    {
        int start = -1;
        int limit = -1;

        if (args.Length > 1)
        {
            if (args.Length == 4 || args.Length > 5)
            {
                Console.WriteLine("Wrong arguments!");
                return;
            }

            if (args.Length >= 3)
            {
                if (args[1] == "-s") int.TryParse(args[0], out start);
                else if (args[1] == "-l") int.TryParse(args[0], out limit);
            }

            if (args.Length == 5)
            {
                if (args[3] == "-s") int.TryParse(args[0], out start);
                else if (args[3] == "-l") int.TryParse(args[0], out limit);

                if (start < 1 && limit < 1)
                {
                    Console.WriteLine("Wrong arguments!");
                    return;
                }
            }
        }

        if (start < 1) start = 1;
        if (limit < 1) limit = 1;

        Console.WriteLine("Parsing mods list...");

        List<ModInfo> modsList = new List<ModInfo>();
        for (int i = start; i < start + limit; i++) {
            Console.WriteLine($"  [{i - start + 1}/{limit}] Parsing page #{i}");
            modsList.AddRange(ParseModsPage(i));
        }

        Console.WriteLine("Parsing ended!");

        driver.Dispose();

        Console.WriteLine("title, author, environment, description, avatar");
        foreach (ModInfo mod in modsList)
        {
            Console.WriteLine($"\"{mod.title}\", \"{mod.author}\", \"{mod.environment}\", {(mod.avatar == null ? "NULL" : "\"" + mod.avatar + "\"")}");
        }

        Console.ReadKey();
    }
}