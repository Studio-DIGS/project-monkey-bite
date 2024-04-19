class_name Hitbox
extends Area3D

@onready var collision_shape = $CollisionShape3D

@export var damage = 0
@export var knockback = Vector2(1,1)
@export var status_effect: String = "None"
@export var freeze_slow = 0.07
@export var freeze_time = 0.2

func configure_hitbox(attack: AttackResource):
	damage = attack.damage
	knockback = attack.knockback
	freeze_slow = attack.freeze_slow
	freeze_time = attack.freeze_time
	
