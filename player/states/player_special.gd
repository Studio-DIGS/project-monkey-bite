extends PlayerState
var special: AttackResource

func enter(_msg := {}):
	special = player.sword.special_attack
	player.anim.play(special.animation)

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if player.stagger:
		state_machine.transition_to("Stagger")
		return

	
func _on_animation_player_animation_finished(_anim_name):
	if state_machine.state.name == "Special":
		if player.is_on_floor(): 
			state_machine.transition_to("Idle") 
		else: 
			state_machine.transition_to("Air")

