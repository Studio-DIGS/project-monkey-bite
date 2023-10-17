extends CharacterBody3D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	velocity.y -= 20 * delta
	
	if is_on_floor():
		velocity.x = lerp(velocity.x, 0.0, delta * 5)
	
	move_and_slide()


func _on_hurtbox_hit(vector: Vector2):
	$AnimationPlayer.play("stagger")
	velocity = Vector3(vector.x, vector.y, 0)
