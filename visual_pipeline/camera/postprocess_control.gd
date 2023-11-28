extends Camera3D

@export var post_processing := true:
	set(p):
		if p:
			$Postprocess.show()
			post_processing = p
			var a = Vector3(-1, 1, 0).normalized()
			var b = Vector3(1, 0, 0).normalized()
			print("dot: ", a.dot(b))
		else:
			$Postprocess.hide()
			post_processing = p
			
func _process(_delta):
	RenderingServer.global_shader_parameter_set("view_basis_matrix", Transform3D(basis).inverse())
	RenderingServer.global_shader_parameter_set("inv_view_basis_matrix", Transform3D(basis))
	RenderingServer.global_shader_parameter_set("camera_proj_matrix", get_camera_projection())
	RenderingServer.global_shader_parameter_set("camera_inv_proj_matrix", get_camera_projection().inverse())
