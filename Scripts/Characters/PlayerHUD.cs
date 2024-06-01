using Godot;
using System;

public partial class PlayerHUD : Control
{

    private Label MessageLabel;
    private Timer RoundTimer;
    private Label WaveLabel;
    private Label TimerLabel;
    private Label CountdownLabel;
    private Label PromptLabel;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MessageLabel = GetNode<Label>("Message");
        WaveLabel = GetNode<Label>("Wave");
        TimerLabel = GetNode<Label>("Timer");
        CountdownLabel = GetNode<Label>("Countdown");
        PromptLabel = GetNode<Label>("Prompt");
        CountdownLabel.Hide();
        PromptLabel.Hide();
    }

    public void ShowPrompt(string prompt)
    {
        PromptLabel.Text = prompt;
        PromptLabel.Show();
    }

    public void HidePrompt()
    {
        PromptLabel.Hide();
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
        CountdownLabel.Text = $"Next Round: {timeLeft:F2}"; ;
    }

    public void ShowCountdownLabel()
    {

        CountdownLabel.Show();

    }

    public void HideCountdownLabel()
    {
        CountdownLabel.Hide();

    }

}