# 데이터 읽고 쓰기(CSV, JSON)와 ScriptableObject

게임에 사용되는 각종 수치들을 빌드시에 결정하는 대신, 데이터 테이블에서 읽어와서 사용하는 방법에는 여러가지 장점이 있다.

* 단순 수치 변경을 위해 다시 빌드할 필요가 없다.
* 프로젝트 구성을 모르는 인원도 수치를 조정할 수 있어서 협업에 유리하다.
* 기획단계에서 사용할 정보들을 엑셀 형태로 준비하기 좋다.
* DB와의 연계

## CSV 파일

각 열이 `,`로, 각 행이 줄바꿈으로 나누어진 데이터 테이블. 문자열로만 구성된 테이블이기 때문에 프로그램상에서 쉽게 읽어올 수 있다.  
데이터 자체는 엑셀로 관리하다가 엑셀 파일을 .csv 파일로 내보내서 서식 등의 게임에서 읽어오는데 불필요한 데이터를 제거한 형태로 사용할 수 있다.

* tsv 파일: `,` 대신 탭(\t)으로 구분한 파일

## JSON

문자열로 내용을 확인 가능한 형태로 데이터를 직렬화 하는 포맷. 유니티에서는 JsonUtility 클래스를 통해 사용한다.

## 스크립터블 오브젝트

유니티에서 인스펙터 데이터를 통해 직렬화, 저장(**에디터 한정**), 불러오기를 관리하는 클래스이다.  
에셋 포맷으로 데이터를 저장한다. 에셋이기 때문에 유니티 엔진에서

* 인스펙터 창을 사용한 데이터 변경
  * 색상, 커브와 같은 전용 UI 사용
  * TextArea와 같은 어트리뷰트 사용
* prefab 참조 저장
* 인스펙터 창을 통한 참조

와 같은 사용이 가능하며, 동시에 파일로 저장했기 때문에 빌드 없이 내용을 변경할 수 있다.

원본 데이터 하나를 참조하는 여러 인스턴스 형태로 구현하는 것으로 자연스럽게 플라이웨이트 패턴이 적용된다.

### 작성

ScriptableObject를 상속해서 데이터를 담을 클래스를 작성하고, CreateAssetMenu 어트리뷰트를 추가한다

```C#
// Asset에서 우클릭 - Create 탭에 메뉴를 추가하는 어트리뷰트
// fileName: 생성시 파일명 지정
[CreateAssetMenu(fileName = "New Quest Data", menuName = "ScriptableObject/Quest Data")]
public class QuestData : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField, TextArea] private string description;
    [SerializeField] private int reward;
    [SerializeField] private Texture icon;
    [SerializeField] private Color color;
    [SerializeField] private GameObject prefab;

    public string Title { get => title; set => title = value; }
    public string Description { get => description; }
    public int Reward { get => reward; }
    public Texture Icon { get => icon; }
    public Color Color { get => color; }

    public GameObject ClonePrefab()
    {
        return Instantiate(prefab);
    }
}

```

### 주의사항

ScriptableObject를 비롯한 Asset 폴더 내의 데이터는 빌드 후 배포된 실제 게임에서는 읽어오기만 가능하다(엄밀히는 환경에 따라 다르기 때문에 일괄적으로 읽어오기만 한다). 따라서 인게임 데이터를 저장하는데에는 사용이 불가능하고, 배포 전에 미리 생성하는 데이터를 만드는데 사용한다.

게임을 설치, 업데이트 할 때에 관리자 권한을 요구하는 것이 본래 쓰기가 불가능한 해당 영역의 데이터를 덮어쓰기 위한 것인듯 하다.

## 경로 탐색

* Application 클래스: [Scripting API](https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Application.html)  
    여러 환경에서 공통적으로 경로 정보를 가져올 수 있음.
  * dataPath: 게임 데이터 폴더 경로. 몇몇 플랫폼에서는 읽기 전용임!! 런타임에 데이터를 저장하는데는 못쓰고 설치할때부터 적용되는 데이터를 읽어오기만 가능
  * persistentDataPath: 쓰기 가능한 폴더 경로. 사용자 설정 등 저장에 사용할 수 있다.
* 파일 입출력: System.IO를 쓴다. 과거에는 포맷별로 다른 네임스페이스의 메서드를 사용해야 했다고 한다.
