class_name Health
extends Node

@export var max_health = 10
var curr_health: int
@export var display: ProgressBar

signal death

func _ready():
	curr_health = max_health

func take_damage(damage: int):
	curr_health = max(curr_health - damage, 0)
	
	if display:
		display.value = (float(curr_health) / float(max_health)) * 100
	
	if curr_health <= 0:
		emit_signal("death")
