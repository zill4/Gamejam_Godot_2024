extends Control

@onready var v_box_container = $VBoxContainer
@onready var main_menu_settings = $main_menu_settings
@onready var tick = $tick

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _play_UI_tick():
	tick.play(0.0)

#BUTTONS
#BUTTONS
#BUTTONS
#BUTTONS

func _on_start_button_pressed():
	
	_play_UI_tick()
	get_tree().change_scene_to_file("res://scenes/_master_game.tscn")

func _on_settings_button_pressed():
	
	_play_UI_tick()
	v_box_container.visible = false
	main_menu_settings.visible = true

func _on_back_button_pressed():
	
	_play_UI_tick()
	main_menu_settings.visible = false
	v_box_container.visible = true

func _on_quit_button_pressed():
	get_tree().quit()

#VOLUME SLIDERS
#VOLUME SLIDERS
#VOLUME SLIDERS


func _on_master_slider_value_changed(value):
	_play_UI_tick()
	AudioServer.set_bus_volume_db (0, 0 + (value/log(10)))

func _on_music_slider_value_changed(value):
	_play_UI_tick()
	AudioServer.set_bus_volume_db (2, 0 + (value/log(10)))

func _on_sfx_slider_value_changed(value):
	_play_UI_tick()
	AudioServer.set_bus_volume_db (1, 0 + (value/log(10)))
