extends Area3D

@export var scene_name: String

func _on_body_entered(_body):
	GameManager.goto_scene("res://levels/" + scene_name + ".tscn")
