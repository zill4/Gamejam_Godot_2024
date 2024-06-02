using Godot;
using System;
using System.Collections.Generic;

public partial class OrderRequest : Node3D
{
	private Sprite3D _orderSprite;
	private const float SpriteScaleFactor = 0.1f; // Adjust this value to make the sprites smaller or larger
	private readonly Dictionary<string, string> orderTextures = new Dictionary<string, string>
	{
		{ "Mocha", "res://Assets/sprites/order icons/mocha.png"  },
		{ "Hazelnut Latte","res://Assets/sprites/order icons/hazelnut.png" },
		{ "Hot Chocolate", "res://Assets/sprites/order icons/cafe_au_lait.png" },
		{ "Espresso", "res://Assets/sprites/order icons/espresso.png" },
		{ "Latte","res://Assets/sprites/order icons/latte.png" }
	};


	public override void _Ready()
	{
		_orderSprite = GetNode<Sprite3D>("OrderSprite");
	}

	public void SetOrderSprite(string orderName)
	{
		if (orderTextures.ContainsKey(orderName))
		{
			_orderSprite.Texture = (Texture2D)GD.Load(orderTextures[orderName]);
			_orderSprite.Scale = new Vector3(SpriteScaleFactor, SpriteScaleFactor, SpriteScaleFactor);
			_orderSprite.Visible = true;
		}
	}

}
