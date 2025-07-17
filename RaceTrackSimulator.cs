namespace RaceTrackSim;

/// <summary>
/// The class where the race simulates between the racers
/// </summary>
public class RaceTrackSimulator
{
    #region Field Variables
    /// <summary>
    /// List of the all bettors
    /// </summary>
    private List<Bettor> _bettorList;
    
    /// <summary>
    /// Current bettor
    /// </summary>
    private Bettor _crtbettor;
    
    /// <summary>
    /// List of racers
    /// </summary>
    private List<Racer> _racerList;
    #endregion
    
    #region Constructor
    /// <summary>
    /// Constructor of the RaceTrackSimulator class
    /// </summary>
    public RaceTrackSimulator()
    {
        _bettorList = new List<Bettor>();
        _racerList = new List<Racer>();
    }
    #endregion

    #region Properties
    public List<Bettor> BettorList
    {
        get
        {
            return _bettorList;
        }
    }

    public List<Racer> RaceList
    {
        get
        {
            return _racerList;
        }
    }

    public Bettor CurrentBettor
    {
        get
        {
            return _crtbettor;
        }
        set
        {
            _crtbettor = value;
        }
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Registers a racer
    /// </summary>
    /// <param name="racer"></param>
    public void RegisterRacer(Racer racer)
    {
        _racerList.Add(racer);
    }
    
    /// <summary>
    /// Adds a bettor
    /// </summary>
    /// <param name="bettor"></param>
    public void AddBettor(Bettor bettor)
    {
        _bettorList.Add(bettor);
    }
    #endregion
}