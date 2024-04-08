extends Resource
class_name Sword

@export var name : String
@export var damage = 10.0
@export var knockback = 5.0

func _init(setName: String = "SWORD"):
	name = setName