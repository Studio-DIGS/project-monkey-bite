extends Node3D

signal resetingHub
signal objectiveOneComplete

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	if Input.is_action_just_pressed("resetHub"):
		emit_signal("resetingHub")
		print("Resetting Hub World")
	if Input.is_action_just_pressed("completeObjectiveOne"):
		emit_signal("objectiveOneComplete")
		print("Objective for NPC 1 completed")
