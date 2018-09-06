// Copyright 2014-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace BlendModes
{
    public abstract class MaskableGraphicExtension<TComponent> : ComponentExtension where TComponent : MaskableGraphic
    {
        protected Material DefaultMaterial { get { return GetDefaultMaterial(); } }

        // UI elements use single material for rendering; this collection is used to prevent allocs.
        private readonly Material[] renderMaterials = new Material[1];

        public override void OnEffectEnabled ()
        {
            GetExtendedComponent<TComponent>().RegisterDirtyMaterialCallback(SetBlendMaterialDirty);
        }

        public override void OnEffectDisabled ()
        {
            if (!IsExtendedComponentValid) return;
            GetExtendedComponent<TComponent>().UnregisterDirtyMaterialCallback(SetBlendMaterialDirty);
            GetExtendedComponent<TComponent>().material = DefaultMaterial;
        }

        public override Material[] GetRenderMaterials ()
        {
            renderMaterials[0] = GetExtendedComponent<TComponent>().materialForRendering;
            return renderMaterials;
        }

        public override void SetRenderMaterials (Material[] materials)
        {
            var material = materials != null && materials.Length > 0 ? materials[0] : DefaultMaterial;
            GetExtendedComponent<TComponent>().material = material;
        }

        protected virtual Material GetDefaultMaterial ()
        {
            return GetExtendedComponent<TComponent>().defaultMaterial;
        }
    }
}
