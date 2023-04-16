 
void GetLightDIr_float(in float3 Normal, in float3 ClipSpacePos, in float3 WorldPos, out float3 Direction) {
	// set the shader graph node previews
	#ifdef SHADERGRAPH_PREVIEW
		Direction = float3(0.5,0.5,0);
	#else
		UNITY_INITIALIZE_OUTPUT(float3, Direction);
		// grab the shadow coordinates
		#if SHADOWS_SCREEN
			float4 shadowCoord = ComputeScreenPos(ClipSpacePos);
		#else
			float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif

		// grab the main light
		#if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
			Light light = GetMainLight();
		#else
			Light light = GetMainLight();
		#endif

		Direction = light.direction;
	#endif

}
