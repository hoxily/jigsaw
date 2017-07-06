# jigsaw
a jigsaw game developed using unity3d
这是使用unity3d开发的拼图游戏。
## 基本玩法
游戏初始会打乱图片块，玩家可以自由拖动图片块，如果拖放后的两组图片块是原本相邻的并且当前位置非常接近，
那么它们就会合并成一个更大的团体。重复这个过程直到所有的图片块组合成为一个整体，玩家就获胜了。
## 增加趣味性与挑战性
- 遵循某种主题，所有拼图图片都是同一个主题下的图片，形成主题系列。
- 每一关卡可以设置评价星级。只有达到达标星级，才开放下一关卡。按照完成拼图所花时间进行星级打分。
- 参照Galgame的玩法，对完成的关卡开放对应鉴赏模式高清大图。

## 改进项
当前是切分为矩形的拼图块。可以简单的添加BoxCollider2D，即可以实现拖放效果。
如果欲实现非矩形的拼图块（例如相邻块之间互相凹凸嵌入对方的曲线边界），那么如何实现精确地拖放有待解决。

## android版app构建流程

1. 在Unity 3D 界面中以Gradle形式export出android工程；
2. 打开 Assembly-CSharp-Split.sln 解决方案，内含两个工程，分别用于生成基础dll代码和动态加载的dll代码；
3. 修改 Assembly-CSharp-init.csproj 工程的引用，使其指向导出的android工程中的相关dll；
4. 同理修改 Assembly-CSharp-dynamic.csproj 工程的引用，使其指向 init 工程生成的dll以及导出的android工程中的相关dll；
5. init与dynamic引用无误后，执行生成解决方案命令；
6. 复制init生成的Assembly-CSharp.dll，覆盖导出的android工程中的src/main/assets/bin/Data/Managed下的同名dll。
7. 生成apk
8. 使用Unity界面的Assets/Build AssetBundles命令生成AssetBundle包。Console里有指出输出路径；
9. 把第5步dynamic工程生成的Assembly-CSharp-dynamic.dll以及AssetBundle包放到web server目录。注意AssetBundle包放在ABs目录下，dll放在根目录即可。
