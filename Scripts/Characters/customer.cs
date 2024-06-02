using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class customer : CharacterBody3D
{
	private Vector3 _targetPosition;
	public float Speed = 5.0f;
	private const float TargetThreshold = 0.1f;
	public Order Order { get; private set; }
	
	public OrderRequest _orderRequestSprite;


	private readonly Dictionary<string, string[]> _drinks = new Dictionary<string, string[]>
	{
		{ "Mocha", new string[] { "espresso", "milk", "mocha", "cup" } },
		{ "Hazelnut Latte", new string[] { "espresso", "milk", "hazelnut", "cup" } },
		{ "Hot Chocolate", new string[] { "milk", "cup", "mocha"} },
		{ "Espresso", new string[] { "espresso", "cup" } },
		{ "Latte", new string[] { "espresso", "milk", "cup" } }
	};

	public override void _Ready()
	{
		_orderRequestSprite = GetNode<OrderRequest>("OrderRequest");
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
    public void DeleteCustomer()
    {
        QueueFree();
    }

	public Order getOrder()
	{
		return Order;
	}

	public void SetStatus(OrderStatus status)
	{
		Order.Status = status;
	}

	private void GenerateRandomOrder()
	{
		Random rand = new Random();
		int index = rand.Next(_drinks.Count);
		List<string> ingredients = new List<string>(_drinks[_drinks.Keys.ElementAt(index)]);
		Drink drink = new Drink(ingredients, _drinks.Keys.ElementAt(index));
		// set Order
		Order = new Order(drink);
		// GD.Print("Generated Order: " + string.Join(", ", Order.Drink.Ingredients) + " - " + Order.Drink.Name);

		// set sprite
		_orderRequestSprite.SetOrderSprite(_drinks.Keys.ElementAt(index));
	}
}
