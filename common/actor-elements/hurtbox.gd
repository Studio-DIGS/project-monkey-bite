class_name Hurtbox
extends Area3D

@export var health: Health = null
@onready var impact_fx = preload("res://common/vfx/impact_fx.tscn")

signal hit(vector: Vector2)

func _ready():
	connect("area_entered", _on_area_entered)

func _on_area_entered(hitbox: Hitbox):
	if hitbox == null:
		return
	
	hit.emit(hitbox.knockback)
	
	var instance = impact_fx.instantiate()
	add_child(instance)
	
	Engine.time_scale = hitbox.freeze_slow
	await get_tree().create_timer(hitbox.freeze_time * hitbox.freeze_slow).timeout
	Engine.time_scale = 1
	
	if health:
		health.take_damage(hitbox.damage)

func something(object: Projectile):
	object.reverse_projectile()
	
