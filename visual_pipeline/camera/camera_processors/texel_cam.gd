extends CameraProcesser
class_name texelCamBase

@export_group("Dependencies")
@export var viewport: SubViewport
@export var display_rect : ColorRect

var texel_size : float
		
func process_cam(_dir : float, current_position : Vector3, _delta : float, camera : Camera3D) -> Vector3:
	# Update texel size, if it's changed
	texel_size = calculate_texel_size(camera, viewport)
	
	# apply snap
	var snap_result = get_texel_snapped_pos(current_position, camera.basis)
	update_display(snap_result)
	return snap_result.snapped_world_pos
	
func calculate_texel_size(camera : Camera3D, viewport : SubViewport) -> float:
	return camera.size / viewport.size.y
	
class SnapResult:
	var snapped_world_pos : Vector3
	
	# 0 to 1, texel snap offset in uv space
	var snap_texel_offset_uv : Vector2
	
	func _init(pos : Vector3, snap_texel_offset_uv : Vector2):
		self.snapped_world_pos = pos
		self.snap_texel_offset_uv = snap_texel_offset_uv
	

func get_texel_snapped_pos(world_pos : Vector3, cam_basis : Basis) -> SnapResult:
	# Create rotation space matrix
	var rot_space_transform = Transform3D(cam_basis)
	var rot_space_transform_inv = rot_space_transform.inverse()
	
	# transform pos to rotation space
	var local_pos = rot_space_transform_inv * world_pos
	
	# snap
	var snapped_x = int(local_pos.x / texel_size) * texel_size
	var snapped_y = int(local_pos.y / texel_size) * texel_size
	
	# calculate offset for use in sub-texel camera
	var snap_texel_offset_uv = Vector2(snapped_x - local_pos.x, snapped_y - local_pos.y) / texel_size
	
	snap_texel_offset_uv.x /= viewport.size.x
	# negate y since UV has y increasing downwards
	snap_texel_offset_uv.y /= -viewport.size.y
	
	var snapped_local_pos = Vector3(
		snapped_x,
		snapped_y,
		local_pos.z
		)
		
	# transform back to world space
	var final_pos = rot_space_transform * snapped_local_pos
	
	var result = SnapResult.new(final_pos, snap_texel_offset_uv)
	return result
	
func update_display(snap_result : SnapResult):
	# update the display rect to match snap offset
	display_rect.material.set_shader_parameter("texel_snap_uv_offset", snap_result.snap_texel_offset_uv)
