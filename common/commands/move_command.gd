class_name MoveCommand
extends Command

class Params:
	var input: float
	
	func _init(input: float) -> void:
		self.input = input


func execute(actor: Actor, data: Object = null) -> void:
	if data is Params:
		actor.move(data.input)
