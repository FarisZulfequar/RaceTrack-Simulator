using System.Collections.ObjectModel;
using Plugin.Maui.Audio;
namespace RaceTrackSim;

/// <summary>
/// The class where it connects racetrack UI to the user 
/// </summary>
public partial class MainPage : ContentPage
{
	#region Field Variables
	/// <summary>
	/// Holds the 4 _imgRacer images I think
	/// </summary>
	private List<Image> _racerImageList;

	private RaceTrackSimulator _raceTrackSim;

	private double _xcor = 0.00;
	
	// A timer interval for each racer to have
	private IDispatcherTimer _raceInterval;

	private IAudioPlayer _audioPlayer;
	#endregion
	
	#region Constructor
	/// <summary>
	/// The constructor for the MainPage
	/// </summary>
	public MainPage()
	{
		_racerImageList = new List<Image>();
		_raceTrackSim = new RaceTrackSimulator();
		_raceInterval = Dispatcher.CreateTimer();
		_raceInterval.Interval = TimeSpan.FromMilliseconds(400);
		_raceInterval.Tick += OnRaceTimerTick;
		
		InitializeComponent();
		
		// Initializes the 3 Bettors
		CreateBettors();
		
		// Initializes the 4 Racers
		CreateRacers();
	}

	#endregion
	
	#region Properties

	public RaceTrackSimulator RaceTrackSim => _raceTrackSim;

	public string LblMinimumCheerTitle
	{
		set {_lblMinimumCheerTitle.Text = value; }
	}

	public IDispatcherTimer RaceInterval => _raceInterval;

	#endregion

	#region Methods
	
	/// <summary>
	/// Creates the Four Racers
	/// </summary>
	private void CreateRacers()
	{
		
		// Creates the 4 Hamster Racer
		Racer firstRacer = new Racer(_imgRacer1);
		Racer secondRacer = new Racer(_imgRacer2);
		Racer thirdRacer = new Racer(_imgRacer3);
		Racer fourthRacer = new Racer(_imgRacer4);
		
		// Adds them into the racerList
		_raceTrackSim.RegisterRacer(firstRacer);
		_raceTrackSim.RegisterRacer(secondRacer);
		_raceTrackSim.RegisterRacer(thirdRacer);
		_raceTrackSim.RegisterRacer(fourthRacer);
		
		// Adds the images into the raceImageList
		_racerImageList.Add(_imgRacer1);
		_racerImageList.Add(_imgRacer2);
		_racerImageList.Add(_imgRacer1);
		_racerImageList.Add(_imgRacer2);
	}
	
	/// <summary>
	/// Creates the Three Bettors
	/// </summary>
	private void CreateBettors()
	{
		// Creates the 3 Bettors. As of now, the name and cash is a constant
		Bettor firstBettor = new Bettor("Nibble", 100, _rbtnBettor1, _txtBet1);
		Bettor secondBettor = new Bettor("Fluff", 100, _rbtnBettor2, _txtBet2 );
		Bettor thirdBettor = new Bettor("Squeak", 100, _rbtnBettor3, _txtBet3);
		
		// Adds the bettors into the Race Track list
		_raceTrackSim.AddBettor(firstBettor);
		_raceTrackSim.AddBettor(secondBettor);
		_raceTrackSim.AddBettor(thirdBettor);
	}

	private Bettor OnSelectBettor()
	{
		// Goes through the Bettor List for each Bettor
		foreach (Bettor bettor in _raceTrackSim.BettorList)
		{
			// If any of their RadioButton is checked returns the Bettor
			if (bettor.BettorUI.IsChecked)
			{
				return bettor;
			}
		}
		// Throws an SimulatorException if there was no Bettor
		throw new RaceSimulatorException("Hamster Fan Not Selected. Please select a Hamster Fan before placing a Cheer");
	}
	
	
	private void OnPlaceBet(object sender, EventArgs e)
	{
		// Tries to place a bet
		try
		{
			// Have a method that sets the currentBettor based on the checked radiobutton
			_raceTrackSim.CurrentBettor = OnSelectBettor();

			// Validate the Bettor cash and bet on the racer
			double betAmount = _raceTrackSim.CurrentBettor.ValidateBetAmount(_txtBettorCash.Text);

			// Validating if the Bettor chose an actual Racer
			string pickedRacer = _pckHamsterRacers.SelectedItem?.ToString();
			int validRacer = _raceTrackSim.CurrentBettor.ValidateRacer(pickedRacer);

			// Sees if Bettor can place a bet
			bool isValidBet = _raceTrackSim.CurrentBettor.PlaceBet(betAmount, validRacer);

			if (isValidBet)
			{
				// Sets the Bet Description
				_raceTrackSim.CurrentBettor.BetDescUI.Text = $"{_raceTrackSim.CurrentBettor.Name} Cheers For {pickedRacer} " +
				                                             $"for {betAmount} Hamster Coins";
				// Places the Bet for the Bettor
				_raceTrackSim.CurrentBettor.Bet = new Bet(betAmount, validRacer, _raceTrackSim.CurrentBettor);
				// Stops the Bettor from going back on its bet, like a true Bettor
				_raceTrackSim.CurrentBettor.BettorUI.IsEnabled = false;
			}
		}
		catch (RaceSimulatorException ex)
		{
			DisplayAlert("Simulation Race Error", ex.Message, "OK");
		}
		catch (Exception ex)
		{
			DisplayAlert("Simulation Race Error", "An error has occured where the program has not expected" +
			    " please try again", "OK");
		}
	}
	
