class_name Player
extends CharacterBody3D

# @onready var anim = $AnimationPlayer doesn't work for some reason
@export var anim: AnimationPlayer

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
#	# Add the gravity.
#	if not is_on_floor():
#		velocity.y -= gravity * delta
#
#	# Handle Jump.
#	if Input.is_action_just_pressed("jump") and is_on_floor():
#		velocity.y = jump_height
	
	# Get the input direction and handle the movement/deceleration.
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

#	velocity.x = lerp(velocity.x, hori_input * speed, delta * accel)
#	move_and_slide()
