class_name HumanController
extends PlayerController

func _physics_process(delta):
	if is_active:
		if Input.is_action_just_pressed("attack"):
			player.attack()
		
		elif Input.is_action_pressed("jump"):
			player.jump(delta)
		elif Input.is_action_just_released("jump"):
			player.stop_jump()
		
		var move_input = Input.get_axis("left", "right")
		player.move(move_input)
