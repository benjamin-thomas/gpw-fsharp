type FootballResult =
    {
        HomeTeam: string
        AwayTeam: string
        HomeGoals: int
        AwayGoals: int
    }

let create (ht, hg) (at, ag) =
    {
        HomeTeam = ht
        AwayTeam = at
        HomeGoals = hg
        AwayGoals = ag
    }

let results =
    [
        create ("Messiville", 1) ("Ronaldo City", 2)
        create ("Messiville", 1) ("Bale Town", 3)
        create ("Bale Town", 3) ("Ronaldo City", 1)
        create ("Bale Town", 3) ("Messiville", 1)
        create ("Ronaldo City", 4) ("Messiville", 2)
        create ("Ronaldo City", 1) ("Bale Town", 2)
    ]

type Wins = { Team: string; score: int }

// Show me which teams won the most away games in the season
let isAwayWin result = result.AwayGoals > result.HomeGoals

results
|> List.filter isAwayWin
|> List.map (fun res -> res.AwayTeam)
|> List.countBy id
|> List.sortByDescending snd
|> List.map (fun (city, wins) -> $"- %s{city}: %d{wins}")
|> String.concat "\n"
