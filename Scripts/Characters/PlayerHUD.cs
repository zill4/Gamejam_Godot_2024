using Godot;
using System;

public partial class PlayerHUD : Control
{

	private Label MessageLabel;
	private Timer RoundTimer;
	private Label WaveLabel;
	private Label TimerLabel;
    private Label CountdownLabel;


	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MessageLabel = GetNode<Label>("Message");
        WaveLabel = GetNode<Label>("Wave");
        TimerLabel = GetNode<Label>("Timer");
        CountdownLabel = GetNode<Label>("Countdown");
        CountdownLabel.Hide();
    }

    public void ShowMessage(string message)
    {
        MessageLabel.Text = message;
        MessageLabel.Show();
    }

    public void UpdateWaveLabel(int waveNumber)
    {
        WaveLabel.Text = $"Wave: {waveNumber}";
    }

    public void UpdateTimerLabel(float timeLeft)
    {
        TimerLabel.Text = $"Time: {timeLeft:F2}";
    }

    public void UpdateCountdownLabel(float timeLeft)
    {
        CountdownLabel.Text =  $"Next Round: {timeLeft:F2}";;
    }

    public void ToggleCountdownLabel()
    {
        if (CountdownLabel.Visible)
        {
            CountdownLabel.Hide();
        }
        else
        {
            CountdownLabel.Show();
        }
    }
}