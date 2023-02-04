using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public static List<Portal> portals = new List<Portal>();


    private void Awake()
    {
        //RenderPipelineManager.beginCameraRendering += RenderPortal;
    }

    public void RenderPortal(ScriptableRenderContext context, Camera camera)
    {
        for (int i = 0; i < portals.Count; i++)
        {
            //portals[i].PrePortalRender();
        }
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].Render(context);
        }

        for (int i = 0; i < portals.Count; i++)
        {
            //portals[i].PostPortalRender();
        }

    }
}
