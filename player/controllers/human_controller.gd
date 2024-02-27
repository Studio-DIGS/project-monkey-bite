class_name HumanController
extends PlayerController

func _physics_process(delta):
	if is_active:
		if Input.is_action_just_pressed("SwitchSceneGrassTest-Debug"):
			GameManager.goto_scene("res://levels/grasstest.tscn")
			return
		if Input.is_action_just_pressed("SwitchSceneWorld-Debug"):
			GameManager.goto_scene("res://levels/world.tscn")
			return
		if Input.is_action_just_pressed("RandomizeInventory-Debug"):
			Inventory.randomInitAll()
			player.updateInventory()
			return
		
		if Input.is_action_just_pressed("attack"):
			player.attack()
#		elif Input.is_action_just_released("attack") or Input.is_action_pressed("attack"):
#			player.stop_attack()
		
		elif Input.is_action_pressed("jump"):
			player.jump(delta)
		elif Input.is_action_just_released("jump"):
			player.stop_jump()
		
		var move_input = Input.get_axis("left", "right")
		player.move(move_input)
