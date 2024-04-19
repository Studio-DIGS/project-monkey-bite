extends PlayerState
var slide_velocity: float

func enter(_msg := {}):
	player.anim.play("Throw")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, slide_velocity, delta * player.accel)
	player.move_and_slide()

func throw_sword():
	player.is_armed = false
	print("threw")
	var projectile_instance: RigidBody3D = player.sword_body.instantiate()
	projectile_instance.position = player.sword_spawn.global_position
	GameManager.current_scene.add_child(projectile_instance)
	projectile_instance.throw(player.orientation)
	
	if player.sword_holder.get_child_count() > 0:
		player.sword_holder.get_child(0).queue_free()
	
func _on_animation_player_animation_finished(_anim_name):
	if state_machine.state.name == "Throw":
		throw_sword()
		var run = false
		
		if player.hori_input != 0:
			run = true
			player.anim.play("Run")
		else:
			player.anim.play("Idle")
		
		# wait for animation to finish blending to idle before fully transitioning state
		var blend_time = player.anim.playback_default_blend_time
		await get_tree().create_timer(blend_time).timeout
		
		if run:
			state_machine.transition_to("Run")
		else:
			state_machine.transition_to("Idle")

