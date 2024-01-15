extends Node3D

@export_group("Worlds List")
@export var Hub1: PackedScene = preload("res://levels/world.tscn")
@export var Level1: PackedScene = preload("res://levels/level1.tscn")
@export var test1: PackedScene
#@export var Level2: PackedScene = preload("res://levels/level2.tscn")

func _process(_delta):
	if (Input.is_action_just_pressed("hub world")):
		get_tree().change_scene_to_packed(Hub1)
		print("Loading %s..." % Hub1)
	if (Input.is_action_just_pressed("level 1")):
		get_tree().change_scene_to_packed(Level1)
		print("Loading %s..." % Level1)
#	if (Input.is_action_just_pressed("hub world")):
#		get_tree().change_scene_to_packed(Level1)
#		print("Loading hub world...")

#Function for loading up hub world
#Function for loading up random starting world
#Function For loading up mid level world
#Function for loading up boss room after so many tries

#Stores player data to transfer between worlds
