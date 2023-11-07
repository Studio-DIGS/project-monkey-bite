extends CharacterBody3D

var attackDuration: float = 5.0
var attackTimer: float = 0
var cooldown: float = 5.0
var cooldownTimer: float = 5.0

@onready var hitboxSpawn = $HitboxSpawn

var hitboxScene = preload("res://common/hitbox.tscn")

var hitbox: Hitbox = null

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func spawnHitbox():
	hitbox = hitboxScene.instantiate()
	hitboxSpawn.add_child(hitbox)
	hitbox.set_collision_layer_value(7, true)
	hitbox.set_collision_layer_value(3, false)
	print("spawned hitbox")

func despawnHitbox():
	hitbox.queue_free()
	hitbox = null
	print("despawned hitbox")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _physics_process(delta):
	if(attackTimer == 0):
		if(cooldownTimer > 0):
			cooldownTimer -= delta
		if(cooldownTimer < 0):
			cooldownTimer = 0
			spawnHitbox()
			attackTimer = attackDuration
	elif(cooldownTimer == 0):
		if(attackTimer > 0):
			attackTimer -= delta
		if(attackTimer < 0):
			attackTimer = 0
			despawnHitbox()
			cooldownTimer = cooldown
	
	velocity.y -= 20 * delta
	
	if is_on_floor():
		velocity.x = lerp(velocity.x, 0.0, delta * 5)
	
	move_and_slide()


func _on_hurtbox_hit(vector: Vector2):
	$AnimationPlayer.play("stagger")
	velocity = Vector3(vector.x, vector.y, 0)
