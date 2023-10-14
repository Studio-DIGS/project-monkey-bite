extends PlayerState

var right_fist = true

func enter(_msg := {}):
	print(player.anim.assigned_animation)
	player.anim.pause()
	if right_fist:
		player.anim.play("attack1")
		right_fist = false
	else:
		player.anim.play("attack2")
		right_fist = true

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.move_and_slide()


func _on_animation_player_animation_finished(_anim_name):
	state_machine.transition_to("Idle")
