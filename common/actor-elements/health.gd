class_name Health
extends Node

@export var max_health = 10
var curr_health: int
var health_percent: float = 100.0
@export var display: ProgressBar
var player_display: InventoryVis

signal death

func _ready():
	curr_health = max_health

func _process(delta):
	if display:
		display.value = lerp(display.value, health_percent, 10 * delta)


func take_damage(damage: int):
	curr_health = max(curr_health - damage, 0)
	health_percent = (float(curr_health) / float(max_health)) * 100
	
	if player_display:
		player_display.update_health(curr_health, max_health)
	
	if curr_health <= 0:
		emit_signal("death")
