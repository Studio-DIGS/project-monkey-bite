extends PlayerState

func enter(_msg := {}):
	start_dash_timer()
#		player.anim.play("Dash")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, player.dash_speed * player.orientation, delta * player.accel)
	player.move_and_slide()

func start_dash_timer():
	await get_tree().create_timer(player.dash_time).timeout
	
	state_machine.transition_to("Idle")
