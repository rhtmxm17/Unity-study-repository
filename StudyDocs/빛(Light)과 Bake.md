# 빛(Light)과 Bake  

## Lightmapping

Window-Rendering-Lighting-Scene

빛은 상당히 많은 그래픽 연산을 필요로 하기 때문에 매 프레임 빛에 대한 모든 연산을 하는것은 최대한 피해야 한다. 그 방법중 하나로 빛 그리고 빛이 비출 대상이 런타임에 위치가 변하지 않는다면 미리 그 대상에 대한 빛 계산을 컴파일 단계에 해두는 것으로 비용을 상당히 절약할 수 있다.  
미리 Baking 해둔 결과물이 Lightmap

* Lightmap Resolution: 계산해둘 Lightmap의 해상도 설정값

### GameObject: Static

해당 물체가 런타임에 움직이지 않을 것을 확정하는 것. Baking의 대상이 될 수 있게 해주기 때문에 움직이지 않을 물체라면 최적화에 필수적인 설정이다.

### Light: Mode

이 빛을 Baking 관점에서 어떻게 처리할지 결정하는 설정

* Realtime: 매 프레임 빛을 연산한다. 빛 자체가 움직여야 하는 경우에는 사용할 수 밖에 없다.
* Baked: 미리 이 빛에 영향을 받는 Static 오브젝트에 빛을 그려두고 런타임에는 실제로 동작하지 않는다. 배경 요소와 같이 움직이는 물체가 들어올 일이 없는 빛에 사용한다.
* Mixed: Static 오브젝트에 대해서는 그려두고 그렇지 않은 물체에 대해서만 빛을 계산한다. 설정에 따라서는 그림자가 제대로 그려지지 않는 단점이 있다.

## Light Probe

여러 Probe 지점에서 받는 빛의 양을 미리 계산해서, 그 영역에 들어온 물체가 받을 빛을 실제로 계산하는 대신 주위 Probe의 정보로부터 보간해서 구하는 기능. 빛이 많이 변하는 경계면 앞 뒤로 Probe들을 배치하는 것이 효과가 좋다.

## Reflection Probe

지정한 Probe 지점에서 주위 영역을 찍어서, 영역 내의 다른 물체에서 그 영역을 반사하듯 비출 수 있게 하는 Component. 이것 또한 사용하려면 미리 매핑해두는 것이 반 필수적이다.

## Enviroment Lighting

Window-Rendering-Lighting-Enviroment

환경광, 주변광. 태양의 난반사와 같이 세계에 깔려있는 평균적인 빛의 설정. Material의 Occlusion Map이 후드 안쪽과 같이 어두워야 하는 구석진 위치에 Enviroment Lighting을 차폐하는 것 같다.
