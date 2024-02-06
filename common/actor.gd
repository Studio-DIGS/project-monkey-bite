class_name Actor
extends CharacterBody3D

var hori_input = 0.0

func move(value: float) -> void:
	hori_input = clamp(value, -1.0, 1.0)

func attack() -> void:
	pass

func jump(_delta) -> void:
	pass

func interact() -> void:
	pass