	private async void OnStartRace(object sender, EventArgs e)
	{
		// For each racer makes their location value to 0 so the xcoordinate is set to 0
		foreach (Racer racer in _raceTrackSim.RaceList)
		{ racer.Location = 0; }
		
		SelectingAllRacersToStart();
		// Use a loop to check if all the bettors have placed a bet
		bool isReadyToRace = CheckAllBettorsBet();

		if (isReadyToRace)
		{
			
			// Starts the actual race
			_audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("HamsterRaceMusic.mp3"));
			_raceInterval.Start();
			_imgStartRaceButton.IsEnabled = false;
			_btnPlaceBet.IsEnabled = false;
			_txtBettorCash.IsEnabled = false;
			_pckHamsterRacers.IsEnabled = false;
			ToggleMusic();

		}
		else
		{
			DisplayAlert("All Hamster Fans Hasn't Cheered", "Please have all Hamster Fans place a Cheer", 
				"OK");
		}

	}

	private bool CheckAllBettorsBet()
	{
		// Goes through the Bettor List for each Bettor
		foreach (Bettor bettor in _raceTrackSim.BettorList)
		{
			// If any of their RadioButton is checked returns the Bettor
			if (bettor.HasPlaceBet == false)
			{
				// There was at least one Bettor who hasn't bet yet
				return false;
			}
		}
		// All Bettors did bets
		return true;
	}


	private void OnRaceTimerTick(object sender, EventArgs e)
	{
		// Retrieves each racer from the list
		Racer racer1 = _raceTrackSim.RaceList[0];
		Racer racer2 = _raceTrackSim.RaceList[1];
		Racer racer3 = _raceTrackSim.RaceList[2];
		Racer racer4 = _raceTrackSim.RaceList[3];
		
		// Makes each racer run the track. If one of them finishes returns a true value
		bool Isracer1Win = racer1.Run();
		bool Isracer2Win = racer2.Run();
		bool Isracer3Win = racer3.Run();
		bool Isracer4Win = racer4.Run();
		
		if (Isracer1Win)
		{
			// If racer 1 finished first, they won the race
			DisplayAlert("Racer 1 Win", "Puffy has won the Hamster Race!", "OK");
			DisplayBetReviewDashBoard(1);
			stopRace();
		}
		else if (Isracer2Win)
		{
			// If racer 2 finished first, they won the race
			DisplayAlert("Racer 2 Win", "Rolly has won the Hamster Race!", "OK");
			DisplayBetReviewDashBoard(2);
			stopRace();
		}
		else if (Isracer3Win)
		{
			// If racer 3 finished first, they won the race
			DisplayAlert("Racer 3 Win", "Tofu has won the Hamster Race!", "OK");
			DisplayBetReviewDashBoard(3);
			stopRace();
		}
		else if (Isracer4Win)
		{
			// If racer 4 finished first, they won the race
			DisplayAlert("Racer 4 Win", "Whiskers has won the Hamster Race!", "OK");
			DisplayBetReviewDashBoard(4);
			stopRace();
		}
	}

	private void stopRace()
	{
		_raceInterval.Stop();
		_btnRestartRace.IsEnabled = true;
		ToggleMusic();
	}

	private void SelectingAllRacersToStart()
	{
		// Retrieves all the Racers in the list
		foreach (Racer racer in _raceTrackSim.RaceList)
		{
			// For each racer, sets their X & Y coordinates to 0
			racer.TakeStartingPosition(0,0);
		}
	}

	private void DisplayBetReviewDashBoard(int racerWonNo)
	{
		foreach (Bettor bettor in _raceTrackSim.BettorList)
		{
			bettor.Collect(racerWonNo);
			bettor.UpdateLabels();
		}
	}

	private void OnRaceRestartClicked(object sender, EventArgs e)
	{
		foreach (Bettor bettor in _raceTrackSim.BettorList)
		{
			bettor.BetDescUI.Text = null;
			bettor.BetDescUI.Placeholder = $"{bettor.Name}'s Cuteness Cheer";
			bettor.BettorUI.IsEnabled = true;
			_btnRestartRace.IsEnabled = false;
			_imgStartRaceButton.IsEnabled = true;
			_btnPlaceBet.IsEnabled = true;
			_txtBettorCash.IsEnabled = true;
			_pckHamsterRacers.IsEnabled = true;
		}
	}
	
	public void ToggleMusic()
	{
		// Play the audio
		if (_audioPlayer.IsPlaying) { _audioPlayer.Stop(); }
		else { _audioPlayer.Play(); }
		_audioPlayer.Volume = 0.1f;
	}
	
	#endregion
	
}

