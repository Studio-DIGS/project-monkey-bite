extends PlayerState
var direction = 1
var can_dash = true

func enter(_msg := {}):
	if can_dash:
		player.anim.play("Dash")
		direction = player.hori_input
		player.hurtbox.set_deferred("monitorable", false)
		player.hurtbox.set_deferred("monitoring", false)
		start_dash_timer()
	#	player.anim.play("Dash")
	else:
		state_machine.transition_to("Idle")

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, player.dash_speed * direction, delta * player.accel)
	player.velocity.y = lerp(player.velocity.y, 0.0, delta * player.accel)
	player.move_and_slide()

func start_dash_timer():
	FMODRuntime.play_one_shot_attached_path("event:/Player Dash", self)
	await get_tree().create_timer(player.dash_time).timeout
	player.hurtbox.set_deferred("monitorable", true)
	player.hurtbox.set_deferred("monitoring", true)
	can_dash = false
	start_cooldown_timer()
	state_machine.transition_to("Run")

func start_cooldown_timer():
	await get_tree().create_timer(player.dash_cooldown).timeout
	can_dash = true
