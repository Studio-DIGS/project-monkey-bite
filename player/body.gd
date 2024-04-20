extends Node3D

@export var smear_origin: Node3D
@onready var smear = preload("res://player/swords/attack_smear.tscn")
var smear_instance: Trail3D


func _on_player_turn_around():
	rotate_y(deg_to_rad(180))
	
func spawn_smear():
	smear_instance = smear.instantiate()
	# IDK WHY WE HAVE TO MULTIPLY BY NEG 1 BUT IT FINALLY FUCKING WORKS
	smear_instance.position = smear_origin.global_position * -1
	smear_origin.add_child(smear_instance)

func free_smear():
	if smear_instance != null:
		if !smear_instance.is_queued_for_deletion():
			smear_instance.queue_free()
