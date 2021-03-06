﻿using System;
using System.Numerics;
using DirectX12GameEngine.Rendering.Lights;
using DirectX12GameEngine.Shaders;

namespace DirectX12GameEngine.Rendering.Materials
{
    [StaticResource]
    public class MaterialDiffuseLambertModelFeature : IMaterialDiffuseModelFeature
    {
        public void Visit(MaterialGeneratorContext context)
        {
        }

        [ShaderMember]
        [ShaderMethod]
        public Vector3 ComputeDirectLightContribution()
        {
            Vector3 diffuseColor = MaterialPixelStream.MaterialDiffuseVisible;
            diffuseColor *= Vector3.One - MaterialPixelStream.MaterialSpecularVisible;

            return diffuseColor / (float)Math.PI * LightStream.LightColorNDotL;
        }

        [ShaderMember]
        [ShaderMethod]
        public Vector3 ComputeEnvironmentLightContribution()
        {
            return default;
        }
    }
}
