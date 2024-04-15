extends RigidBody3D

@export var stats: Sword
@onready var hitbox: Hitbox = $Hitbox
@onready var hurtbox: Hurtbox = $Hurtbox
var direction = 1

func throw(direction, speed = stats.throw_speed):
	self.direction = direction
	self.rotation_degrees = Vector3(0,0, -90.0 * direction)
	hitbox.knockback.x *= direction
	linear_velocity.x = speed * direction
	await get_tree().create_timer(0.5).timeout
	axis_lock_linear_y = false
	

func bounce():
	apply_torque_impulse(Vector3(0, 0, 5 * direction))
	apply_impulse(Vector3(-4 * direction, 7, 0))


func settle():
	axis_lock_angular_x = false
	axis_lock_angular_y = false
	axis_lock_linear_y = false
	axis_lock_linear_z = false
	linear_damp = 1
	# no longer react to enemies or player attacks
	set_collision_mask_value(5, false) 
	hitbox.set_deferred("monitoring", false)
	hitbox.set_deferred("monitorable", false)
	hurtbox.set_deferred("monitoring", false)
	hurtbox.set_deferred("monitorable", false)


func _on_body_entered(body):
	linear_velocity.x = 0.0
	axis_lock_linear_y = false
	if body is Actor:
		bounce()
	else:
		settle()

# hit by player
func _on_hurtbox_hit(vector):
	sleeping = true # this basically turns off the rigid body, reseting all physics
	sleeping = false # turn physics back on
	axis_lock_linear_y = true
	var kick_direction = clamp(vector.x, -1, 1)
	throw(kick_direction, stats.throw_speed * 1.5)
