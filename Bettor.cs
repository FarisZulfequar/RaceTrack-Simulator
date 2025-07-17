namespace RaceTrackSim;

/// <summary>
/// The class where each user would place bets on a racer 
/// </summary>
public class Bettor
{
    #region Field Variables
    /// <summary>
    /// The bettor's name
    /// </summary>
    private string _name;
    
    /// <summary>
    /// The bettor's current cash
    /// </summary>
    private double _cash;
    
    /// <summary>
    /// The Bettor's bet
    /// </summary>
    private Bet _bet;

    /// <summary>
    /// Have No Idea
    /// </summary>
    private Entry _betDescUI;

    private RadioButton _bettorUI;

    private int _minimumBetAmount;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructor of the Bettor class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cash"></param>
    /// <param name="bettorUI"></param>
    /// <param name="bestDescUI"></param>
    public Bettor(string name, double cash, RadioButton bettorUI, Entry betDescUI)
    {
        _name = name;
        _cash = cash;
        _betDescUI = betDescUI;
        _bettorUI = bettorUI;
        _minimumBetAmount = 5;
    }
    #endregion
    
    #region Properties

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public double Cash
    {
        get { return _cash; }
        set
        {
            if (value >= _minimumBetAmount && value <= Cash)
            { _cash = value; }
            else { _cash = 0; }
        }
    }
    
    /// <summary>
    /// If the user placed a bet 
    /// </summary>
    public bool HasPlaceBet
    {
        get
        {
            // A bettor didn't place a bet if their bet description is null/whitespaces
            if (String.IsNullOrWhiteSpace(_betDescUI.Text))
            {
                return false;
            }
            // If a bettor did place a bet it returns true
            return true;
        }
    }

    public RadioButton BettorUI
    {
        get { return _bettorUI; }
        set { _bettorUI = value; }
    }

    public Entry BetDescUI
    {
        get { return _betDescUI; }
        set { _betDescUI = value; }
    }

    public Bet Bet
    {
        get { return _bet; }
        set { _bet = value; }
    }

    public int MinimumBetAmount
    {
        get { return _minimumBetAmount; }
        set { _minimumBetAmount = value; }
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// The feature of the user placing a bet
    /// </summary>
    /// <param name="betAmount"></param>
    /// <param name="RacerNoToWin"></param>
    /// <returns></returns>
    public bool PlaceBet(double betAmount, int RacerNoToWin)
    {
        if (betAmount == 0)
        {
            // Throws an exception due to not having a Bettor
            throw new RaceSimulatorException("Please provide Hamster Coins from the Minimum to your current Hamster Coins");
        }
        else if (RacerNoToWin == 0)
        {
            // Throws an exception due to not having a Bettor
            throw new RaceSimulatorException("Please select a existing Hamster Racer");
        }
        
        // The Bet was successful to be placed
        return true;
    }

    /// <summary>
    /// Clears the user's current bet
    /// </summary>
    public void ClearBet()
    {
        // TODO: Implement Method
    }

    /// <summary>
    /// Have no idea
    /// </summary>
    /// <param name="winnerNo"></param>
    public void Collect(int winnerNo)
    {
        double betpayout = _bet.PayOut(winnerNo);
        
        _cash += betpayout;
        
    }

    /// <summary>
    /// Have no idea
    /// </summary>
    /// <param name="winnerNo"></param>
    public void UpdateLabels()
    {
        _betDescUI.Text = $"{_name} Cheered with {_bet.Amount}. New Hamster Coin: {_cash}";
        _bet.AddBetToList(_betDescUI.Text);
    }
    
    public double ValidateBetAmount(string currentBetAmount, Bettor crtBettor)
    {
        // Variable to hold the parsed value temporarily
        double parsedBetAmount;

        // Tries to parse the amount of Hamster Coins
        bool isValidBetAmount = Double.TryParse(currentBetAmount, out parsedBetAmount);

        // If the _txtBettorAmount null/whitespaces or the parsing failed sets the cash to 0
        if (String.IsNullOrEmpty(currentBetAmount) || isValidBetAmount == false)
        {
            parsedBetAmount = 0;
            return parsedBetAmount;
        }
        else if (parsedBetAmount < _minimumBetAmount)
        {
            parsedBetAmount = 0;
            return parsedBetAmount;
        }
        // If successful sets it to the Bettor cash
        return parsedBetAmount;
    }
    
    public int ValidateRacer(string hamsterRacerName)
    {
        if (hamsterRacerName == "Puffy")
        {
            // Bettor chose Racer Puffy
            return 1;
        }
        else if (hamsterRacerName == "Rolly")
        {
            // Bettor chose Racer Rolly
            return 2;
        }
        else if (hamsterRacerName == "Tofu")
        {
            // Bettor chose Racer Tofu
            return 3;
        }
        else if (hamsterRacerName == "Whiskers")
        {
            // Bettor chose Racer Whiskers
            return 4;
        }
        // Otherwise it wasn't a valid racer
        return 0;
    }
    #endregion


}