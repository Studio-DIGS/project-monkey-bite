extends PlayerState


func enter(_msg := {}):
	player.anim.play("Block")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()

	
func _on_animation_player_animation_finished(_anim_name):
	if state_machine.state.name == "Block":
		if player.is_on_floor(): 
			state_machine.transition_to("Idle") 
		else: 
			state_machine.transition_to("Air")

