extends Area3D

@export var character: CharacterBody3D

func _on_body_entered(body):
	if character:
		character.set_collision_mask_value(9, true)
	else:
		print("Make sure to set the character in the inspector")

func _on_body_exited(body):
	if character:
		character.set_collision_mask_value(9, false)
	else:
		print("Make sure to set the character in the inspector")
