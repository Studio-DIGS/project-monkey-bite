extends PlayerState

var combo_counter = 0
var max_combo = 0

func enter(_msg := {}):
	print(combo_counter)
	max_combo = player.combo.size() - 1
	
	var xknockback = player.combo[combo_counter].knockback.x * player.orientation
	var yknockback = player.combo[combo_counter].knockback.y
	player.hitbox.knockback = Vector2(xknockback, yknockback)
	
	if combo_counter == 0:
		player.anim.play(player.combo[combo_counter].animation)
	else:
		player.anim.queue(player.combo[combo_counter].animation)
	combo_counter += 1

func physics_update(delta):
	player.velocity.x = lerp(player.velocity.x, 0.0, delta * player.accel)
	player.move_and_slide()
	
	if Input.is_action_just_pressed("attack") and combo_counter <= max_combo:
		state_machine.transition_to("Attack")


func _on_animation_player_animation_finished(_anim_name):
	combo_counter = 0
	state_machine.transition_to("Idle")
