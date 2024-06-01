using Godot;
using System;
using System.Collections.Generic;

public partial class customer : CharacterBody3D
{
	private Vector3 _targetPosition;
	public float Speed = 5.0f;
	private const float TargetThreshold = 0.1f;
	public Order Order { get; private set; }

	private static readonly List<string[]> DrinkOptions = new List<string[]>
	{
		new string[] { "Espresso", "Milk" },
		new string[] { "Black Tea", "Sugar", "Lemon" },
		new string[] { "Green Tea", "Honey", "Ginger" },
		new string[] { "Latte", "Milk", "Vanilla" }
	};

	public override void _Ready()
	{
		GenerateRandomOrder();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = (_targetPosition - GlobalTransform.Origin).Normalized();

		if (GlobalTransform.Origin.DistanceTo(_targetPosition) < TargetThreshold)
		{
			// NPC has reached the target
			Velocity = Vector3.Zero;
		}
		else
		{
			// NPC is still moving towards the target
			Velocity = direction * Speed;
		}

		MoveAndSlide();
	}

	public void SetTargetPosition(Vector3 position)
	{
		_targetPosition = position;
	}

	private void GenerateRandomOrder()
	{
		Random rand = new Random();
		int index = rand.Next(DrinkOptions.Count);
		List<string> ingredients = new List<string>(DrinkOptions[index]);
		Drink drink = new Drink(ingredients);
		Order = new Order(drink);
		GD.Print("Generated Order: " + string.Join(", ", Order.Drink.Ingredients));
	}
}
