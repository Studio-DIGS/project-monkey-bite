class_name HumanController
extends PlayerController

func _physics_process(delta):
	if is_active:
		if Input.is_action_just_pressed("attack"):
			player.attack()
#		elif Input.is_action_just_released("attack") or Input.is_action_pressed("attack"):
#			player.stop_attack()
		elif Input.is_action_just_pressed("throw"):
			player.throw()
		elif Input.is_action_pressed("jump"):
			player.jump(delta)
		elif Input.is_action_just_released("jump"):
			player.stop_jump()
		elif Input.is_action_just_pressed("dash"):
			player.dash()
		elif Input.is_action_just_pressed("interact"):
			player.interact()
		
		var move_input = Input.get_axis("left", "right")
		player.move(move_input)
		
		var vertical_input = Input.get_axis("down", "up")
		player.vert_move(vertical_input)
#
#		if Input.is_action_just_pressed("SwitchSceneGrassTest-Debug"):
#			GameManager.goto_scene("res://levels/grasstest.tscn")
#			return
#		if Input.is_action_just_pressed("SwitchSceneWorld-Debug"):
#			GameManager.goto_scene("res://levels/world.tscn")
#			return
#		if Input.is_action_just_pressed("RandomizeInventory-Debug"):
#			InventoryManager.randomInitAll()
#			player.updateInventory()
#			return
#		if Input.is_action_just_pressed("SaveGame"):
#			print("saved game")
#			SaverLoader.save_game()
#			return
#		if Input.is_action_just_pressed("LoadGame"):
#			print("loaded game")
#			SaverLoader.load_savedgame()
#			# TODO, The below should be managed by the above funciton
#			player.updateInventory()
#			return
		
