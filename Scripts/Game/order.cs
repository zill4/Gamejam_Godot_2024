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

    public bool CheckIngredients(List<string> ingredients)
    {
        HashSet<string> playerIngredientsSet = new HashSet<string>(ingredients);
        HashSet<string> customerIngredientsSet = new HashSet<string>(Ingredients);

        // Check if playerIngredientsSet contains all elements of customerIngredientsSet
        GD.Print("Player Ingredients: " + string.Join(", ", playerIngredientsSet));
        GD.Print("Customer Ingredients: " + string.Join(", ", customerIngredientsSet));
        GD.Print($"Player Ing: {customerIngredientsSet.IsSubsetOf(playerIngredientsSet)}");
        return customerIngredientsSet.IsSubsetOf(playerIngredientsSet);
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
