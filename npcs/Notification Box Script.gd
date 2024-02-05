extends Node3D

var rotation_speed: float = .6
var oscillation_amplitude = .3
var oscillation_speed = .2
var oscillationGeneral

# Called when the node enters the scene tree for the first time.
func _ready():
	$"3D Exclamation Notif".visible = false
	$"3D Question Notif".visible = false
	$"3D Dots Notif".visible = false
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	rotate_y(rotation_speed * delta)
	oscillation_speed += .005
	transform.origin.y = oscillation_amplitude * sin(oscillation_speed)

func _on_temporary_run_simulator_objective_one_complete():
	$"3D Exclamation Notif".visible = true
	print("Showing")

func _on_temporary_run_simulator_reseting_hub():
	$"3D Exclamation Notif".visible = false
	$"3D Question Notif".visible = false
	$"3D Dots Notif".visible = false
