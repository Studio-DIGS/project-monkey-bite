extends PlayerState
var slide_velocity: float

func enter(_msg := {}):
	slide_velocity = player.velocity.x * 0.5
	
	player.anim.play("Throw")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, slide_velocity, delta * player.accel)
	player.move_and_slide()



func _on_animation_player_animation_finished(_anim_name):
	var run = false
	if state_machine.state.name == "Throw":
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

