[简体中文](README.md) | [English](README.en-US.md) | [日本語](README.ja-JP.md) | **한국어** | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | [Português (Brasil)](README.pt-BR.md) | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow++ 2.0

Windows용 마우스 클릭 물결 미화 도구. 왼쪽/가운데/오른쪽/X1/X2 클릭마다 커서 위치에 색상이 다른 물결을 표시합니다.

![물결 데모](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![설정](docs/screenshot-settings.png)

> [cuiliang/ClickShow](https://github.com/cuiliang/clickshow)(1.4.1 기반)의 모던 리라이트 버전. 원작자에게 감사드립니다. UI와 기능을 전면 재작성했으며 Windows 10/11 전용입니다.

## 기능

### 클릭 물결
- 5개 버튼(좌/중/우/X1/X2)별 색상 및 on/off 개별 설정, 컬러 피커와 16진수 입력 지원；
- **8가지 색상 프리셋 테마**: 클래식 / 네온 나이트 / 캔디 / 오션 / 선셋 / 포레스트 / 베리 / 마카롱 원클릭 적용；

![테마 프리셋](docs/theme-presets.png)

- **애니메이션 길이** 200–600ms 조절；
- **5가지 확산 스타일**: 등속 / 감속(자연) / 강한 감속(물결) / 살짝 튕김 / 탄성；
- 밝은 배경과 어두운 배경 미리보기에서 효과를 즉시 확인.

### 라이트 / 다크 프로필
라이트와 다크 외관을 독립 저장(크기·색상·길이·스타일). 시스템 테마를 자동 따라가거나 한쪽으로 고정 가능.

### 마우스 점
커서를 따라다니는 원형 표시. 크기와 색상을 조절할 수 있고 독립적인 on/off.

### 무음
- **전체 화면 자동 숨김**: 포그라운드 창이 화면을 가득 채우면 표시하지 않아 게임과 영상을 방해하지 않음；
- **지정 프로그램 무음**: 목록에 추가한 프로그램(직접 입력 / 찾아보기 / 포그라운드 캡처)이 포그라운드이면 항상 숨김. 

### UI 언어
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. 첫 실행 시 시스템 언어를 따르며 설정에서 변경 가능. 

### 트레이
- 트레이 아이콘을 왼쪽 클릭하면 설정 열림；
- 우클릭 메뉴: 설정 열기 / 물결 일시중지 / 종료；
- 일시중지는 즉시 적용되고 해제해도 설정은 그대로. 

## 시스템 요구 사항

Windows 10 1809+ 또는 Windows 11(x64). 런타임 포함으로 **.NET 설치 불필요**. 

## 설치

**버전 선택**: `ClickShow-Setup-2.0.0.exe`는 .NET 런타임을 포함(약 63MB)해 Windows 10/11에서 바로 실행됩니다. `ClickShow-Setup-2.0.0-lite.exe`는 약 2MB이지만 **.NET 8 데스크톱 런타임(x64)이 반드시 필요**하며, 없으면 실행되지 않습니다(설치 프로그램이 감지해 다운로드 안내). 확실하지 않으면 전체 버전을 선택하세요.

포터블도 두 가지: `ClickShowPlusPlus-Portable-2.0.0.zip`(전체, 약 63MB)과 `ClickShowPlusPlus-Portable-2.0.0-lite.zip`(Lite, 약 0.6MB, 동일하게 런타임 필요).

1. 이 저장소의 Releases 페이지에서 ClickShow-Setup-2.0.0.exe 다운로드；
2. 마법사 실행: 사용자 지정 경로 지원(드라이브 루트 선택 시 자동으로 폴더 추가). 바탕 화면/시작 메뉴 바로 가기는 선택 사항；
3. 이전 버전이 실행 중이면 종료를 안내합니다. 

포터블 버전: ClickShowPlusPlus-Portable-2.0.0.zip 다운로드 후 압축 해제하여 바로 사용. 설치 불필요. 설정은 %APPDATA%/ClickShow에 저장. 

## 1.x에서 업그레이드

- 첫 실행 시 기존 설정(문서/ClickShow.setting)이 %APPDATA%/ClickShow/setting.json으로 자동 마이그레이션되며 색상과 설정이 유지됩니다；
- 라이트/다크 프로필은 동일한 값으로 시작；
- 덮어쓰기 설치해도 설정과 시작 프로그램 등록이 유지됩니다. 

## 빌드

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

설치 패키지: [Inno Setup 6](https://jrsoftware.org/isinfo.php)로 installer/ClickShow.iss 컴파일. 

## FAQ

- **전체 화면에서 물결이 안 보임?** "전체 화면 자동 숨김"이 동작 중입니다. 설정에서 끌 수 있음；
- **트레이의 "물결 일시중지"가 회색?** 마우스 후크 설치 실패(드묾). 앱을 다시 시작하세요；
- **제거 후 시작 프로그램 항목이 남음?** 제거 프로그램이 레지스트리 항목을 자동 정리합니다. 

## 변경 이력

### 2.0.0(2026-10)
- UI 전면 재작성: 단일 창 설정, 시스템 추종 라이트/다크 테마, 10개 언어 UI；
- 물결 렌더링 재작성: 스케일 변환 애니메이션(고주사율에서 풀 프레임), 벡터 안티에일리어싱, 길이와 확산 스타일 조절；
- 색상 프리셋 8종, 라이트/다크 독립 프로필；
- 전체 화면 자동 숨김과 프로그램별 무음；
- 컬러 피커 + 16진수 입력, 트레이 일시중지, 설정 가져오기/내보내기；
- .NET Framework 4.7.2 → .NET 8(자체 포함 단일 파일), Inno Setup 설치 패키지. 

### 1.4.1 이전
[원본 프로젝트 릴리스](https://github.com/cuiliang/clickshow/releases) 참조. 

## 라이선스

원본 프로젝트 라이선스 따름(LICENSE 참조). 원작자 [cuiliang](https://github.com/cuiliang)에게 감사드립니다.
