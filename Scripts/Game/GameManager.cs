using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public PackedScene EnemyScene;
	public int CurrentWave = 1;
	public float TimeBetweenWaves = 25.0f;
	private float waveTimer = 0.0f;

	private PlayerHUD _playerHUD;
	private Timer _roundTimer;

	public override void _Ready()
	{
		// EnemyScene = (PackedScene)ResourceLoader.Load("res://Enemy.tscn");
		_playerHUD = GetNode<PlayerHUD>("PlayerHud"); // Adjust the path as necessary
		GD.Print($"PlayerHUD: {_playerHUD.Name}");
		// Initialize HUD
		_roundTimer = new Timer();
		AddChild(_roundTimer);
		_playerHUD.UpdateWaveLabel(CurrentWave);
		StartRoundTimer(TimeBetweenWaves);
	}

	private void StartRoundTimer(float time)
	{
		_roundTimer.WaitTime = time;
		_roundTimer.OneShot = true;
		_roundTimer.Connect("timeout", new Callable(this, nameof(OnRoundTimerTimeout)));
		_roundTimer.Start();
	}

	private void OnRoundTimerTimeout()
	{
		GD.Print("Round ended, increasing difficulty");
		IncreaseDifficulty();
		SpawnWave();
		StartRoundTimer(TimeBetweenWaves);
		_playerHUD.UpdateWaveLabel(CurrentWave);
	}

	public override void _Process(double delta)
	{
		// waveTimer += (float)delta;
		// if (waveTimer >= TimeBetweenWaves)
		// {
		// 	// GD.Print("Meow new wave time? wave");
		// 	waveTimer = 0.0f;
		// 	SpawnWave();
		// 	IncreaseDifficulty();
		// }

		if (_roundTimer.TimeLeft > 0)
		{
			_playerHUD.UpdateTimerLabel((float)_roundTimer.TimeLeft);
		}
	}

	private void SpawnWave()
	{
		GD.Print("Meow Spawning wave");
	}

	private void IncreaseDifficulty()
	{
		CurrentWave++;
		// Increase enemy properties for next wave
		// This is a basic example; you could also increase other attributes like health, speed, etc.
	}
}
