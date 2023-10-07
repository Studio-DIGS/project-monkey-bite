class_name Hurtbox
extends Area3D

@export var health: Health = null

func _ready():
	connect("area_entered", _on_area_entered)

func _on_area_entered(hitbox: Hitbox):
	if hitbox == null:
		return
	
	if health:
		health.take_damage(hitbox.damage)
