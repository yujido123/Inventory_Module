# Inventory_Module
First person perspective inventory module

Effect screenshot below:
![Inventory Preview](builds/Inventory_Preview.png)


Thought list:</br>
1.Raycast找到面对的物体</br>
2.按F捡起物体</br>
    (1) 销毁物体</br>
    (2) 在背包中找到第一个空位并Instantiate Preview</br>
3.物体在背包内拖拽</br>
    (1) OnBeginDrag，OnDrag，OnDrop</br>
    (2) drop的slot有物体就调换，没有就直接放下</br>
4.物体拖到背包外，丢出物体</br>
    (1) 销毁背包里的item</br>
    (2) Instantiate真正的物体丢出</br>

