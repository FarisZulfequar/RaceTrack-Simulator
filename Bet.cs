namespace RaceTrackSim;
using System.Collections.ObjectModel;
/// <summary>
/// The actual bet itself that the bettor makes
/// </summary>
public struct Bet
{
    #region Field Variables
    /// <summary>
    /// The amount of money the bettor placed
    /// </summary>
    private double _amount;

    /// <summary>
    /// The racerNo that the Bettor placed on
    /// </summary>
    private int _racerNo;

    private Bettor _crtBettor;
    
    private ObservableCollection<string> _betListAsString = new ObservableCollection<string>();
    #endregion

    #region Constructor
    
    /// <summary>
    /// Constructor of the Bet class
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="racerNo"></param>
    /// <param name="bettor"></param>
    public Bet(double amount, int racerNo, Bettor bettor)
    {
        _amount = amount;
        _racerNo = racerNo;
        _crtBettor = bettor;
        _betListAsString = new ObservableCollection<string>();
    }

    public void AddBetToList(string betReview)
    {
        _betListAsString.Add(betReview);
    }

    #endregion
    
    #region Properties

    public string Description
    {
        get
        {
            return $"Bet {_amount} on racer {_racerNo}";
        }
    }

    public double Amount
    {
        get { return _amount; }
    }

    public int RacerNo
    {
        get { return _racerNo; }
    }

    public ObservableCollection<string> BetListAsString
    {
        get { return _betListAsString; }
    }
    #endregion
    
    #region Methods

    /// <summary>
    /// The amount of money that the winner gets from the bet
    /// </summary>
    /// <param name="winner"></param>
    /// <returns></returns>
    public double PayOut(int winner)
    {
        if (_crtBettor.Bet._racerNo == winner)
        {
            // Returns the amount as a positive due them winning
            return _amount;
        }
        // Returns the amount as a negative due them losing
        return -_amount;
    }
    
    #endregion
}