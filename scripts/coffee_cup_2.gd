extends Node3D

var coffee_plain_mat = preload("res://assets/materials/coffee_plain.tres")

func _ready():
	set_empty_cup()
	

func set_empty_cup():
	coffee_plain_mat.albedo_color = Color(0,0,0,0)
	print("cups are empty")

func add_coffee():
	coffee_plain_mat.albedo_color = Color(58,25,3,255)
	print("added coffee")

func change_coffee_color():
	coffee_plain_mat.albedo_color = Color(139,70,18,255)
	print("changed to latte")
