extends Node3D

signal projectile
signal check_velocity
signal turn_transparent

var projectile_direction: Vector3
var projectile_interval_timer
var attack_cooldown
var movement_inhibit_cooldown
var attack_ready: bool
var attack_inhibit: bool #Prevents shooting when invisible
var movement_inhibit: bool

var evade_ready: bool
var offset_original_position: float
var new_position: Vector3
var occur_once_evade: bool
var occur_once_hit: bool
var escape_timer

var state_passive: bool
var state_active: bool

var enemy_body
var player_body

var original_position: Vector3
var enemy_direction: Vector3
var tether_range: float

var enemy_velocity: Vector3
var enemy_vertical_velocity: Vector3 #Given the maiden floaty effect

#Floating Cube Initial Variables
var cube_direction
var cube_body

var player_detection_range
var random_variance: float

@onready var maiden_3D = $"../attackAnim_maiden2"
@onready var maiden_animation_player = $"../attackAnim_maiden2"/AnimationPlayer
# Called when the node enters the scene tree for the first time.
func _ready():
	enemy_body = $".."
	projectile_interval_timer = .2 #Seconds
	attack_cooldown = $"Attack Cooldown"
	movement_inhibit_cooldown = $"Movement Inhibit Timer"
	
	attack_ready = true
	attack_inhibit = false
	movement_inhibit = false
	
	
	state_passive = false #adjust this for testing purposes
	
	enemy_velocity = Vector3(0, 0, 0)
	original_position = Vector3(enemy_body.position.x ,0 , 0) + Vector3(1, 0, 0) #Helps passive state have initial velocity
	tether_range = 4
	
	occur_once_evade = true #Designed for evade()
	occur_once_hit = true
	escape_timer = $"Escape Timer"
	
#	cube_body = $"../Floating Cube"
	
	player_detection_range = 10
	random_variance = randf()
	
#Physics Process Controls the state machine between evade/active/passive states	
func _physics_process(delta):
	if Input.is_action_just_pressed("interact") and player_body: #TEMP test for spawn_projectile
			emit_signal("turn_transparent")
			await get_tree().create_timer(.8).timeout
			evade_ready = true
			occur_once_evade = true
			await get_tree().create_timer(.8).timeout
			emit_signal("turn_transparent")
	
	if player_body:
		player_detection_sphere()
	
#	passive_state()
	if evade_ready:
		evade()
#		print("evade State")
	elif state_active:
		if player_body:
			active_state(delta)
#			print("active State")
			pass
	elif state_passive:
		passive_state()
#		print("passive State")
	
	#Necessary
	move()
	
func passive_state():
	if abs(enemy_body.position.x) >= tether_range + abs(original_position.x):
		enemy_direction = (original_position - enemy_body.position).normalized()
		enemy_velocity = Vector3(enemy_direction.x, 0 ,0)
		
func active_state(delta):
	enemy_velocity = Vector3.ZERO
	if attack_ready and !attack_inhibit:
		enemy_velocity = Vector3.ZERO
		attack_cooldown.start() #attack ready = true after timer
		movement_inhibit_cooldown.start()
		spawn_projectile(delta) #attack ready = false here
		attack_ready = false
		movement_inhibit = true
	if attack_ready == false and !movement_inhibit:
		if abs(player_body.position.x - enemy_body.position.x) < player_detection_range*(random_variance + .3):
			enemy_direction = (enemy_body.position - player_body.position).normalized()
			enemy_velocity = Vector3(enemy_direction.x/(random_variance+3), 0 ,0)
		else:
			enemy_velocity = Vector3.ZERO
		
func evade():
	#TEMP ignore math
	#Is Called once calculating the new position for enemy
	if occur_once_evade: 
		occur_once_evade = false
		var temp_number = randf()
		if temp_number < .5:
			offset_original_position = 1 * randf() * 3 + 3
			print("right")
		else:
			offset_original_position = -1 * randf() * 3 - 3
			print('left')
		new_position = Vector3(enemy_body.position.x + offset_original_position, 0, 0)
		enemy_direction = (new_position - enemy_body.position).normalized()
		enemy_velocity = Vector3(enemy_direction.x, 0 ,0)
	
	#Enemy continues to move towards new position and stopped upon reaching destination	
	if abs(new_position.x - enemy_body.position.x) < .2: 
		enemy_velocity = Vector3.ZERO
		evade_ready = false
		print("Evade ends")

func spawn_projectile(delta):
	maiden_animation_player.play("Key_001Action")
	projectile_direction = (player_body.position - enemy_body.position).normalized()
	#Maiden turns using the following lines
	if projectile_direction.x >= 0:
		maiden_3D.rotation_degrees = Vector3(0, 90, 0)
	else:
		maiden_3D.rotation_degrees = Vector3(0, -90, 0)
	#Shoots 3 balls
	for i in range(3): 
		emit_signal("projectile", projectile_direction, enemy_body.position) #Sends signal to spawn balls
		await get_tree().create_timer(projectile_interval_timer).timeout

	#Check for evade cooldown
func player_detection_sphere():
	if abs(player_body.position.x - enemy_body.position.x) < player_detection_range:
		state_active = true
		state_passive = false
	elif abs(player_body.position.x - enemy_body.position.x) > 2 * player_detection_range:
		state_passive = true
		state_active = false
		original_position = enemy_body.position
		
func _on_attack_cooldown_timeout():
	attack_ready = true

func _on_movement_inhibit_timer_timeout():
	movement_inhibit = false

#If player attacks maiden, enemy prepares to flee
func _on_maiden_enemy_enemy_hit_instance(): #Connect Signal from "Maiden Enemy"
	if occur_once_hit:
		occur_once_evade = false
		escape_timer.start()

func _on_escape_timer_timeout(): #Enemy begins to fade out and in
	emit_signal("turn_transparent")
	attack_inhibit = true
	await get_tree().create_timer(.8).timeout
	evade_ready = true
	occur_once_evade = true
	occur_once_hit = true
	await get_tree().create_timer(.4).timeout
	emit_signal("turn_transparent")
	attack_inhibit = false
	
#Emits signal to Main Maiden Enemy Node
func move():
	emit_signal("check_velocity", enemy_velocity)
	maiden_animation_player.play("Armature_001Action")
	print("Playing action")
	

func _on_maiden_enemy_send_player_information(player_information):
	player_body = player_information
	print("information transferred")

func _on_hurtbox_enemy_has_been_hit():
	movement_inhibit = true
	await get_tree().create_timer(1.5).timeout
	movement_inhibit = false
