extends Actor

@export var attack_resource: AttackResource
var start_pos

#Schmovosaur sfx
@export var event: EventAsset 
#loads event on schmovosaur Enemy.gd, link corresponding fmod event

@onready var hitbox_information = $Hitbox

signal knockback_direction
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
	
	hitbox_information.knockback = (velocity + Vector3(0,2,0))* (3 + 2*randf())
	

# @TEMP
func _on_hurtbox_hit(vector: Vector2):
	FMODRuntime.play_one_shot_path("event:/Schmovosaur ouchie")
	velocity = Vector3(vector.x, vector.y, 0)


func _on_health_death():
	await get_tree().create_timer(0.3).timeout
	queue_free()

	
func _on_schmovosaur_ai_2_check_velocity(vector: Vector3):
	velocity = velocity + vector
