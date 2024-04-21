extends CharacterBody3D
class_name Projectile
var projectile_speed
var destruct_timer
@onready var hitbox = $Hitbox

func _ready():
	projectile_speed = 5
	destruct_timer = $"Self-Destruct Timer"
	destruct_timer.start()
	
func _process(delta):
	translate(Vector3(velocity.x, velocity.y, 0) * projectile_speed * delta)

func _on_self_destruct_timer_timeout(): #After timer ends instance deletes itself
	queue_free()
	
func reverse_projectile():
	velocity.x = -velocity.x
	velocity.y = -velocity.y

func _on_area_3d_area_entered(area):
	reverse_projectile()


func _on_hurtbox_hit(vector):
	hitbox.collision_layer = 3
	hitbox.collision_mask = 5
	if clamp(vector.x, -1, 1) * clamp(velocity.x, -1, 1) < 0:
		reverse_projectile()
