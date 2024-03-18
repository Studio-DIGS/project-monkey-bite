extends Node3D

signal projectile
signal check_velocity

var projectile_direction: Vector3
var projectile_interval_timer
var attack_cooldown
var attack_ready: bool

var evade_ready: bool
var minimum_escape_value: float
var maximum_escape_value: float
var offset_original_position: float

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
	
	minimum_escape_value = 3
	maximum_escape_value = 5
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func _physics_process(delta):
	if Input.is_action_just_pressed("interact") and player_body: #TEMP test for spawn_projectile
		spawn_projectile(delta)
		evade()
	
	if state_passive:
		passive_state()
	else:
		if player_body:
			active_state(delta)
	move()
	
#passive_state()
	#Wander
func passive_state():
	if abs(enemy_body.position.x) >= tether_range + abs(original_position.x):
		enemy_direction = (original_position - enemy_body.position).normalized()
		enemy_velocity = Vector3(enemy_direction.x, 0 ,0)
func active_state(delta):
	if attack_ready:
		attack_cooldown.start() #attack ready = true after timer
		print("started cooldown")
		spawn_projectile(delta) #attack ready = false here
		attack_ready = false
	if evade_ready:
		evade()
	else:
		enemy_velocity = Vector3.ZERO
		
		
func evade():
	enemy_direction = (original_position - enemy_body.position).normalized()
	enemy_velocity = Vector3(enemy_direction.x, 0 ,0)
	if (original_position.x - enemy_body.position.x) < .5:
		evade_ready = false

func spawn_projectile(delta):
	projectile_direction = (player_body.position - enemy_body.position).normalized()
	for i in range(3): #Shoots 3 balls
		emit_signal("projectile", projectile_direction, enemy_body.position)
		await get_tree().create_timer(projectile_interval_timer).timeout
func _on_player_detection_sphere_area_entered(area): #Evade State
	if area:
		print("entering evasion state")
		player_body = area.get_parent()
		
	#Check for evade cooldown

func _on_player_detection_sphere_area_exited(area): #Attack State
	if area:
#		print("entering attack state")
		pass
		
func _on_player_detection_sphere_2_area_exited(area): #Enter passive state
	if area:
#		print("entering passive state")
		pass
		
func _on_attack_cooldown_timeout():
	attack_ready = true

#If player attacks maiden, enemy prepares to flee
func _on_maiden_enemy_enemy_hit_instance(): #Connect Signal from "Maiden Enemy"
	#TEMP ignore math
	offset_original_position = (randf() * 2 - 1) * 8
	original_position.x += offset_original_position
	evade_ready = true
	
#Emits signal to Main Maiden Enemy Node
func move():
	emit_signal("check_velocity", enemy_velocity)

