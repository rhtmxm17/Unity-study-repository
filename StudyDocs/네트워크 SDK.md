# 네트워크 SDK

## Photon

[기술 문서](https://doc.photonengine.com/)  
클라우드 상에 서버 앱을 생성하고 접속이 가능하다

### 프로젝트 설정

[기술 문서 > 시작하기](https://doc.photonengine.com/ko-kr/fusion/current/tutorials/host-mode-basics/1-getting-started)  
SDK는 [유니티 AssetStore](https://assetstore.unity.com/packages/tools/network/photon-fusion-267958)에서 추가해도 된다  
프로젝트에 추가 후 상단 Tools 메뉴에서 메인 Hub창을 찾을 수 있다

* 테스트 환경 구성 팁: [ParrelSync 패키지](https://github.com/VeriorPies/ParrelSync)  
  Assets와 ProjectSettings를 바로가기 형태로 공유하는 복제 프로젝트를 만들어준다. 에디터 플레이모드로 네트워크 테스트하기에 유용

### 주요 기능

* Network Runner 컴포넌트: 네트워크 연결을 실제로 유지 관리하는 관리자 컴포넌트

* Network Object 컴포넌트: 네트워크 상에서 공유될 정보를 포함한다는 것을 지정
* Network Transform 컴포넌트: 네트워크 상에서 Transform을 공유하는 기능

* SimulationBehaviour 클래스: 네트워크 기능을 추가로 구현하기 위한 베이스 클래스. Behaviour를 상속하고 있다.

* IPlayerJoined, IPlayerLeft 등의 인터페이스:
  레이어 접속, 종료 등 특정 상황이 발생했을 경우 Network Runner가 같은 GameObject가 갖는 해당 인터페이스를 호출한다
