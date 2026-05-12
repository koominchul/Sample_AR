# Sample_AR

Unity 기반 AR 샘플 프로젝트입니다.

## Requirements

- **Unity 6000.3.14f1** (Unity 6)
- **Vuforia Engine 11.4.4** — 본 저장소의 `Packages/com.ptc.vuforia.engine-11.4.4.tgz`로 포함되어 있으며,
  Unity Package Manager가 자동 인식합니다. ([Vuforia Developer Portal](https://developer.vuforia.com/))
- **OpenCVForUnity** (유료 에셋) — **본 저장소에 포함되지 않습니다.**
- **UISystemPackage** — `manifest.json`에서 git URL로 참조 (자동 설치)
- **UniTask** — `manifest.json`에서 git URL로 참조 (자동 설치)
- **LitMotion** — `manifest.json`에서 git URL로 참조 (자동 설치)
- **LitMotion.Animation** — `manifest.json`에서 git URL로 참조 (자동 설치)
- **UI Effect** — `manifest.json`에서 git URL로 참조 (자동 설치)

## Setup

### 1. 저장소 클론

```bash
git clone https://github.com/koominchul/Sample_AR.git
```

### 2. OpenCVForUnity 별도 import (필수)

본 프로젝트는 [OpenCVForUnity (Enox Software)](https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088) 유료 에셋에 의존합니다. 라이선스 보호와 저장소 용량 절약을 위해 git에 포함되어 있지 않습니다.

> ⚠️ **이 프로젝트를 빌드/실행하려면 OpenCVForUnity 에셋을 본인 명의로 구매한 라이선스가 있어야 합니다.**

설치 방법:

1. Unity Asset Store에서 OpenCVForUnity 구매
2. Unity Editor에서 프로젝트 오픈
3. `Window → Package Manager → My Assets`에서 OpenCVForUnity import
4. import 경로가 `Assets/OpenCVForUnity/`인지 확인

OpenCVForUnity가 import되지 않은 상태에서는 `Assets/Scripts/AR/ColorMapping/AirarManager.cs` OpenCV 의존 코드의 컴파일 에러가 발생합니다.

### 3. Unity Editor에서 프로젝트 열기

`Assets/Scenes/_Intro.unity` 씬을 열어 사용합니다.


## License

본 저장소의 코드는 작성자 소유입니다.

OpenCVForUnity 제공자의 라이선스 약관을 따릅니다.

Vuforia Engine plans중 Basic Plan 기능만 사용합니다.(https://www.ptc.com/en/products/vuforia/vuforia-engine/pricing)
