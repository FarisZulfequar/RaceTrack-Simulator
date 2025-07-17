using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceTrackSim;

public partial class SimulationOptionPage : ContentPage
{
    private MainPage _mainPage;
    public SimulationOptionPage(MainPage mainPage)
    {
        _mainPage = mainPage;
        InitializeComponent();
    }

    private void ChangeFirstBettorName(object sender, EventArgs e)
    {
        string name = _txtFirstBettorName.Text;
        Bettor bettor = _mainPage.RaceTrackSim.BettorList[0];
        bettor.Name = name;
        bettor.BettorUI.Content = $"{name}'s Button";
        bettor.BetDescUI.Placeholder = $"{name}'s Cuteness Cheer";
    }
    
    private void ChangeSecondBettorName(object sender, EventArgs e)
    {
        string name = _txtSecondBettorName.Text;
        Bettor bettor = _mainPage.RaceTrackSim.BettorList[1];
        bettor.Name = name;
        bettor.BettorUI.Content = $"{name}'s Button";
        bettor.BetDescUI.Placeholder = $"{name}'s Cuteness Cheer";
    }
    
    private void ChangeThirdBettorName(object sender, EventArgs e)
    {
        string name = _txtThirdBettorName.Text;
        Bettor bettor = _mainPage.RaceTrackSim.BettorList[2];
        bettor.Name = name;
        bettor.BettorUI.Content = $"{name}'s Button";
        bettor.BetDescUI.Placeholder = $"{name}'s Cuteness Cheer";
    }

    private void ChangeMinimumBetAmount(object sender, EventArgs e)
    {
        bool isValidMinimumBetAmount;
        try
        {
            int minimumbetAmount = ValidateMinimumBetAmount(_txtMinimumBet.Text, out isValidMinimumBetAmount);

            if (isValidMinimumBetAmount)
            {
                foreach (Bettor bettor in _mainPage.RaceTrackSim.BettorList)
                {
                    bettor.MinimumBetAmount = minimumbetAmount;
                }
            }
            
            _mainPage.LblMinimumCheerTitle = $"Minimum Cheer is: {minimumbetAmount}";
            
            DisplayAlert("Minimum Hamster Coin Changed", "The Minimum Cheer with Hamster Coins has been " +
                "successfully changed.", "OK");
            
        }
        catch (OptionSimulatorException ex)
        {
            DisplayAlert("Invalid Hamster Coin Amount", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            
        }
    }

    private int ValidateMinimumBetAmount(string minimumBetAmount, out bool isValidMinimumBetAmount)
    {
        int intMinimumBetAmount;

        if (int.TryParse(minimumBetAmount, out intMinimumBetAmount) == false)
        {
            throw new OptionSimulatorException("Please enter a valid Hamster Coin Amount as Integer.");
        }
        
        isValidMinimumBetAmount = true;
        return intMinimumBetAmount;
    }

    private void ChangeSimulationSpeed(object sender, EventArgs e)
    {
        Button simulationSpeedButton = (Button)sender;

        switch (simulationSpeedButton.Text)
        {
            case "Slow":
                _mainPage.RaceInterval.Interval = TimeSpan.FromMilliseconds(600);
                break;
            
            case "Medium":
                _mainPage.RaceInterval.Interval = TimeSpan.FromMilliseconds(400);
                break;
            
            case "Fast":
                _mainPage.RaceInterval.Interval = TimeSpan.FromMilliseconds(200);
                break;
            
            case "FULL SEND!":
                _mainPage.RaceInterval.Interval = TimeSpan.FromMilliseconds(50);
                break;
            
            default:
                throw new OptionSimulatorException("Please enter a valid Hamster Coin Speed.");
            
        }
    }
}