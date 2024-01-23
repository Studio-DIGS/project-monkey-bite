extends Node3D

@export var camera: CameraSystem
@export var player: Player

@onready var anim = $CutsceneAnimations
@onready var cam_target = $CameraTarget

# Called when the node enters the scene tree for the first time.
func _ready():
	GameManager.connect("start_cutscene", _start_cutscene)

func _start_cutscene(cutscene):
	print("start cutscene")
	camera.emit_signal("change_target", cam_target)
	anim.play("scene1_1")
	
func _end_cutscene():
	camera.emit_signal("change_target")

func _on_cutscene_animations_animation_finished(anim_name):
	_end_cutscene()

func wait_for_dialogue():
	anim.pause()
	print("test dialogue")
	$Timer.start()
	await $Timer.timeout
	print("Thank you for listing")
	anim.play()
	
	
