using System.Text.Json;
using System.Threading.Tasks;


public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int numberPages = GoalBalanceClientAsync(team, year, 1).Result.total_pages;

        int goals = 0;

        for (int i = 1; i <= numberPages; i++)
        {
            var response = GoalBalanceClientAsync(team, year, i).Result;

            goals += (from p in response.data
                          select int.Parse(p.team1goals)).Sum();
                   
        }       

        return goals;
    }

    public static async Task<ApiResponse> GoalBalanceClientAsync(string team, int year, int page)
    {
        string apiUrl = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
        var result = new ApiResponse();

        try
        {
            
            using (HttpClient client = new HttpClient())
            {
               
                HttpResponseMessage response = await client.GetAsync(apiUrl);             

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                                     
                    result = JsonSerializer.Deserialize<ApiResponse>(responseBody);

                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }

        return result;
    }

}

public class FootballMatch
{
    public string competition { get; set; }
    public int year { get; set; }
    public string round { get; set; }
    public string team1 { get; set; }
    public string team2 { get; set; }
    public string team1goals { get; set; }
    public string team2goals { get; set; }
}

public class ApiResponse
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<FootballMatch> data { get; set; }
}