namespace RaceTrackSim;

public partial class SimulationAnalyzePage : ContentPage
{
    private MainPage _mainPage;
    
    private List<string> _allBets;
    
    public SimulationAnalyzePage(MainPage mainPage)
    {
        _allBets = new List<string>();
        _mainPage = mainPage;
        InitializeComponent();
        
    }

    public void ShowingAllBettorsBets(object sender, EventArgs e)
    {
        _allBets.Clear();
        
        foreach (Bettor bettor in RaceTrackSimulator.BettorList)
        {
            if (bettor.Bet.BetListAsString != null)
            {
                foreach (string bet in bettor.Bet.BetListAsString)
                {
                    _allBets.Add($"{bettor.Name}: {bet}");
                }
            }
        }
        
        _lstBettorBet.ItemsSource = null;
        _lstBettorBet.ItemsSource = _allBets; 
    }
    
}