extends CharacterBody3D

signal enemy_hit_instance

@export var attack_resource: AttackResource

var gravity_speed: float
var hitbox
var animation

func _ready():
	gravity_speed = 20.0
	animation = $AnimationPlayer
	hitbox = $Hitbox

func _physics_process(delta):
	velocity.y -= gravity_speed * delta
	
	if is_on_floor():
		velocity.x = lerp(velocity.x, 0.0, delta * 5)
	
	move_and_slide()

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
	velocity = velocity + velocity_passed
