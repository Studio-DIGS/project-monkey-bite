extends Node3D

# reset rotation every frame cuz the smear doesn't fucking work if you don't 
func _process(_delta):
	global_rotation = Vector3.ZERO
