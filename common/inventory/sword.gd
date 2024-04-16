extends Resource
class_name Sword

@export var name : String
@export var damage : float
@export var knockback : float
	
func _init(setName: String, setDamage: float, setKnockback: float):
	name = setName
	damage = setDamage
	knockback = setKnockback
