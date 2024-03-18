extends MeshInstance3D

var TestEnemyAIBody
# Called when the node enters the scene tree for the first time.
func _ready():
	TestEnemyAIBody = $"Test Enemy AI"


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	TestEnemyAIBody.position = self.position
