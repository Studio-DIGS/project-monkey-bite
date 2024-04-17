extends RigidBody3D

@export var stats: Sword
@onready var hitbox: Hitbox = $Hitbox
@onready var hurtbox: Hurtbox = $Hurtbox
@onready var pivot: Node3D = $Pivot
@onready var smear = preload("res://player/swords/sword_smear.tscn")
var smear_instance: Trail3D
var direction = 1 # 1 is right, -1 is left

func throw(direction, speed = stats.throw_speed):
	# delete smear
	if smear_instance:
		smear_instance.queue_free()

	self.direction = direction
	self.rotation_degrees = Vector3(0,0,0) # reset the rigid body
	pivot.rotation_degrees = Vector3(0, 0, 90 * direction) # face the sword mesh the right direction
	hitbox.knockback.x *= direction
	linear_velocity.x = speed * direction # sword flies at constant velocity
	await get_tree().create_timer(stats.flight_time).timeout
	axis_lock_linear_y = false # gravity takes effect after flying straight for x seconds
	

func bounce():
	# add smear
	smear_instance = smear.instantiate()
	# IDK WHY WE HAVE TO MULTIPLY BY NEG 1 BUT IT FINALLY FUCKING WORKS
	smear_instance.position = self.global_position * -1
	self.add_child(smear_instance)
	
	# apply forces to bounce sword
	apply_torque_impulse(Vector3(0, 0, 1.5 * direction))
	apply_impulse(Vector3(-4 * direction, 7, 0))


func settle():
	# lock axis and damp rigidbody (bring velocity to 0)
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
	
	# fade out smear
	if smear_instance != null:
		smear_instance.anim.play("fade")
		await get_tree().create_timer(0.1).timeout


func _on_body_entered(body):
	linear_velocity.x = 0.0 # sword stops flying forward
	axis_lock_linear_y = false # unlock gravity
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
