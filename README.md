# JiangHun
tencent next Idea

推荐使用Unity2018.2

# 如何搭建地图的prefab
## 关卡模式
1 添加gamemanager挂载AddLineList.cs   Map.cs   Click.cs  R_GameManager.cs这四个脚本
2 添加地图上需要的点(Prefabs/Point)
3 对组件Node(script)进行修改，添加它的NearNode
4 对gamemanager中的组件进行修改
4.1 AddLineList中，EachLine_node的size为固定加载几步线（最初存在的线也算一步）
