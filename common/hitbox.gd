class_name Hitbox
extends Area3D

@export var damage = 0
@export var knockback = Vector2(1,1)




func _on_player_turn_around():
	knockback = Vector2(-knockback.x, knockback.y)
