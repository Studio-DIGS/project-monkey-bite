class_name Inventory
extends Resource

@export var sword: Sword 
@export var bigPassives: Array[BigPassive]
@export var angelAbility: AngelAbility

func _init(setSword: Sword = Sword.new("null", 0.0, 0.0), setBigPassives: Array[BigPassive] = [],
 setAngelAbility: AngelAbility = AngelAbility.new("null")):
	sword = setSword
	bigPassives = setBigPassives
	angelAbility = setAngelAbility
