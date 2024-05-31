extends Node3D

@onready var timer = $Timer

var rat_speed = 5.0
var rat_health = 5.0


func _ready():
	rat_health = 5.0
	rat_speed = 5.0


func rat_take_damage(amount):
	rat_health -= amount
	
	if rat_health == 0:
		on_rat_death()


func on_rat_death():
	timer.start()



func _on_timer_timeout():
	queue_free()
