using UnityEngine;
using Vuforia;

public class ColorMappingBase : MonoBehaviour
{
    [SerializeField] protected Material transparentMat;
    [SerializeField] protected int realWidth, realHeight;

    protected GameObject trackedObj;
    protected ImageTargetBehaviour imageTaget;



    void Start()
    {
        imageTaget = GetComponent<ImageTargetBehaviour>();
        trackedObj = CreateCubeForVuforiaTarget(gameObject, imageTaget.GetSize().x, imageTaget.GetSize().y);
    }

    public Texture2D GetProcessColoredMapTexture()
    {
        float[] srcValue = AirarManager.Instance.CalculateMarkerImageVertex(trackedObj);
        Texture2D screenShotTex = ScreenShot.GetScreenShot(trackedObj);
        return AirarManager.Instance.ProcessColoredMapTexture(screenShotTex, srcValue, realHeight, realWidth);
    }

    protected GameObject CreateCubeForVuforiaTarget(GameObject parentObj, float targetWidth, float targetHeight)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material = transparentMat;
        cube.transform.SetParent(parentObj.transform);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localScale = new Vector3(targetWidth, 0.001f, targetHeight);
        return cube;
    }

    public void SetDisableTrackedObj()
    {
        trackedObj.GetComponent<Renderer>().enabled = false;
    }
}
