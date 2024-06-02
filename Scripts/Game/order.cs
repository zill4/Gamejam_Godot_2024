using Godot;
using System;
using System.Collections.Generic;

public class Drink
{
    public List<string> Ingredients { get; set; }
    public string Name { get; set; }

    public Drink(List<string> ingredients, string name)
    {
        Ingredients = ingredients;
        Name = name;
    }
}

public enum OrderStatus
{
		WAITING,
		COMPLETED,
		FAILED
}

public class Order
{
    public OrderStatus Status { get; set; }
    public Drink Drink { get; set; }

    public Order(Drink drink)
    {
        Status = OrderStatus.WAITING;
        Drink = drink;
    }
}
