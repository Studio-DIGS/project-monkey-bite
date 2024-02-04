class_name CutsceneController
extends Controller

var actor: Actor

var move_command := MoveCommand.new()

func _init(actor: Actor):
	self.actor = actor

func move(move_input: float):
	move_input = clamp(move_input, -1, 1)
	move_command.execute(actor, MoveCommand.Params.new(move_input))
