class_name Hurtbox
extends Area3D

@export var health: Health = null

signal hit(vector: Vector2)

func _ready():
	connect("area_entered", _on_area_entered)

func _on_area_entered(hitbox: Hitbox):
	if hitbox == null:
		return
	
	hit.emit(hitbox.knockback)
	
	if health:
		health.take_damage(hitbox.damage)
	
	# Freeze frame
	## should probably be moved somewhere else because I think this will get 
	## called multiple times if multiple enemies get hit at the same time
	Engine.time_scale = hitbox.freeze_slow
	await get_tree().create_timer(hitbox.freeze_time * hitbox.freeze_slow).timeout
	Engine.time_scale = 1
