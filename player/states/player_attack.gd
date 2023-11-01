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
		
	print(combo_counter)
	max_combo = player.combo.size() - 1
	
	var xknockback = combo[combo_counter].knockback.x * player.orientation
	var yknockback = combo[combo_counter].knockback.y
	player.hitbox.knockback = Vector2(xknockback, yknockback)
	player.hitbox.freeze_slow = combo[combo_counter].freeze_slow
	player.hitbox.freeze_time = combo[combo_counter].freeze_time
	
	if combo_counter == 0:
		player.anim.play(combo[combo_counter].animation)
	else:
		player.anim.queue(combo[combo_counter].animation)
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
