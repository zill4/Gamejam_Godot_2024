using Godot;
using System;
using System.Collections.Generic;

public class Drink
{
    public List<string> Ingredients { get; set; }

    public Drink(List<string> ingredients)
    {
        Ingredients = ingredients;
    }
}

public enum OrderStatus
{
    Pending,
    Completed,
    Failed
}

public class Order
{
    public OrderStatus Status { get; set; }
    public Drink Drink { get; set; }

    public Order(Drink drink)
    {
        Status = OrderStatus.Pending;
        Drink = drink;
    }
}
