extends Control

@onready var settings = $settings
@onready var tick = $tick

@onready var main_master_slider = $main_menu_settings/master_slider
@onready var main_sfx_slider = $main_menu_settings/sfx_slider
@onready var main_music_slider = $main_menu_settings/music_slider

@onready var player_master_slider = $master_slider
@onready var player_sfx_slider = $sfx_slider
@onready var player_music_slider = $music_slider

func _ready():
	#get set current audio levels
	print("settings ready")
	settings.visible = false
	
	AudioServer.set_bus_volume_db(0,AudioServer.get_bus_volume_db(0))
	AudioServer.set_bus_volume_db(1,AudioServer.get_bus_volume_db(1))
	AudioServer.set_bus_volume_db(2,AudioServer.get_bus_volume_db(2))	
	
	#set slider values to main menu values
	player_master_slider = main_master_slider
	player_sfx_slider = main_sfx_slider
	player_music_slider = main_music_slider

func _play_UI_tick():
	tick.play(0.0)


	if (Input.is_action_just_released("esc")):
		settings.visible = true
		print("opened settings")

func _on_back_button_pressed():
	
	_play_UI_tick()
	settings.visible = false

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

