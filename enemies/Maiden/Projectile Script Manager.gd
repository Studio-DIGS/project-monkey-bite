extends Node

var projectile = preload("res://enemies/Maiden/Maiden_Projectiles.tscn")
var projectile_direction

func _on_maiden_ai_manager_projectile(direction: Vector3, enemy_origin_position):
	projectile_direction = direction
	var projectile_instance = projectile.instantiate()
	projectile_instance.global_transform.origin = enemy_origin_position
	projectile_instance.velocity = direction
	add_child(projectile_instance)
