![](https://capsule-render.vercel.app/api?type=waving&height=250&color=f7ef68&text=국가%20권력급%20곡괭이%20⛏️)

# 🖥 프로젝트 소개

### ⛏ 국가권력급 곡괭이 ⛏
- 플레이어가 수집한 자원으로 곡괭이를 강화하여 강력한 보스를 처치하는 2D RPG 게임입니다.

### ⏲ 개발기간
- 총 기간 : 2024.06.27(목) ~ 2024.07.24(수)
  
| 구분 | 기간 |
| --- | --- |
| 사전 기획 | 06.27(목) ~ 07.01(월) |
| 프로젝트 기본 구조 설계 | 07.02(화) ~ 07.05(금) |
| 필수 구현 사항 개발 | 07.08(월) ~ 07.15(월) |
| 중간 회의 | 07.16(화) ~ 07.16(화) |
| 추가 구현 사항 | 07.17(수) ~ 07.21(일) |
| 건의 사항 및 오류 수정 | 07.22(월) ~ 07.24(수) |
| 추가 기능 구현 및 최적화 | 07.25(목) ~ 08.08(목) |
| 유저 테스트 진행 | 08.09(금) ~ 08.18(일) |
| 버그 수정 및 기능 개선 | 08.19(월) ~ 08.21(수) |

### ⚙ 개발 환경
- **Game Engine**:  ![Unity](https://img.shields.io/badge/Unity-2022.3.17f-000000?style=flat&logo=unity)
- **IDE**: ![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91?style=flat&logo=visual-studio&logoColor=white)
- **Language**:  ![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=c-sharp&logoColor=white)
- **VCS**: ![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)

### 👪 팀원 및 역할
#### 팀장 : [금재은](https://github.com/JaeEun18)
- Item, Inventory, QuickSlot
  
#### 부팀장 : [임재훈](https://github.com/limjh0222)
- Manager, Upgrade, Data Management
  
#### 팀원 : [박신후](https://github.com/SinHoo99)
- Player, MonsterAI, Boss

#### 팀원 : [진강산](https://github.com/MothorMoth)
- Map, Scene, UI

### 🖼 와이어 프레임

<div align = "center"> 

![image](https://github.com/user-attachments/assets/0a89b629-10e2-40b7-b52a-8b5ca26b4439)

</div>

### 📹 시연 영상

<div align = "center"> 

<details> 
  
<summary> ⛏️ 시연 영상 ⛏️ </summary>

[시연 영상 유튜브 링크](https://www.youtube.com/watch?v=Dl3bSj9uupQ)

</details>

</div>

### 🛠 사용한 기술
#### 유닛
- **FSM** : 상태 관리를 체계적으로 수행하여 게임 로직의 복잡성을 줄이고, 유지보수성을 향상시키기 위해 선택
- **InputSystem** : 다양한 입력 장치에 유연하게 대응하고, 입력 처리를 최적화하기 위해 선택
- **NavMeshPlus** : 2D 환경에서 AI의 경로 탐색을 자연스럽게 구현하고, 다양한 지형과 장애물 사이에서도 효율적인 경로 계산을 가능하게 하여 AI의 성능을 최적화하기 위해 선택

#### 카메라
- **Chinemachine** : 정교한 카메라 연출을 구현하기 위해 선택
- **CameraResolution** : 다양한 디바이스의 해상도에 대응하여 최적의 사용자 경험을 제공하기 위해 선택

#### 데이터
- **ScriptableObject** : 게임 데이터를 효율적으로 관리하고, 게임 오브젝트 간의 데이터 공유를 원활하게 하기 위해 선택
- **CSV** : 대량의 데이터를 직관적으로 처리하고, 데이터와의 연동을 용이하게 하기 위해 선택
- **Json** : 데이터를 구조화하여 유연하게 저장하고, 다양한 플랫폼 간의 데이터 호환성을 확보하여 효율적인 데이터 관리를 가능하게 하기 위해 선택

#### 애니메이션
- **DOTween** : 간단한 애니메이션에서 내장 애니메이션 시스템보다 성능적으로 우수하며, 간단하고 직관적인 문법으로 코드작성에 용이하여 선택

#### 오브젝트 생성
- **ObjectPool** : 메모리 할당 및 해제를 최소화하고, 게임의 성능을 최적화하여 보다 효율적인 리소스 관리를 달성하기 위해 선택

#### 매니저
- **Singleton** : 특정 클래스의 인스턴스를 전역적으로 하나만 유지하여 데이터 관리 및 접근을 효율화하고, 일관된 상태를 유지하기 위해 선택

### 📖 주요 기능
#### Tittle SCENE
- Play 버튼을 누르면 게임이 시작됩니다.
- Load Data 버튼을 누르면 저장된 데이터를 불러올 수 있습니다.
- Setting 버튼을 누르면 BGM과 SFX를 개별적으로 조절할 수 있습니다.
- Exit 버튼을 누르면 게임이 종료됩니다.

#### Loading SCENE
- Scene 전환 시 로딩창을 활성화시킵니다.

#### Forest SCENE
- 게임의 최종 목표는 곡괭이를 강화하여 보스를 처치하는 것입니다.
- 플레이어는 인게임 상 조이스틱을 통해 캐릭터를 조작할 수 있습니다.
- 우측 하단 대시 버튼을 누를 때마다 빠르게 달릴 수 있습니다.
- ForestScene 맵에는 다양한 자원을 획득할 수 있는 나무와 돌과 같은 플레이어가 상호작용할 수 있는 오브젝트 틀이 있습니다.
- 상호 작용을 통해 획득한 아이템은 인벤토리에서 확인할 수 있고, 소비 아이템만 사용하고 퀵슬롯에 등록할 수 있습니다.
- 인벤토리 상에 아이템을 클릭하면 해당 아이템의 이름, 아이템 설명을 확인할 수 있습니다.

#### Dungeon SCENE
- DungeonScene 맵에는 곡괭이를 강화할 수 있는 '광석 오브젝트'와 몬스터를 처치하고 획득할 수 있는 '강화 스크롤'이 있습니다.
- 곡괭이의 상호작용 레벨이 낮으면 각 다른 층의 광석과 상호작용할 수 없습니다.

#### Boss SCENE
- 일정 패턴으로 플레이어를 공격하고 플레이어는 이를 '대시'로 회피하면서 공격하여 보스를 클리어합니다.


![](https://capsule-render.vercel.app/api?type=rect&height=100&color=f7ef68&fontAlignY=50&descAlignY=60)
