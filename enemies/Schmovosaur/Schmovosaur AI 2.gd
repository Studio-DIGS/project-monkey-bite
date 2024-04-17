#Notes******************************
#Make sure to connect PlayerDetectionSphere enter and exit to this AI
#Make sure to connect this AI to main parent node
#***********************************
extends Node

var enemy_speed
var current_direction
var current_velocity

var player_body
var enemy_body
var bread_crumb #Location of player at a certain time
var bread_crumb_multiplier

#var state_passive: bool
var state_active: bool
var state_charge: bool

var isGrounded: bool

var charge_ready: bool
var charge_freeze: bool

var cooldown_charge
var cooldown_prepare
var cooldown_duration

var particles_preparation
var particles_post

signal checkVelocity
# Called when the node enters the scene tree for the first time.
func _ready():
	enemy_speed = 5
	bread_crumb_multiplier = 1.3 #Changes offset for how much further enemy dashes past player
	enemy_body = $".." #Make sure to check/update this when adding AI
	current_velocity = Vector3(0,0,0)
	
	state_active = false
	
	cooldown_charge = $"Charge Cooldown"
	cooldown_prepare = $"Prepare Charge"
	cooldown_duration = $"Charge Duration"
	
	charge_ready = true
	charge_freeze = true
	
	isGrounded = true
	
	particles_preparation = $"Charge Preparation Particles"
	particles_post = $"Charge Particles"
	particles_preparation.visible = true
	particles_post.visible = true
	particles_preparation.emitting = false
	particles_post.emitting = false

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if state_charge == true:
		chargeState(delta)
	elif state_active == true:
		activeState(delta)
		if charge_ready == true and enemy_body.is_on_floor():
			state_charge = true
			bread_crumb = player_body.position
			cooldown_prepare.start()
			particles_preparation.emitting = true
	else:
		passiveState(delta)
	
	emit_signal("checkVelocity", current_velocity)
	
#currentVelocity
#if need to turn
#emitvelocity
#if charge
#emitvelocity

func activeState(delta): #Tracks player moving towards their direction
#	print("activeState")
	if player_body:
		current_direction = (player_body.position - enemy_body.position).normalized()
		current_velocity = current_direction * enemy_speed * delta
		current_velocity = Vector3(current_velocity.x, 0 ,0)
func passiveState(delta): #Stops all movement
#	print("passiveState")
	current_velocity = Vector3.ZERO

func chargeState(delta): #Increase velocity
#	print("chargeState")
	particles_post.position = enemy_body.position
	particles_preparation.position = enemy_body.position
	
	if charge_freeze == true: #before charge
		current_velocity = Vector3.ZERO
	else: #during charge
		current_direction = (bread_crumb * bread_crumb_multiplier - enemy_body.position).normalized()
		
		rotateParticles()
		current_velocity = current_direction * enemy_speed * 15 * delta
		current_velocity = Vector3(current_velocity.x, 0 ,0)
		
func rotateParticles(): #Compares position between player trail and enemy to rotate particle face
	if enemy_body.position.x < bread_crumb.x:
		particles_post.rotate(Vector3(0,1,0), 0)
		particles_preparation.rotate(Vector3(0,1,0), 0)
	else:
		particles_post.rotate(Vector3(0,1,0), PI)
		particles_preparation.rotate(Vector3(0,1,0), PI)
		
func _on_player_detection_sphere_area_entered(area: Hurtbox): #Grabs player information for program
	if area:
		state_active = true
		player_body = area.get_parent()
		print("Got parent")

func _on_player_detection_sphere_area_exited(area):
	state_active = false

func _on_prepare_charge_timeout(): #enemy charges
	charge_freeze = false
	particles_preparation.emitting = false
	particles_post.emitting = true
	FMODRuntime.play_one_shot_attached_path("event:/Dash", self)
	cooldown_duration.start()

func _on_charge_duration_timeout(): #charge is on cooldown
	state_charge = false
	charge_ready = false
	particles_post.emitting = false
	cooldown_charge.start()
	

func _on_charge_cooldown_timeout(): #ready condition for charging
	charge_ready = true
	charge_freeze = true

