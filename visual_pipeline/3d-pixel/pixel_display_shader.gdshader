shader_type canvas_item;

// Viewport texture to render to screen
uniform sampler2D viewport_tex : filter_nearest;

// 0 to 1, the offset needed to snap the camera to a pixel grid
uniform vec2 texel_snap_uv_offset;

void fragment() 
{
	// Pixel snap undo
	vec2 uv = UV;
	uv -= texel_snap_uv_offset;
	
	// Ramp texture
	vec4 color = texture(viewport_tex, uv);
	
	COLOR = color;
}
