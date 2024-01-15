extends Node

signal NPC_text

var dialogue
var dialogue2
var regularLine1: String = "Regular: Ya know, my brother can be really too social sometimes."
var regularLine2: String = "Regular: We've been traveling together for years and he always manages to befriend the strangest people."
var regularLine3: String = "Regular: Look after him will you?"
var regularLine4: String = "Regular: I have faith in our team and believe we got a solid team that can implement it"
var regularLine5: String = "Regular: We just gotta pull together and progress through this all"

var questLine1: String = "Quest: hi my name Anthony Nguyen"
var questLine2: String = "Quest: I love studio DIGS, I think it is incredible because of the type of members in it"
var questLine3: String = "Quest: Sometimes I wonder how far we are going to get into developing this game"
var questLine4: String = "Quest: I have faith in our team and believe we got a solid team that can implement it"
var questLine5: String = "Quest: We just gotta pull together and progress through this all"

var endLine1: String = "End: I got nothing left to say, leave me~"

var questDialogue
var regularDialogue
var endDialogue



func _ready():
	questDialogue  = [questLine1, questLine2, questLine3]
	regularDialogue  = [regularLine1, regularLine2, regularLine3]
	endDialogue  = [endLine1]
func _process(_delta):
	pass

func _on_player_detection_box_area_entered(area):
	if (area.name == "PlayerArea"):
		emit_signal("NPC_text", questDialogue, regularDialogue, endDialogue)
