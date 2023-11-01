class_name Player
extends CharacterBody3D

# @onready var anim = $AnimationPlayer doesn't work for some reason
@export var anim: AnimationPlayer
@export var hitbox: Hitbox
@export var combo: Array[AttackResource] = []
@export var air_combo: Array[AttackResource] = []

@export var speed = 5.0
@export var accel = 15.0
@export var jump_height = 4.5

# Get the gravity from the project settings to be synced with RigidBody nodes.
#var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")
var gravity = 20

# direction player is facing (1 is forward, -1 is backwards)
var orientation = 1
signal turn_around

var hori_input = 0.0

func _physics_process(_delta):
	# Get the input direction
	hori_input = Input.get_axis("left", "right")
	
	# stores current orientation
	var new_orientation = orientation
	if hori_input > 0:
		new_orientation = 1
	elif hori_input < 0:
		new_orientation = -1

	# Turn player around
	if new_orientation != orientation:
		orientation = new_orientation
		emit_signal("turn_around")
