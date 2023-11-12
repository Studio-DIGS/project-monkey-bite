extends PlayerState

var combo_counter = 0
var max_combo = 0
var combo: Array[AttackResource]
var apply_gravity = false

func enter(msg := {}):
	if msg.has("air"):
		combo = player.air_combo
		apply_gravity = true
	else:
		combo = player.combo
	
	var curr_attack = combo[combo_counter]
	max_combo = player.combo.size() - 1
	
	curr_attack.knockback.x *= player.orientation
	player.hitbox.configure_hitbox(curr_attack)
	
	if combo_counter == 0:
		player.anim.play(curr_attack.animation)
	else:
		player.anim.queue(curr_attack.animation)
	combo_counter += 1

func physics_update(delta):
	if apply_gravity:
		player.velocity.y -= player.gravity * delta
	
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if Input.is_action_just_pressed("attack") and combo_counter <= max_combo:
		state_machine.transition_to("Attack")


func _on_animation_player_animation_finished(_anim_name):
	combo_counter = 0
	state_machine.transition_to("Idle")
