using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityIntegration;
using Luck9kr.Uisystem;

public class AirarManager : SingletonMonoBehaviour<AirarManager>
{
    /// <summary>
    /// OpenCV를 사용하여 특정 영역을 잘라내고 보정된 텍스처를 반환합니다.
    /// </summary>
    public Texture2D ProcessColoredMapTexture(Texture2D srcTexture, float[] srcPoints, int width, int height)
    {
        // 1. Texture2D를 Mat으로 변환
        Mat srcMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC4);
        OpenCVMatUtils.Texture2DToMat(srcTexture, srcMat);

        // 2. 소스 좌표 설정 (float[] -> MatOfPoint2f)
        // srcPoints 순서: LU(0,1), RU(2,3), RD(4,5), LD(6,7)
        MatOfPoint2f srcPointsMat = new MatOfPoint2f(
            new Point(srcPoints[0], srcPoints[1]),
            new Point(srcPoints[2], srcPoints[3]),
            new Point(srcPoints[4], srcPoints[5]),
            new Point(srcPoints[6], srcPoints[7])
        );

        // 3. 목적지 좌표 설정 (결과물의 사각형 좌표)
        MatOfPoint2f dstPointsMat = new MatOfPoint2f(
            new Point(0, 0),
            new Point(width, 0),
            new Point(width, height),
            new Point(0, height)
        );

        // 4. Perspective 변환 행렬 계산
        Mat perspectiveTransform = Imgproc.getPerspectiveTransform(srcPointsMat, dstPointsMat);

        // 5. Warp Perspective 적용 (이미지 변환)
        Mat dstMat = new Mat(height, width, CvType.CV_8UC4);
        Imgproc.warpPerspective(srcMat, dstMat, perspectiveTransform, new Size(width, height));

        // 6. 결과 Mat을 Texture2D로 변환
        Texture2D resultTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        OpenCVMatUtils.MatToTexture2D(dstMat, resultTexture);

        // 7. 메모리 해제 (OpenCV 객체는 Dispose가 중요)
        srcMat.Dispose();
        dstMat.Dispose();
        perspectiveTransform.Dispose();
        srcPointsMat.Dispose();
        dstPointsMat.Dispose();

        string imgPath = Path.Combine(Application.persistentDataPath, "airar.jpg");
        File.WriteAllBytes(imgPath, resultTexture.EncodeToJPG());

        return resultTexture;
    }

    public float[] CalculateMarkerImageVertex(GameObject cube)
    {
        List<Vector2> vertexList = new List<Vector2>();
        MeshFilter meshFilter = cube.GetComponent<MeshFilter>();

        if (meshFilter == null) return null;

        Vector3[] vertices = meshFilter.mesh.vertices;

        // 메쉬의 모든 정점을 스크린 좌표로 변환
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 worldPos = cube.transform.TransformPoint(vertices[i]);
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            vertexList.Add(screenPos);
        }

        int screenHeight = Screen.height;
        Vector2 LU = vertexList[14];
        Vector2 RU = vertexList[13];
        Vector2 RD = vertexList[12];
        Vector2 LD = vertexList[15];

        float[] srcPosition = new float[8];

        srcPosition[0] = LU.x;
        srcPosition[1] = screenHeight - LU.y;

        srcPosition[2] = RU.x;
        srcPosition[3] = screenHeight - RU.y;

        srcPosition[4] = RD.x;
        srcPosition[5] = screenHeight - RD.y;

        srcPosition[6] = LD.x;
        srcPosition[7] = screenHeight - LD.y;

        return srcPosition;
    }




}
