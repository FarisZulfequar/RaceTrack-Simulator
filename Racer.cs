namespace RaceTrackSim;

/// <summary>
/// The racer class is the actual racer vehicle object blueprint. Where the racers would race around the track
/// </summary>
public class Racer
{
    #region Field Variables
    /// <summary>
    /// The X, Y coordinates of the racers when before start of the race
    /// </summary>
    private double _startPosition;

    /// <summary>
    /// I have no idea
    /// </summary>
    private double _location;
    
    /// <summary>
    /// The length of the total race track
    /// </summary>
    private double _raceTrackLength;

    /// <summary>
    /// The image of the racer to be moved around
    /// </summary>
    private Image _racerUI;
    
    /// <summary>
    /// I have no idea
    /// </summary>
    private static Random _random;

    #endregion

    #region Constructors
    /// <summary>
    /// Constructor for the Racer class
    /// </summary>
    /// <param name="racerUI"></param>
    public Racer(Image racerUI)
    {
        _startPosition = 0;
        _location = 0;
        _raceTrackLength = 0;
        _racerUI = racerUI;
        _random = new Random();
    }
    #endregion
    
    #region Properties

    public double Location
    {
        set { _location = value; }
    }

    public Image RacerUI => _racerUI;

    #endregion
    
    #region Methods

    /// <summary>
    /// Sets al the hamster x-coordinte to 0 when starting the race
    /// </summary>
    /// <param name="xcoord"></param>
    /// <param name="ycoord"></param>
    public void TakeStartingPosition(double xcoord, double ycoord)
    {
        // Sets Racer's X and Y coordinates set to 0,0, length and width to Auto
        AbsoluteLayout.SetLayoutBounds(RacerUI, new Rect(xcoord, ycoord, RacerUI.Width, RacerUI.Height));
    }
    
    /// <summary>
    /// Where each racer actually races based on their random double value
    /// </summary>
    /// <returns></returns>
    public bool Run()
    {
        // Calculates a value between 0.01 to 0.02
        double xcor = 0.01 + (_random.NextDouble() * (0.02 - 0.01));
        
        // Adds it to their location as of the current time in the race
        _location += xcor;
        
        // Sets it as their new coordinate for X
        AbsoluteLayout.SetLayoutBounds(RacerUI, new Rect(_location, 0, RacerUI.Width, RacerUI.Height));

        if (_location >= 1.0)
        {
            // If the x-coordinate is over or equal to 1.0, they reached the finish line
            return true;
        }
        // returns false if didn't
        return false;
    }
    
    #endregion


}