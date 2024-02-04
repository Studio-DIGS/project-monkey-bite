class_name Actor
extends CharacterBody3D

@export var hori_input = 0.0

func move(value: float) -> void:
	hori_input = value
