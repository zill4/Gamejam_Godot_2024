using Godot;
using System;

public partial class PlayerHUD : Control
{

	private Label MessageLabel;
	private Timer RoundTimer;
	private Label WaveLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MessageLabel = GetNode<Label>("Message");
		RoundTimer = GetNode<Timer>("RoundTimer");
		WaveLabel = GetNode<Label>("Wave");
	}

	public void ShowMessage(string message)
	{
		MessageLabel.Text = message;
		MessageLabel.Show();
	}
}
