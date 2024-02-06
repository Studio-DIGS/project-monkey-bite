extends Node3D

func _ready():
	pass


func _on_audio_stream_player_finished():
	queue_free()
