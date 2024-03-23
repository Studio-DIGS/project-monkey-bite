extends Node3D

var hover_speed
var hover_start_height
var cube_current_position
var floating_timer

# Called when the node enters the scene tree for the first time.
func _ready():
	cube_current_position = 0
	floating_timer = 0
	hover_start_height = 2
	hover_speed = 45

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	floating_timer += PI/180
	cube_current_position = sin(floating_timer * delta * hover_speed) + hover_start_height
	position.y = cube_current_position
