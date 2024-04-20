extends PlayerState
var velocity

func enter(_msg := {}):
	player.velocity = Vector3(player.stagger_vector.x, player.stagger_vector.y, 0)
	player.anim.play("Flinch")
	player.inventory_vis.update_face(5)
	start_cooldown_timer()

func physics_update(delta):
	player.velocity.y -= player.gravity * delta
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.move_and_slide()


func start_cooldown_timer():
	await get_tree().create_timer(player.stagger_cooldown).timeout
	player.stagger = false
	player.inventory_vis.update_face(1)
	state_machine.transition_to("Idle")
