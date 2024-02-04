class_name HumanController
extends Controller

var player: Player

var move_command := MoveCommand.new()
var attack_command := AttackCommand.new()
var jump_command := JumpCommand.new()

func _init(player: Player):
	self.player = player

func _physics_process(_delta):
	if Input.is_action_just_pressed("attack"):
		attack_command.execute(player)
	var move_input = Input.get_axis("left", "right")
	move_command.execute(player, MoveCommand.Params.new(move_input))
