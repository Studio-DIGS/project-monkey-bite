extends RigidBody3D

@export var stats: Sword
@onready var hitbox: Hitbox = $Hitbox
var direction = 1

func throw(direction):
	self.direction = direction
	self.rotation_degrees = Vector3(0,0, -90.0 * direction)
	hitbox.knockback.x *= direction
	linear_velocity.x = stats.throw_speed * direction
	

func bounce():
	apply_torque_impulse(Vector3(0, 0, 5 * direction))
	apply_impulse(Vector3(-4 * direction, 7, 0))
	
func settle():
	axis_lock_angular_x = false
	axis_lock_angular_y = false
	axis_lock_linear_y = false
	axis_lock_linear_z = false
	linear_damp = 1
	# no longer react to enemies
	set_collision_mask_value(5, false) 
	hitbox.set_deferred("monitoring", false)
	hitbox.set_deferred("monitorable", false)
	

func _on_body_entered(body):
	axis_lock_angular_z = false
	linear_velocity.x = 0.0
	axis_lock_linear_y = false
	if body is Actor:
		print('hit enemy')
		bounce()
	else:
		print('hit ground')
		settle()

		

