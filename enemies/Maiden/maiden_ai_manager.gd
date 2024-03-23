extends Node3D

signal projectile
signal check_velocity
signal turn_transparent

var projectile_direction: Vector3
var projectile_interval_timer
var attack_cooldown
var attack_ready: bool

var evade_ready: bool
var minimum_escape_value: float
var maximum_escape_value: float
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
# Called when the node enters the scene tree for the first time.
func _ready():
	enemy_body = $".."
	projectile_interval_timer = .2 #Seconds
	attack_cooldown = $"Attack Cooldown"
	attack_ready = true
	
	state_passive = false #adjust this for testing purposes
	
	enemy_velocity = Vector3(1, 0, 0)
	original_position = enemy_body.position
	tether_range = 10
	
	occur_once_evade = true #Designed for evade()
	occur_once_hit = true
	escape_timer = $"Escape Timer"
	
#Physics Process Controls the state machine between evade/active/passive states	
func _physics_process(delta):
	if Input.is_action_just_pressed("interact") and player_body: #TEMP test for spawn_projectile
			emit_signal("turn_transparent")
			await get_tree().create_timer(.8).timeout
			evade_ready = true
			occur_once_evade = true
			await get_tree().create_timer(.8).timeout
			emit_signal("turn_transparent")
	
#	passive_state()
	#Test
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
	if attack_ready:
		attack_cooldown.start() #attack ready = true after timer
#		spawn_projectile(delta) #attack ready = false here
		attack_ready = false
		
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
		print(offset_original_position)
	
	#Enemy continues to move towards new position and stopped upon reaching destination	
	if abs(new_position.x - enemy_body.position.x) < .2: 
		enemy_velocity = Vector3.ZERO
		evade_ready = false
		print("Evade ends")

func spawn_projectile(delta):
	projectile_direction = (player_body.position - enemy_body.position).normalized()
	
	#Shoots 3 balls
	for i in range(3): 
		emit_signal("projectile", projectile_direction, enemy_body.position)
		await get_tree().create_timer(projectile_interval_timer).timeout

func _on_player_detection_sphere_area_entered(area): #Evade State
	if area:
		player_body = area.get_parent()
		state_active = true
		state_passive = false
	#Check for evade cooldown

func _on_player_detection_sphere_area_exited(area): #Attack State
	if area:
		pass
		
func _on_player_detection_sphere_2_area_exited(area): #Enter passive state
	if area:
		state_passive = true
		state_active = false
		
func _on_attack_cooldown_timeout():
	attack_ready = true

#If player attacks maiden, enemy prepares to flee
func _on_maiden_enemy_enemy_hit_instance(): #Connect Signal from "Maiden Enemy"
	if occur_once_hit:
		occur_once_evade = false
		escape_timer.start()

func _on_escape_timer_timeout():
	emit_signal("turn_transparent")
	await get_tree().create_timer(.8).timeout
	evade_ready = true
	occur_once_evade = true
	occur_once_hit = true
	await get_tree().create_timer(.8).timeout
	emit_signal("turn_transparent")
	print("evade active")
	
#Emits signal to Main Maiden Enemy Node
func move():
	emit_signal("check_velocity", enemy_velocity)



