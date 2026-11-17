using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

namespace LongRoad.Core.RenderFeature
{
    public class PixelationFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            [Tooltip("Материал с вашим шейдером пикселизации (ScreenPixelization)")]
            public Material material;

            [Tooltip("Точка внедрения эффекта. BeforeRenderingPostProcessing работает лучше всего.")]
            public RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public Settings settings = new Settings();
        private PixelationPass m_ScriptablePass;

        public override void Create()
        {
            m_ScriptablePass = new PixelationPass(settings.material)
            {
                renderPassEvent = settings.injectionPoint
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            // Если материал не назначен, ничего не делаем
            if (settings.material == null) return;

            // Проверка: если это камера Scene View или Preview, пропускаем эффект
            if (renderingData.cameraData.isSceneViewCamera || renderingData.cameraData.isPreviewCamera)
                return;

            m_ScriptablePass.SetMaterial(settings.material);
            m_ScriptablePass.renderPassEvent = settings.injectionPoint;

            renderer.EnqueuePass(m_ScriptablePass);
        }

        class PixelationPass : ScriptableRenderPass
        {
            private Material m_Material;

            public PixelationPass(Material mat)
            {
                m_Material = mat;
            }

            public void SetMaterial(Material mat)
            {
                m_Material = mat;
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                if (m_Material == null) return;

                var resourceData = frameData.Get<UniversalResourceData>();
                var source = resourceData.activeColorTexture;

                if (!source.IsValid()) return;

                // 1. Создаем временную текстуру
                var desc = renderGraph.GetTextureDesc(source);
                desc.name = "PixelationTemp";
                desc.clearBuffer = false;
                desc.depthBufferBits = 0;

                var temp = renderGraph.CreateTexture(desc);

                // 2. Применяем материал пикселизации (source -> temp)
                var blitParams = new RenderGraphUtils.BlitMaterialParameters(source, temp, m_Material, 0);
                renderGraph.AddBlitPass(blitParams, "Pixelation Pass");

                // 3. Копируем результат обратно (temp -> source)
                renderGraph.AddCopyPass(temp, source, passName: "Copy Back Pass");
            }
        }
    }
}
