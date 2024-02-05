extends Actor

@export var attack_resource: AttackResource
var start_pos

# Called when the node enters the scene tree for the first time.
func _ready():
	start_pos = position
	

func _physics_process(delta):
	if Input.is_action_just_pressed("reset"):
		position = start_pos
	
	velocity.y -= 20 * delta
	
	if is_on_floor():
		velocity.x = lerp(velocity.x, 0.0, delta * 5)
	
	move_and_slide()


func _on_hurtbox_hit(vector: Vector2):
	$AnimationPlayer.play("stagger")
	velocity = Vector3(vector.x, vector.y, 0)


func _on_health_death():
	await get_tree().create_timer(0.3).timeout
	queue_free()


func _on_timer_timeout():
	$AnimationPlayer.play(attack_resource.animation)
	$Hitbox.configure_hitbox(attack_resource)
