extends Node3D

var self_destruct_timer
# Called when the node enters the scene tree for the first time.
func _ready():
	self_destruct_timer = $"Self-Destruct Timer"
	self_destruct_timer.start()
	

func _on_self_destruct_timer_timeout():
	queue_free()
