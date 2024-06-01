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
	private Timer _countdownTimer;

	// Customer Variables
	public PackedScene CustomerScene;

	private Node3D _spawnPoint;
	private Node3D _targetPoints;
	public float SpawnInterval = 5.0f;
	private Timer _spawnTimer;
	private int currentNPCCount = 0;

	// Containers
	private Node _customerContainer;

	public override void _Ready()
	{
		// Initialize HUD
		_playerHUD = GetNode<PlayerHUD>("PlayerHud");
		_roundTimer = new Timer();
		AddChild(_roundTimer);
		_playerHUD.UpdateWaveLabel(CurrentWave);
		StartRoundTimer(TimeBetweenWaves);

		// Load the NPC scene (model/character)
		CustomerScene = (PackedScene)ResourceLoader.Load("res://Scenes/Characters/customer.tscn");

		// Containers
		_customerContainer = new Node();
		AddChild(_customerContainer);

		// reference to customer spawn point location.
		_spawnPoint = GetNode<Node3D>("CustomerSpawnPoint");
		_targetPoints = GetNode<Node3D>("TargetPoints");

		// Set up the spawn timer
		_spawnTimer = new Timer();
		_spawnTimer.WaitTime = SpawnInterval;
		_spawnTimer.OneShot = false;
		_spawnTimer.Connect("timeout", new Callable(this, nameof(OnSpawnTimerTimeout)));
		AddChild(_spawnTimer);
		_spawnTimer.Start();

		// Set up the round timer
		_roundTimer = new Timer();
		_roundTimer.WaitTime = 30.0f; // Set the duration for each round
		_roundTimer.OneShot = true;
		_roundTimer.Connect("timeout", new Callable(this, nameof(OnRoundTimerTimeout)));
		AddChild(_roundTimer);

		// Set up the countdown timer
		_countdownTimer = new Timer();
		_countdownTimer.WaitTime = 10.0f; // Set the countdown duration
		_countdownTimer.OneShot = true;
		_countdownTimer.Connect("timeout", new Callable(this, nameof(OnCountdownTimerTimeout)));
		AddChild(_countdownTimer);

		  // Start the first round
        StartRound();
	}

	private void StartRoundTimer(float time)
	{
		_roundTimer.WaitTime = time;
		_roundTimer.OneShot = true;
		_roundTimer.Connect("timeout", new Callable(this, nameof(OnRoundTimerTimeout)));
		_roundTimer.Start();
	}

	// private void OnRoundTimerTimeout()
	// {
	// 	GD.Print("Round ended, increasing difficulty");
	// 	IncreaseDifficulty();
	// 	SpawnWave();
	// 	StartRoundTimer(TimeBetweenWaves);
	// 	_playerHUD.UpdateWaveLabel(CurrentWave);
	// }

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

    private void StartRound()
    {
        GD.Print($"Starting round {CurrentWave}");
        _spawnTimer.Start();
        _roundTimer.Start();
    }

    private void OnSpawnTimerTimeout()
    {
        SpawnNPC();
    }

    private void OnRoundTimerTimeout()
    {
        GD.Print("Round ended");
        _spawnTimer.Stop();
        RemoveAllNPCs();
        _countdownTimer.Start();
    }

    private void OnCountdownTimerTimeout()
    {
        GD.Print("Starting next round");
        CurrentWave++;
		_playerHUD.UpdateWaveLabel(CurrentWave);
        StartRound();
    }

	private void IncreaseDifficulty()
	{
		CurrentWave++;
		// Increase enemy properties for next wave
		// This is a basic example; you could also increase other attributes like health, speed, etc.
	}

	private void SpawnNPC()
	{
		if (currentNPCCount >= _targetPoints.GetChildCount())
		{
			GD.Print("All target points are occupied");
			return;
		}

		var customerInstance = (customer)CustomerScene.Instantiate();

		// THE ORDER OF THIS CODE MATTERS

		// Set NPC position to spawn point
		customerInstance.GlobalTransform = _spawnPoint.GlobalTransform;

		// Scale down the NPC
		customerInstance.Scale = new Vector3(0.5f, 0.5f, 0.5f);


		// Set target position for the NPC
		var targetPoint = _targetPoints.GetChild<Node3D>(currentNPCCount % _targetPoints.GetChildCount());

		GD.Print($"Setting Target Point: {targetPoint.Name} {targetPoint.GlobalTransform.Origin} TargetPoints Count: {_targetPoints.GetChildCount()}  TargetPoints Name: {_targetPoints.Name}");
		customerInstance.SetTargetPosition(targetPoint.GlobalTransform.Origin);

		_customerContainer.AddChild(customerInstance);
		currentNPCCount++;
	}

	private void RemoveAllNPCs()
    {
        foreach (Node child in _customerContainer.GetChildren())
        {
            child.QueueFree();
        }
        currentNPCCount = 0;
    }
}
