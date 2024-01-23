extends Node3D

var animationSpeed = 0.1
var pathFollow
var flyingActive: bool
var enableInteract: bool
var threshHold: bool = false
var playerBody

var displacement

#Levels
@export var Hub1: PackedScene = preload("res://levels/test_scene_3.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	pathFollow = $"Cube Type 2/Path3D/PathFollow3D"
	flyingActive = false #Prevents animation from playing until true
	displacement = Vector3(-58.747, -73.497, 0)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if flyingActive == true:
			pathFollow.progress_ratio += animationSpeed * delta
			playerBody.position = pathFollow.position + displacement
			
		
		
#If player presses to interact
#Turn off player movement
#Play animation moving forward
#when reaching playerDetectionPortal2 transfer zones
#copy player coordinates and update new coordinates
#when reaching playerDetectionPortal3 allow player to move

func _on_player_detection_portal_2_area_entered(area): #Transfer zones
		if area.name == "PlayerArea":
			enableInteract = true
			playerBody = area.get_parent()
			flyingActive = true

func _on_player_detection_portal_3_area_entered(area): #Allows player to move again
	print("Player is now able to move")
	if area.name == "PlayerArea":
		threshHold = true
		flyingActive = false
