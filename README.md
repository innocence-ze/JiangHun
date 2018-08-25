# JiangHun
tencent next Idea

推荐使用Unity2018.2

# 如何搭建地图的prefab
## 关卡模式
1 添加gamemanager，挂载AddLineList.cs   Map.cs   Click.cs  R_GameManager.cs这四个脚本

2 添加地图上需要的点(Prefabs/Point)

3 对组件Node(script)进行修改，添加它的NearNode

4 对gamemanager中的组件进行修改。

AddLineList中，EachLine_node的size为固定加载几步线（最初存在的线也算一步），array中的size为当前这一步需要加载的线的条数*2，eg（最初存在3条线，则Element0中Array的size为6，Array下的几个Element依次填入三条线的两个端点，element0、1为第一条线的两个端点...）

Click中的ClickStep为玩家可以点击消除边的次数，消除边减一，为0时无法消除边

R_GameManager中的R_Step为随机生成边的步数，如本关不需要随机生成，则为0。RandomIndex为每一步随机生成时，生成的边的条数。OverPanel为UI

5 UI层，Canvas下的NextButton为下一步按钮，在其Button组件的OnClick上绑定R_Gamemanager.NextStep方法。

##无尽模式
1 添加gamemanager，挂载Map.cs  Click.cs  E_GameManager.cs这三个脚本

2，3，4同上

E_GameManager中的RandomIndex为每一步随机生成时，生成的边的条数。addClick为每一步增加的点击次数。
