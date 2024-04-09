extends Node3D

var animationSpeed = 0.1
var pathFollow
var flyingActive: bool
var enableInteract: bool
var threshHold: bool = false
var playerBody



# Called when the node enters the scene tree for the first time.
func _ready():
	pathFollow = $"Cube Type 2/Path3D/PathFollow3D"
	flyingActive = false #Prevents animation from playing until true
	enableInteract = false


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if Input.is_action_just_pressed("interact") and enableInteract:
		flyingActive = true
		print("Disabling Player Movement")
	if Input.is_action_just_pressed("transition animation backward"):
		flyingActive = false
	
	if flyingActive == true:
		if threshHold != true:
			pathFollow.progress_ratio += animationSpeed * delta
			playerBody.position = pathFollow.position
			
		
		
#If player presses to interact
#Turn off player movement
#Play animation moving forward
#when reaching playerDetectionPortal2 transfer zones
#copy player coordinates and update new coordinates
#when reaching playerDetectionPortal3 allow player to move

func _on_player_detection_portal_area_entered(area): #Allows you to interact to nextLevel
	if area.name == "PlayerArea":
		enableInteract = true
		playerBody = area.get_parent()
	print("Active to enter")


func _on_player_detection_portal_area_exited(area): #Prevents you from interacting to nextLevel
	enableInteract = false
	print("lost activity")


func _on_player_detection_portal_2_area_entered(area): #Transfer zones
	if area.name == "PlayerArea":
		print("Sending player to other world")
#		get_tree().change_scene_to_packed(Hub1)
	

func _on_player_detection_portal_3_area_entered(area): #Allows player to move again
	print("Player is now able to move")
