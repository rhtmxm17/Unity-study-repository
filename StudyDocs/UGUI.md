# UGUI

유니티의 기본 제공 UI 시스템이다.

## 크기와 위치 설정(RectTransform)

UI는 2D상에서의 위치와 크기 설정에 특화된 RectTransform을 사용해 배치한다.

### Anchor

위치 좌표를 설정하기위한 기준 위치. 각 UI요소마다 화면 가운데, 오른쪽 위, 하단 가운데 등 제각각의 위치를 잡을 기준을 설정할 수 있다. Anchor 자체의 위치는 캔버스 전체 크기를 기준으로 좌하단에서부터 우상단까지 x, y 각각 0~1로 잡는다.

### Pivot

크기를 설정하기 위한 기준 위치. 너비를 2배로 늘린다면 Pivot 위치를 기준으로 왼쪽으로 2배, 오른쪽으로 2배 떨어진 좌표까지 늘어난다.

## EventSystem

UI의 입력, 상태 변경 등 각종 동작을 관리하는 오브젝트.

## Button

* Navigation: 방향 입력으로 버튼 포커스를 옮기는 기준이다. 패드 입력에서 방향키로 버튼을 고르는 경우나 양손이 키보드에 놓이는 게임에서 UI 선택을 편하게 하는 등에 사용

## Image와 RawImage

캔버스에 스프라이트 이미지(텍스처 타입 설정 중 하나)를 그려주는 UI 컴포넌트.

## TextMeshPro

텍스트 출력에 사용한다. 레거시 텍스트에 비해 기능이 매우 다양하다.  
폰트 사용시 폰트 파일로부터 폰트 에셋을 만들어야 한다. TMPro 네임스페이스를 통해 코드상에서 사용할 수 있다.

### 폰트 애셋 생성시 참고사항

Window - TextMeshPro - Font Asset Creater 윈도우  
Character Set 에서 Custom Range를 선택해야 ASCII 범위 바깥을 포함시킬 수 있다.  
Character Sequence에서 유니코드 단위의 10진수로 사용할 문자 범위를 지정한다.

* 영문자 범위: 32-126
* 한글 범위: 44032-55203
* 한글 자모음 범위: 12593-12643
* 특수문자 범위: 8200-9900

## 공통 요소

### RayCastTarget

해당 이미지, 텍스트가 클릭 RayCast를 받아들일지 통과시킬지 결정하는 프로퍼티이다. 선택 영역 앞에 데코레이션 등을 띄울 경우 해제해줘야 정상적으로 뒤의 선택 영역(버튼 등)을 선택할 수 있다.

***
***

## MVC 패턴

체력바와 같이 데이터를 **보여주기만을 위한** UI에서 **데이터를 변경하는** 의도치 않은 동작을 확실하게 막기 위한 패턴.

임의의 데이터에 대해 실제 데이터인 모델`Model`, 데이터 갱신을 통지받는 관찰자`View`, 데이터를 갱신하는 사용자`Controller`로 나누어 관리하는 패턴.

```C#
// Controller는 Value를 사용하고
// View는 OnChanged를 구독한다
public class Model
{
    public UnityEvent<int> OnChanged;

    private int value;
    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            OnChanged?.Invoke(value);
        }
    }
}
```

## MVP 패턴

유니티에 조금 더 적합한 적합한 MVC 패턴의 변형. 사용자`Controller` 대신 중개자`Presenter`를 둔다. MVC 방식은 관찰자마다 모델의 구독을 위한 코드가 필요해지기 때문에, 관찰자(UI)들을 작동시켜주는 중개자를 두는 것이다.  
관찰자마다 모델한테 "값 바뀌면 알려줘!" 하던 코드를 관찰자는 별 동작을 안하고(UI에 구독을 위한 코드를 붙이지 않고) 중개자가 "값 바뀌면 이거 이거 이거 하게 알려줘" 하는 식으로(모델 근처에 붙인 컴포넌트의 코드) 작동하는것.

데이터 관리의 안전함 외에도, 유니티 인스펙터 상에서 데이터 자체와 데이터를 조회하는 오브젝트, 그리고 데이터를 다루고 있는 영역을 명확히 구분해서 확인하기 유리해 보인다.

모델의 변경과 통지를 더 명확히 분리하기 위해 사용자 자체는 그대로 두고 모델과 관찰자 사이에 중개자만 추가하는 형태도 가능할 듯 하다.