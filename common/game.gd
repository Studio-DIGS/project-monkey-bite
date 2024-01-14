extends Node3D

#@export_group("Worlds List")
#@export var World1: PackedScene
var preloadScene = preload("res://levels/test_scene_2.tscn")

func _process(_delta):
	if (Input.is_action_just_pressed("next level")):
#		if World1 != null:
#			get_tree().change_scene(World1)
#			print("next world")
#		else:
#			print("World 1 is Null")
		get_tree().change_scene_to_packed(preloadScene)
		print("Loading next world...")
