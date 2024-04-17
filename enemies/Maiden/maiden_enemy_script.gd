extends CharacterBody3D

signal enemy_hit_instance
signal send_player_information

@export var attack_resource: AttackResource

var gravity_speed: float
var hitbox
var animation

#TEMP
var hover_speed
var hover_start_height
var cube_current_position
var floating_timer
var maiden_body

#TEMP Test
var player_information: CharacterBody3D
var is_frozen: bool
var status_frozen_timer: float
var frozen = preload("res://Freeze_status_effect.tscn")

func _ready():
	gravity_speed = 0
	animation = $KnockbackPlayer
	hitbox = $Hitbox
	
	#TEMP
	cube_current_position = 0
	floating_timer = 0
	hover_start_height = randf() + 1
	hover_speed = 45
	
	player_information = $"../Player"
	
	emit_signal("send_player_information", player_information) #Signals to Maiden AI Manager player's position
	
	status_frozen_timer = 5
	
	#Temp
	maiden_body = $"Maiden Body"
	
func _physics_process(delta):
	velocity.y -= gravity_speed * delta
	
#	if is_on_floor():
#		velocity.x = lerp(velocity.x, 0.0, delta * 5)
	velocity.x = lerp(velocity.x, 0.0, delta * 5)
	move_and_slide()
	
	#TEMP
	if is_frozen:
		pass
	else:
		floating_timer += PI/180
		cube_current_position = sin(floating_timer * delta * hover_speed)/(hover_start_height + 1.2) + hover_start_height
		position.y = cube_current_position

func _on_hurtbox_hit(vector):
	animation.play("stagger")
	emit_signal("enemy_hit_instance")
	velocity = Vector3(vector.x, vector.y, 0)

func _on_health_death():
	await get_tree().create_timer(0.3).timeout
	queue_free()

func _on_death_timer_timeout():
	animation.player(attack_resource.animation)
	hitbox.configure_hitbox(attack_resource)
	
func _on_maiden_ai_manager_check_velocity(velocity_passed: Vector3):
	if !is_frozen:
		velocity = velocity + velocity_passed
	else:
		velocity = Vector3(0,0,0)

#Frozen Status
func _on_hurtbox_send_freeze():
	is_frozen = true
	apply_ice()
	await get_tree().create_timer(status_frozen_timer).timeout
	is_frozen = false

func apply_ice():
	var frozen_instance = frozen.instantiate()
	frozen_instance.position = maiden_body.position
	add_child(frozen_instance)
