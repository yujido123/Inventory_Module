# Inventory_Module
First person perspective inventory module

Effect screenshot below:
![Inventory Preview](builds/Inventory_Preview.png)


Thought list:
1.Raycast找到面对的物体
2.按F捡起物体
    (1) 销毁物体
    (2) 在背包中找到第一个空位并Instantiate Preview
3.物体在背包内拖拽
    (1) OnBeginDrag，OnDrag，OnDrop
    (2) drop的slot有物体就调换，没有就直接放下
4.物体拖到背包外，丢出物体
    (1) 销毁背包里的item
    (2) Instantiate真正的物体丢出

